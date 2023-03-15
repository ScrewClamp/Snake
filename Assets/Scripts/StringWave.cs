using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StringWave : MonoBehaviour
{
	private struct WaveItem
	{
		public GameObject block;

		public float speed;

		public WaveItem(GameObject _block, float _speed)
		{
			this.block = _block;
			this.speed = _speed;
		}
	}

	private sealed class _OnBlockHit_c__AnonStorey0
	{
		internal int currentIndex;

		internal StringWave _this;

		internal void __m__0()
		{
			this._this.FirstWaveEnded(this.currentIndex);
		}
	}

	private sealed class _FirstWaveEnded_c__AnonStorey1
	{
		internal int blockIndex;

		internal StringWave _this;

		internal void __m__0()
		{
			this._this.LastWave(this.blockIndex);
		}
	}

	public VibrationManager vibrationManager;

	public AnimationCurve WaveCurve;

	public int distantNeighborAffectedByWave = 6;

	public float wavePeriod = 0.1f;

	public float waveSpeed = 7f;

	public float waveLoseEnergy = 0.2f;

	public float longWavePeriodMultiplier = 1f;

	public float lastWavePeriodMultiplier = 0.5f;

	public Action<GameObject> onBlockDestroy;

	private BlockGenerator _blockGenerator;

	private List<StringWave.WaveItem> _affected = new List<StringWave.WaveItem>();

	private float _isLeftMultiplier;

	private int _endWaveCallbacks;

	private GameObject _currentBlockHit;

	private Vector3 _hitBlockPosition;

	private void Start()
	{
		this._blockGenerator = base.GetComponent<BlockGenerator>();
	}

	public void OnBlockHit(float speed, GameObject blockHit, bool isLeft)
	{
		this._hitBlockPosition = blockHit.transform.position;
		this.vibrationManager.VibrateOnBlockHit();
		speed = this.waveSpeed;
		List<GameObject> blocks = this._blockGenerator.GetBlocks();
		this._isLeftMultiplier = ((!isLeft) ? 1f : -1f);
		int blockIndex = this.GetBlockIndex(blocks, blockHit);
		for (int i = -this.distantNeighborAffectedByWave; i < this.distantNeighborAffectedByWave; i++)
		{
			int index = (blockIndex + i + blocks.Count) % blocks.Count;
			float time = (float)i / (float)this.distantNeighborAffectedByWave;
			float num = this.WaveCurve.Evaluate(time);
			float num2 = speed * num;
			GameObject gameObject = blocks[index];
			int currentIndex = this._affected.Count;
			if (i == 0)
			{
				this._currentBlockHit = gameObject;
				base.Invoke("DestroyBlock", this.wavePeriod);
			}
			gameObject.transform.DOMoveX(this._isLeftMultiplier * num2 * this.wavePeriod, this.wavePeriod, false).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.FirstWaveEnded(currentIndex);
			});
			this._affected.Add(new StringWave.WaveItem(gameObject, num2));
		}
	}

	private int GetBlockIndex(List<GameObject> blocks, GameObject blockHit)
	{
		int i;
		for (i = 0; i < blocks.Count; i++)
		{
			if (blocks[i] == blockHit)
			{
				break;
			}
		}
		return i;
	}

	private void DestroyBlock()
	{
		bool flag = this._hitBlockPosition.y + 1.401298E-45f < this._currentBlockHit.transform.position.y;
		if (flag)
		{
			Vector3 position = this._currentBlockHit.transform.position;
			position.x = 0f;
			this._currentBlockHit.transform.position = position;
		}
		else if (this.onBlockDestroy != null)
		{
			this.onBlockDestroy(this._currentBlockHit);
		}
	}

	private void FirstWaveEnded(int blockIndex)
	{
		StringWave.WaveItem waveItem = this._affected[blockIndex];
		if (blockIndex != this.distantNeighborAffectedByWave)
		{
			waveItem.speed = -waveItem.speed * this.waveLoseEnergy;
			waveItem.block.transform.DOMoveX(this._isLeftMultiplier * waveItem.speed * this.wavePeriod, this.longWavePeriodMultiplier * this.wavePeriod, false).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.LastWave(blockIndex);
			});
		}
	}

	private void LastWave(int blockIndex)
	{
		this._affected[blockIndex].block.transform.DOMoveX(0f, this.lastWavePeriodMultiplier * this.wavePeriod, false).SetEase(Ease.Linear).OnComplete(delegate
		{
			this._endWaveCallbacks++;
			if (this._endWaveCallbacks == this._affected.Count - 1)
			{
				this.CleanOldWave();
			}
		});
	}

	private void CleanOldWave()
	{
		this._affected.Clear();
		this._endWaveCallbacks = 0;
	}
}
