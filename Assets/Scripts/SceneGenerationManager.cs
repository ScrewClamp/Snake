using DG.Tweening;
using System;
using UnityEngine;

public class SceneGenerationManager : MonoBehaviour
{
	public bool isDebugLogEnabled = true;

	private BlockGenerator _blockGenerator;

	private ObstacleGenerator _obstacleGenerator;

	private CollectibleGenerator _collectibleGenerator;

	private PowerUpGenerator _powerUpGenerator;

	private void Awake()
	{
		UnityEngine.Debug.unityLogger.logEnabled = this.isDebugLogEnabled;
		DOTween.SetTweensCapacity(500, 50);
		this._blockGenerator = base.GetComponent<BlockGenerator>();
		this._obstacleGenerator = base.GetComponent<ObstacleGenerator>();
		this._collectibleGenerator = base.GetComponent<CollectibleGenerator>();
		this._powerUpGenerator = base.GetComponent<PowerUpGenerator>();
		BlockGenerator expr_52 = this._blockGenerator;
		expr_52.BlockCreatedEvent = (Action<Vector3, int>)Delegate.Combine(expr_52.BlockCreatedEvent, new Action<Vector3, int>(this.OnBlockCreated));
	}

	private void OnBlockCreated(Vector3 blockPosition, int blockHp)
	{
		this._obstacleGenerator.OnBlockCreated(blockPosition, blockHp);
		if (this._obstacleGenerator.LastObstacleDistance > 1)
		{
			this._collectibleGenerator.OnBlockCreated(blockPosition);
			if (!this._collectibleGenerator.IsCollectibleGeneratedOnCurrentBlock)
			{
				this._powerUpGenerator.OnBlockCreated(blockPosition);
			}
		}
	}
}
