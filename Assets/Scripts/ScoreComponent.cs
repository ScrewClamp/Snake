using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScoreComponent : MonoBehaviour
{
	private sealed class _ChangeTextByAnimation_c__AnonStorey0
	{
		internal int currentScoreInText;

		internal ScoreComponent _this;

		internal int __m__0()
		{
			return this.currentScoreInText;
		}

		internal void __m__1(int value)
		{
			this._this._textGroupUpdater.SetText(value.ToString());
		}
	}

	[SerializeField]
	private TextGroupUpdater _textGroupUpdater;

	private int _score;

	public int GetScore()
	{
		return this._score;
	}

	public void SetScore(int newScore)
	{
		if (base.transform.CompareTag("Player"))
		{
			this.ChangeTextByAnimation(newScore);
		}
		else
		{
			this._score = newScore;
			if (this._textGroupUpdater != null)
			{
				this._textGroupUpdater.SetText(this._score.ToString());
			}
			else
			{
				base.transform.GetComponentInChildren<TextMesh>().text = this._score.ToString();
			}
		}
	}

	private void ChangeTextByAnimation(int newScore)
	{
		if (this._textGroupUpdater != null)
		{
			int currentScoreInText = Convert.ToInt32(this._textGroupUpdater.GetText());
			DOTween.To(() => currentScoreInText, delegate(int value)
			{
				this._textGroupUpdater.SetText(value.ToString());
			}, newScore, 0.5f);
		}
		this._score = newScore;
	}
}
