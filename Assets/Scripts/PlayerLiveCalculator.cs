using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PlayerLiveCalculator : MonoBehaviour
{
	public SoundManager soundManager;

	public GameObject collectibleBurstBallPrefab;

	public GameObject collectibleBurstParticlePrefab;

	[HideInInspector]
	public PlayerLiveCountChangedEvent onPlayerLiveCountChanged = new PlayerLiveCountChangedEvent();

	public Action<int> GameScoreChangedEvent;

	public Action<Vector3, int> CollectibleCollectedEvent;

	public Action<int> PlayerLiveUpdatedByBlockEvent;

	public Action PlayerNearToLoseLiveCountEvent;



	private TextGroupUpdater _textGroupUpdater;

	private ChallengeItem _challengeItem;

	private GameObject _gameManager;

	[HideInInspector]
	public bool isLevelUpScoreAlreadyChecked;

	public event Action PlayerReachedFinishLineEvent;

	private void Awake()
	{
		AbstractChallengeProgress.OnSelectSkin = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnSelectSkin, new Action<ChallengeItem>(this.OnSelectSkin));
	}

	private void OnSelectSkin(ChallengeItem obj)
	{
		this._challengeItem = obj;
	}

	private void Start()
	{
		this._gameManager = GameObject.FindGameObjectWithTag("GameManager");
		StringWave component = this._gameManager.GetComponent<StringWave>();
		base.GetComponent<ScoreComponent>().SetScore(4);
		this.onPlayerLiveCountChanged.Invoke(4);
		StringWave expr_35 = component;
		expr_35.onBlockDestroy = (Action<GameObject>)Delegate.Combine(expr_35.onBlockDestroy, new Action<GameObject>(this.OnPlayerHitBlock));
		this._textGroupUpdater = base.GetComponent<TextGroupUpdater>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		int num = base.GetComponent<ScoreComponent>().GetScore();
		if (other.CompareTag("Collectible"))
		{
			CollectibleBusrt component = UnityEngine.Object.Instantiate<GameObject>(this.collectibleBurstBallPrefab, other.transform.position + new Vector3(0f, -0.5f, 0f), other.transform.GetChild(0).rotation).GetComponent<CollectibleBusrt>();
			component.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = this._challengeItem.material.mainTexture;
			ParticleSystem component2 = UnityEngine.Object.Instantiate<GameObject>(this.collectibleBurstParticlePrefab).GetComponent<ParticleSystem>();
			component2.transform.position = other.transform.position;
			component2.startColor = this._challengeItem.collectibleTextColor;
			int liveCountFromCollectible = this.GetLiveCountFromCollectible(other);
			num += liveCountFromCollectible;
			this.soundManager.PlayCollectibleHit();
			this.UpdateGameScore(liveCountFromCollectible);
			base.GetComponent<ScoreComponent>().SetScore(num);
			this.onPlayerLiveCountChanged.Invoke(num);
		}
		else if (other.CompareTag("FinishLine"))
		{
			other.enabled = false;
			if (this.PlayerReachedFinishLineEvent != null)
			{
				this.PlayerReachedFinishLineEvent();
			}
		}
	}

	private void OnPlayerHitBlock(GameObject block)
	{
		int num = base.GetComponent<ScoreComponent>().GetScore();
		ArmorComponent component = base.GetComponent<ArmorComponent>();
		AutoBreakerComponent component2 = base.GetComponent<AutoBreakerComponent>();
		if (component != null)
		{
			component.HitArmor();
		}
		if (component == null && component2 == null)
		{
			num = this.SubtractBlockScore(block, num);
			this.DispatchPlayerLiveUpdatedByBlockEvent(num);
			if (num == 1)
			{
				this.DispatchPlayerNearToLoseLiveCountEvent();
			}
			base.GetComponent<ScoreComponent>().SetScore(num);
			this.onPlayerLiveCountChanged.Invoke(num);
		}
	}

	private void DispatchPlayerNearToLoseLiveCountEvent()
	{
		if (this.PlayerNearToLoseLiveCountEvent != null)
		{
			this.PlayerNearToLoseLiveCountEvent();
		}
	}

	private void DispatchPlayerLiveUpdatedByBlockEvent(int liveCount)
	{
		if (this.PlayerLiveUpdatedByBlockEvent != null)
		{
			this.PlayerLiveUpdatedByBlockEvent(liveCount);
		}
	}

	public void UpdateGameScore(int collectibleLiveCount)
	{
		if (!this.isLevelUpScoreAlreadyChecked)
		{
			if (this.CollectibleCollectedEvent != null)
			{
				this.CollectibleCollectedEvent(base.transform.position, collectibleLiveCount);
			}
			ScoreSystem component = this._gameManager.GetComponent<ScoreSystem>();
			component.UpdateScore(collectibleLiveCount);
			this.GameScoreChangedEvent(component.GetScore());
		}
	}

	private int GetLiveCountFromCollectible(Collider2D collectible)
	{
		return collectible.gameObject.GetComponent<ScoreComponent>().GetScore();
	}

	private int SubtractBlockScore(GameObject block, int liveCountToReduce)
	{
		int score = block.GetComponent<ScoreComponent>().GetScore();
		int num = liveCountToReduce - score;
		if (num < 1)
		{
			GameState component = this._gameManager.GetComponent<GameState>();
			if (!component.IsGameOver())
			{
				component.RequestGameOver();
			}
			num = 0;
		}
		return num;
	}
}
