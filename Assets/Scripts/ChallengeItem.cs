using System;
using UnityEngine;

[Serializable]
public class ChallengeItem
{
	public string itemDescription;

	public Sprite challengeIcon;

	public Material material;

	public int maxValue;

	public int watchAdCounter;

	public bool isUnlockedByDefault;

	[Space(3f)]
	public Vector3 playerRotation = new Vector3(0f, 0f, 90f);

	public Vector3 playerBodyRotation = new Vector3(0f, 0f, 0f);

	public Vector3 collectibleRotation = new Vector3(180f, 0f, 0f);

	public Color collectibleBaseColor = new Color(0.1868993f, 0.288727f, 0.5283019f);

	public Color collectibleTextColor = new Color(0.1868993f, 0.288727f, 0.5283019f);

	private bool _isCompleted;

	private int _currentProgress;

	public int currentProgress
	{
		get
		{
			return this._currentProgress;
		}
		set
		{
			if (value >= this.maxValue)
			{
				this._isCompleted = true;
			}
			this._currentProgress = value;
		}
	}

	public bool IsChallengeCompleted()
	{
		return this._isCompleted || this.isUnlockedByDefault;
	}

	public void SetCompleted(bool isCompleted)
	{
		this._isCompleted = isCompleted;
	}
}
