using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleGenerator : MonoBehaviour
{
	public GameObject player;

	public GameObject collectiblePrefab;

	public int maxCollectibleCount;

	public float maxDistanceFromCamera = 15f;

	public float leftPosition = 2f;

	public float rightPosition = -2f;

	private Transform _cameraHolder;

	private List<GameObject> _collectibles = new List<GameObject>();

	[SerializeField]
	private int _targetHpForLevelUp;

	[SerializeField]
	private float _hpProgressionDifference;

	private int _progressionNumber = 1;

	private GameState _gameState;

	public LevelProgress _levelProgress;

	[Range(0f, 10f), SerializeField]
	private float _periodInSeconds;

	[Range(-1f, 0f), SerializeField]
	private float _minDeltaPeriodInSeconds;

	[Range(0f, 1f), SerializeField]
	private float _maxDeltaPeriodInSeconds;

	[SerializeField]
	private float _levelMinDurationInSeconds = 20f;

	private float _currentPeriodInSeconds;

	private float _elapsedTime;

	private bool _shouldGenerate;

	[Range(0f, 100f), SerializeField]
	private float _sideDifficulty;

	[Range(1f, 100f), SerializeField]
	private float _noiseRangePercent;

	[Range(0f, 100f), SerializeField]
	private float _noiseProbability;

	[Range(0f, 100f), SerializeField]
	private float _noiseLowerBoundProbability;

	private bool _isOnLeftSideLastTime;

	private int _sameSideCreatedCount;

	private bool _shouldHelpPlayerWithCollectibles;

	private float _liveCountError;

	private bool _isGenerationEnabled;

	private bool _isCollectibleGeneratedOnCurrentBlock;

	private int _frontIndex;

	private int _rearIndex;

	private ChallengeItem _currentChallengeItem;

	public int TargetHpForLevelUp
	{
		get
		{
			return this._targetHpForLevelUp;
		}
		set
		{
			this._targetHpForLevelUp = value;
		}
	}

	public float HpProgressionDifference
	{
		get
		{
			return this._hpProgressionDifference;
		}
		set
		{
			this._hpProgressionDifference = value;
		}
	}

	public float LevelMinDurationInSeconds
	{
		get
		{
			return this._levelMinDurationInSeconds;
		}
		set
		{
			this._levelMinDurationInSeconds = value;
		}
	}

	public float PeriodInSeconds
	{
		get
		{
			return this._periodInSeconds;
		}
		set
		{
			this._periodInSeconds = value;
		}
	}

	public float MinDeltaPeriodInSeconds
	{
		get
		{
			return this._minDeltaPeriodInSeconds;
		}
		set
		{
			this._minDeltaPeriodInSeconds = value;
		}
	}

	public float MaxDeltaPeriodInSeconds
	{
		get
		{
			return this._maxDeltaPeriodInSeconds;
		}
		set
		{
			this._maxDeltaPeriodInSeconds = value;
		}
	}

	public float NoiseRangePercent
	{
		get
		{
			return this._noiseRangePercent;
		}
		set
		{
			this._noiseRangePercent = value;
		}
	}

	public float NoiseProbability
	{
		get
		{
			return this._noiseProbability;
		}
		set
		{
			this._noiseProbability = value;
		}
	}

	public float NoiseLowerBoundProbability
	{
		get
		{
			return this._noiseLowerBoundProbability;
		}
		set
		{
			this._noiseLowerBoundProbability = value;
		}
	}

	public bool IsCollectibleGeneratedOnCurrentBlock
	{
		get
		{
			return this._isCollectibleGeneratedOnCurrentBlock;
		}
	}

	private void Awake()
	{
		this._cameraHolder = Camera.main.transform.parent;
		this._gameState = base.GetComponent<GameState>();
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.ResetCollectibleProgression));
		this._gameState.OnGameIdleEvent.AddListener(new UnityAction(this.DisableCollectibleGenerator));
		this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.EnableCollectibleGenerator));
		LevelProgress expr_7B = this._levelProgress;
		expr_7B.onNextLevel = (Action)Delegate.Combine(expr_7B.onNextLevel, new Action(this.ResetCollectibleProgression));
		AbstractChallengeProgress.OnSelectSkin = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnSelectSkin, new Action<ChallengeItem>(this.OnSelectSkin));
		PlayerLiveCalculator expr_C7 = this.player.GetComponent<PlayerLiveCalculator>();
		expr_C7.PlayerNearToLoseLiveCountEvent = (Action)Delegate.Combine(expr_C7.PlayerNearToLoseLiveCountEvent, new Action(this.OnPlayerNearToLoseLiveCount));
		this.CreateCollectibles();
	}

	private void CreateCollectibles()
	{
		for (int i = 0; i < this.maxCollectibleCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.collectiblePrefab);
			gameObject.name = "collectible_" + i.ToString();
			gameObject.SetActive(false);
			gameObject.transform.GetChild(1).GetComponent<CollectibleBaseAnimator>().StartAnim();
			this._collectibles.Add(gameObject);
		}
		this._frontIndex = 0;
		this._rearIndex = 0;
	}

	private void OnSelectSkin(ChallengeItem challengeItem)
	{
		this._currentChallengeItem = challengeItem;
		GameObject[] array = this._collectibles.ToArray();
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject collectibleGameObject = array2[i];
			this.ChangeCollecibleMaterial(collectibleGameObject);
		}
	}

	private void ChangeCollecibleMaterial(GameObject collectibleGameObject)
	{
		MeshRenderer component = collectibleGameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
		component.material = this._currentChallengeItem.material;
		component.transform.localEulerAngles = this._currentChallengeItem.collectibleRotation;
		MeshRenderer component2 = collectibleGameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
		component2.sharedMaterial.color = this._currentChallengeItem.collectibleBaseColor;
		TextMesh component3 = collectibleGameObject.transform.GetChild(2).GetComponent<TextMesh>();
		component3.color = this._currentChallengeItem.collectibleTextColor;
	}

	private void OnPlayerNearToLoseLiveCount()
	{
		this._shouldHelpPlayerWithCollectibles = true;
	}

	public void OnBlockCreated(Vector3 blockPosition)
	{
		this._isCollectibleGeneratedOnCurrentBlock = false;
		if (this._shouldGenerate || this._shouldHelpPlayerWithCollectibles)
		{
			this.ResetTimer();
			this.GenerateCollectible(blockPosition);
			this.CheckToDestroyCollectible();
			this._isCollectibleGeneratedOnCurrentBlock = true;
			this._shouldHelpPlayerWithCollectibles = false;
		}
	}

	private void ResetTimer()
	{
		this._shouldGenerate = false;
		this.ResetElapsedTime();
		this.ReinitCurrentPeriod();
	}

	private void ResetElapsedTime()
	{
		int num = (int)(this._elapsedTime / this._currentPeriodInSeconds);
		float num2 = this._elapsedTime - this._currentPeriodInSeconds * (float)num;
		float num3 = num2 / this._currentPeriodInSeconds;
		this._elapsedTime = ((num3 <= 0.5f) ? Math.Max(0f, num2) : 0f);
	}

	private void ReinitCurrentPeriod()
	{
		this._currentPeriodInSeconds = this._periodInSeconds + UnityEngine.Random.Range(this._minDeltaPeriodInSeconds, this._maxDeltaPeriodInSeconds);
	}

	private void GenerateCollectible(Vector3 blockPosition)
	{
		GameObject gameObject = this._collectibles[this._frontIndex];
		gameObject.SetActive(true);
		this._frontIndex = (this._frontIndex + 1) % this._collectibles.Count;
		gameObject.GetComponent<Destroyer>().Reset();
		float x = (!this.IsOnLeftSide()) ? this.rightPosition : this.leftPosition;
		float y = blockPosition.y;
		Vector3 position = new Vector3(x, y, 0f);
		gameObject.transform.position = position;
		gameObject.GetComponent<ScoreComponent>().SetScore(this.GetLiveCount());
		this.IncrementProgression();
		this.ChangeCollecibleMaterial(gameObject);
	}

	private bool IsOnLeftSide()
	{
		bool flag = this.player.transform.position.x < 0f;
		float num = UnityEngine.Random.Range(0f, 100f);
		bool flag2 = num >= this._sideDifficulty || this._shouldHelpPlayerWithCollectibles;
		bool result = (!flag2) ? (!flag) : flag;
		this.LimitCollectiblesOnSameSide(ref result);
		return result;
	}

	private void LimitCollectiblesOnSameSide(ref bool isOnLeftSide)
	{
		bool flag = isOnLeftSide == this._isOnLeftSideLastTime;
		if (flag)
		{
			this._sameSideCreatedCount++;
			if (this._sameSideCreatedCount >= 2)
			{
				isOnLeftSide = !isOnLeftSide;
			}
		}
		else
		{
			this._sameSideCreatedCount = 0;
		}
		this._isOnLeftSideLastTime = isOnLeftSide;
	}

	private void ResetCollectibleProgression()
	{
		this._progressionNumber = 1;
	}

	private void EnableCollectibleGenerator()
	{
		this._isGenerationEnabled = true;
	}

	private void DisableCollectibleGenerator()
	{
		this._isGenerationEnabled = false;
		this._elapsedTime = 0f;
	}

	private int GetLiveCount()
	{
		float num = this.PeriodInSeconds + this._minDeltaPeriodInSeconds + this._maxDeltaPeriodInSeconds;
		float num2 = (float)this._targetHpForLevelUp / this._levelMinDurationInSeconds;
		num2 *= num;
		float b = 0.3f * (float)this._targetHpForLevelUp;
		float num3 = 0.2f;
		num2 += this._hpProgressionDifference * (float)(this._progressionNumber - 1);
		num2 = Mathf.Min(num2, b);
		float num4 = num2 * num3;
		float num5 = num2 - num4;
		float num6 = num2 + num4;
		float playerLiveCount;
		if (this.ShouldAddNoise())
		{
			playerLiveCount = this.GenerateNoiseHp(num5, num6);
		}
		else
		{
			playerLiveCount = UnityEngine.Random.Range(num5, num6);
		}
		return this.CorrectPlayerLiveCount(playerLiveCount);
	}

	private void IncrementProgression()
	{
		if (this._gameState.IsInGame())
		{
			this._progressionNumber++;
		}
	}

	private int CorrectPlayerLiveCount(float playerLiveCount)
	{
		int num = Mathf.FloorToInt(playerLiveCount);
		this._liveCountError += playerLiveCount - (float)num;
		if (this._liveCountError > 1f)
		{
			this._liveCountError -= 1f;
			num++;
		}
		return num;
	}

	private bool ShouldAddNoise()
	{
		return UnityEngine.Random.Range(0f, 100f) < this._noiseProbability;
	}

	private float GenerateNoiseHp(float minHp, float maxHp)
	{
		bool flag = UnityEngine.Random.value <= this._noiseLowerBoundProbability / 100f;
		float num = maxHp - minHp;
		float max = this._noiseRangePercent * num / 100f;
		float result;
		if (flag)
		{
			result = Mathf.Max(1f, minHp - UnityEngine.Random.Range(0f, max));
		}
		else
		{
			result = maxHp + UnityEngine.Random.Range(0f, max);
		}
		return result;
	}

	private void CheckToDestroyCollectible()
	{
		GameObject gameObject = this._collectibles[this._rearIndex];
		if (gameObject.activeSelf && this.IsOutsideCameraView(gameObject))
		{
			gameObject.SetActive(false);
			this._rearIndex = (this._rearIndex + 1) % this._collectibles.Count;
		}
	}

	private void Update()
	{
		this.UpdateTimer();
	}

	private void UpdateTimer()
	{
		if (this._gameState.IsInGame())
		{
			this._elapsedTime += Time.deltaTime;
		}
		this._shouldGenerate = (this._elapsedTime >= this._currentPeriodInSeconds && this._isGenerationEnabled);
	}

	private bool IsOutsideCameraView(GameObject firstCollectible)
	{
		float num = firstCollectible.transform.position.y - this._cameraHolder.position.y;
		return num < -this.maxDistanceFromCamera;
	}

	public void OnParseFinish()
	{
		this.ReinitCurrentPeriod();
	}
}
