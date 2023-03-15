using System;
using UnityEngine;

public class RewardFlierGenerator : MonoBehaviour
{
	public GameObject collectibleFlierPrefab;

	public PlayerLiveCalculator playerLiveCalculator;

	public StringWave blockStringWave;

	public PerfectWave blockPerfectWave;

	private ChallengeItem _challengeItem;

	private void Awake()
	{
		AbstractChallengeProgress.OnSelectSkin = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnSelectSkin, new Action<ChallengeItem>(this.OnSelectSkin));
	}

	public void Start()
	{
		PlayerLiveCalculator expr_06 = this.playerLiveCalculator;
		expr_06.CollectibleCollectedEvent = (Action<Vector3, int>)Delegate.Combine(expr_06.CollectibleCollectedEvent, new Action<Vector3, int>(this.CollectibleCollectedEvent));
		StringWave expr_2D = this.blockStringWave;
		expr_2D.onBlockDestroy = (Action<GameObject>)Delegate.Combine(expr_2D.onBlockDestroy, new Action<GameObject>(this.OnBlockDestroy));
		PerfectWave expr_54 = this.blockPerfectWave;
		expr_54.onPerfectWave = (Action<Vector3, int>)Delegate.Combine(expr_54.onPerfectWave, new Action<Vector3, int>(this.OnSpawnFlier));
	}

	private void OnSelectSkin(ChallengeItem obj)
	{
		this._challengeItem = obj;
	}

	private void OnBlockDestroy(GameObject block)
	{
		Vector3 position = block.transform.position;
		int score = block.GetComponent<ScoreComponent>().GetScore();
		this.OnSpawnFlier(position, score);
	}

	private void OnSpawnFlier(Vector3 flierPosition, int blockScore)
	{
		ObjectFlier objectFlier = this.CreateColectibleFlier(flierPosition, blockScore, Color.white);
		objectFlier.targetLocalPosition = new Vector3(2.95f, 5.13f, 12.57f);
		objectFlier.targetScale = objectFlier.transform.localScale * 1.5f;
	}

	private void CollectibleCollectedEvent(Vector3 playerPosition, int collectibleValue)
	{
		Vector3 position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
		ObjectFlier objectFlier = this.CreateColectibleFlier(position, collectibleValue, this._challengeItem.collectibleTextColor);
		objectFlier.targetLocalPosition = new Vector3(-3.37f, 6.95f, 12.57f);
		objectFlier.targetScale = objectFlier.transform.localScale * 2f;
	}

	private ObjectFlier CreateColectibleFlier(Vector3 position, int collectibleValue, Color color)
	{
		ObjectFlier component = UnityEngine.Object.Instantiate<GameObject>(this.collectibleFlierPrefab).GetComponent<ObjectFlier>();
		component.transform.parent = Camera.main.transform;
		Vector3 position2 = new Vector3(position.x, position.y, position.z);
		component.transform.position = position2;
		TextGroupUpdater component2 = component.GetComponent<TextGroupUpdater>();
		component2.SetText("+" + collectibleValue);
		component2.SetColor(color);
		return component;
	}
}
