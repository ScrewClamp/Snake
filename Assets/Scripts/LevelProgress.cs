using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
	public UIElementGroup levelUpPopup;

	public GameObject flagGameObject;

	public Text levelNumberText;

	public Text currentLevelText;

	public Text nextLevelText;

	public Image nextLevelBackgound;

	public Color defaultProgressColor;

	public BackgroundColor backgroundColor;

	[HideInInspector]
	public int currentScore;

	[SerializeField]
	private LevelManager _levelManager;

	public float fadeInStep;

	public AnimationCurve levelUpFadeIn;

	public float fadeOutStep;

	public AnimationCurve levelUpFadeOut;

	private Slider _slider;

	public Action onNextLevel;

	private bool _levelUpStart;

	private bool _nextLevelStart;

	private float _fadeInProgress = 1f;

	private float _fadeOutProgress;

	private float _sliderFillElapsedTime;

	private float _sliderFillDuration = 1f;

	private bool _shouldFillTheProgressBar;

	private float _previousSliderValue;

	private float _targetSliderValue;

	private bool _isOnLevelUpProcess;

	public GameState _gameState;

	private float _levelDuration;

	private void Awake()
	{
		this._slider = base.GetComponent<Slider>();
		this._previousSliderValue = this._slider.value;
		this._targetSliderValue = this._slider.value;
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
		this._levelManager.LevelUpEvent += new Action<int>(this.OnLevelUp);
		LevelManager expr_67 = this._levelManager;
		expr_67.LevelScoreUpdatedEvent = (Action)Delegate.Combine(expr_67.LevelScoreUpdatedEvent, new Action(this.OnLevelScoreUpdated));
	}

	private void OnGameOver()
	{
		this.ResetScore();
		this._levelDuration = 0f;
	}

	private void OnLevelScoreUpdated()
	{
		this.UpdateProgressBar(false);
	}

	private void OnLevelUp(int levelIndex)
	{
		if (this._gameState.IsInGame())
		{
			this._levelUpStart = true;
			this._levelDuration = 0f;
			this.UpdateNextLevelBackground();
		}
	}

	private void UpdateNextLevelBackground()
	{
		this.nextLevelBackgound.color = Color.white;
		this.nextLevelText.color = Color.black;
	}

	private void Update()
	{
		if (this._gameState.IsInGame())
		{
			this._levelDuration += Time.deltaTime;
		}
		this.HandleLevelUp();
		this.HandleNextLevel();
		this.HandleProgressBar();
	}

	private void HandleProgressBar()
	{
		if (this._shouldFillTheProgressBar)
		{
			if (this._previousSliderValue < this._targetSliderValue)
			{
				float num = 0.005f;
				this._previousSliderValue += num;
				this._slider.value = this._previousSliderValue;
			}
			else
			{
				this._previousSliderValue = this._targetSliderValue;
				this._shouldFillTheProgressBar = false;
			}
		}
	}

	private void HandleNextLevel()
	{
		if (this._nextLevelStart && this._gameState.IsInGame())
		{
			this._fadeOutProgress += this.fadeOutStep;
			Time.timeScale = this.levelUpFadeOut.Evaluate(this._fadeOutProgress);
			if (this._fadeInProgress >= 0.5f && this._isOnLevelUpProcess)
			{
				this._isOnLevelUpProcess = false;
			}
			if (this._fadeOutProgress >= 1f)
			{
				this._fadeOutProgress = 0f;
				Time.timeScale = 1f;
				this._nextLevelStart = false;
			}
		}
		else if (this._nextLevelStart && this._gameState.IsGameOver())
		{
			this._fadeOutProgress = 0f;
			this._fadeInProgress = 1f;
			Time.timeScale = 1f;
			this._nextLevelStart = false;
		}
	}

	private void HandleLevelUp()
	{
		if (this._levelUpStart && this._gameState.IsInGame())
		{
			this._fadeInProgress -= this.fadeInStep;
			Time.timeScale = this.levelUpFadeIn.Evaluate(this._fadeInProgress);
			if (this._fadeInProgress <= 0.5f && !this._isOnLevelUpProcess)
			{
				this._isOnLevelUpProcess = true;
			}
			if (this._fadeInProgress <= 0f)
			{
				this._fadeInProgress = 1f;
				Time.timeScale = 0f;
				this._levelUpStart = false;
				this.LevelUpInternal();
				Time.timeScale = 0f;
			}
		}
		else if (this._levelUpStart && this._gameState.IsGameOver())
		{
			this._fadeOutProgress = 0f;
			this._fadeInProgress = 1f;
			Time.timeScale = 1f;
			this._levelUpStart = false;
		}
	}

	private void LevelUpInternal()
	{
		if (this._gameState.IsInGame())
		{
			this.FillProgressBar();
			this.ShowLevelUpPopup();
			this.backgroundColor.ChangeBackgroundColor();
		}
	}

	private void ShowLevelUpPopup()
	{
		this.levelUpPopup.Show();
		this.levelNumberText.text = this._levelManager.CurrentLevelIndex.ToString();
	}

	private void UpdateLevelScores()
	{
		this.currentLevelText.text = this.GetLevelIndexToShow().ToString();
		this.nextLevelText.text = (this.GetLevelIndexToShow() + 1).ToString();
		this.nextLevelBackgound.color = this.defaultProgressColor;
		this.nextLevelText.color = Color.white;
	}

	public bool IsOnLevelUpProcess()
	{
		return this._isOnLevelUpProcess;
	}

	public void OnNextLevelButtonClickEditor()
	{
		this.levelUpPopup.Hide();
		this.ResetScore();
		this._nextLevelStart = true;
		this.onNextLevel();
	}

	private void UpdateProgressBar(bool instantaneousUpdate = false)
	{
		if (instantaneousUpdate)
		{
			this._slider.value = (float)this._levelManager.CurrentTargetHp / (float)this._levelManager.TargetHpForLevelUp;
			this._previousSliderValue = this._slider.value;
			this._targetSliderValue = this._slider.value;
			this._shouldFillTheProgressBar = false;
		}
		else
		{
			this._targetSliderValue = (float)this._levelManager.CurrentTargetHp / (float)this._levelManager.TargetHpForLevelUp;
			this._shouldFillTheProgressBar = true;
		}
	}

	private void FillProgressBar()
	{
		this._previousSliderValue = 1f;
		this._targetSliderValue = 1f;
		this._slider.value = 1f;
		this._shouldFillTheProgressBar = false;
	}

	public void ResetScore()
	{
		this.UpdateLevelScores();
		this.UpdateProgressBar(true);
	}

	public void DrawFlagInProgress(float progress)
	{
		float value = this._slider.value;
		this._slider.value = progress;
		this._slider.handleRect = this.flagGameObject.GetComponent<RectTransform>();
		this._slider.handleRect.gameObject.SetActive(true);
		this._slider.handleRect = null;
		this._slider.value = value;
	}

	public void HideFlag()
	{
		this.flagGameObject.SetActive(false);
	}

	public int GetLevelIndexToShow()
	{
		return this._levelManager.CurrentLevelIndex + 1;
	}
}
