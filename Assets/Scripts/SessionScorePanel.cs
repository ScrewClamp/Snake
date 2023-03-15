using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SessionScorePanel : MonoBehaviour
{
	private sealed class _OnScoreUpdated_c__AnonStorey0
	{
		internal int currentScoreInText;

		internal SessionScorePanel _this;

		internal int __m__0()
		{
			return this.currentScoreInText;
		}

		internal void __m__1(int value)
		{
			this._this._scoreText.text = value.ToString();
		}
	}

	[SerializeField]
	private UIElementGroup _uiElementGroup;

	[SerializeField]
	private SessionScoreManager _sessionScoreManager;

	[SerializeField]
	private GameObject _newBestScoreIcon;

	[SerializeField]
	private GameObject _regularScoreIcon;

	[SerializeField]
	private GameState _gameState;

	[SerializeField]
	private Text _scoreText;

	[SerializeField]
	private Transform _container;

	private void Start()
	{
		this._uiElementGroup.Hide();
		this._sessionScoreManager.ScoreUpdatedEvent += new Action(this.OnScoreUpdated);
		this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.OnGameStart));
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
	}

	private void OnGameOver()
	{
		this._regularScoreIcon.SetActive(false);
		this._uiElementGroup.Hide();
	}

	private void OnGameStart()
	{
		this._scoreText.text = "0";
		this._regularScoreIcon.SetActive(false);
		this._uiElementGroup.Show();
	}

	private void OnScoreUpdated()
	{
		this._regularScoreIcon.SetActive(this._sessionScoreManager.IsNewBestScore);
		int currentScoreInText = Convert.ToInt32(this._scoreText.text);
		DOTween.To(() => currentScoreInText, delegate(int value)
		{
			this._scoreText.text = value.ToString();
		}, this._sessionScoreManager.SessionScore, 0.5f);
	}
}
