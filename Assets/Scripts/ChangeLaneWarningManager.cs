using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ChangeLaneWarningManager : MonoBehaviour
{
	public const float leftPosition = -2f;

	public const float rightPosition = 2f;



	public GameState gameState;

	public GameObject warningIcon;

	public PlayerController playerController;

	[SerializeField]
	private float _timeoutToShowWarning;

	[SerializeField]
	private float _warningDuration;

	private float _elapsedTime;

	private bool _isWarningShown;

	public event Action<bool> RequestObstacleEvent;

	public float TimeoutToShowWarning
	{
		get
		{
			return this._timeoutToShowWarning;
		}
		set
		{
			this._timeoutToShowWarning = value;
		}
	}

	public float WarningDuration
	{
		get
		{
			return this._warningDuration;
		}
		set
		{
			this._warningDuration = value;
		}
	}

	private void Awake()
	{
		this.playerController.PlayerTransitionStartedEvent += new Action(this.OnPlayerTransitionStarted);
	}

	private void OnPlayerTransitionStarted()
	{
		this.ResetTimeout();
		this.HideWarning();
	}

	private void ResetTimeout()
	{
		this._elapsedTime = 0f;
	}

	private void HideWarning()
	{
		this._isWarningShown = false;
		this.warningIcon.SetActive(false);
	}

	public void Update()
	{
		if (this.gameState.IsInGame() && this.TimeoutToShowWarning > 0f)
		{
			this._elapsedTime += Time.deltaTime;
			this.CheckToShowWarning();
			this.CheckToRequestObstacle();
		}
		else if (this._isWarningShown)
		{
			this.ResetTimeout();
			this.HideWarning();
		}
	}

	private void CheckToRequestObstacle()
	{
		bool flag = this._elapsedTime >= this.TimeoutToShowWarning + this.WarningDuration;
		if (flag)
		{
			this.RequestObstacle();
			this.ResetTimeout();
		}
	}

	private void RequestObstacle()
	{
		if (this.RequestObstacleEvent != null)
		{
			this.RequestObstacleEvent(this.playerController.isLeft);
			this.HideWarning();
		}
	}

	private void CheckToShowWarning()
	{
		bool flag = !this._isWarningShown && this._elapsedTime >= this.TimeoutToShowWarning;
		if (flag)
		{
			this.ShowWarningIcon();
		}
	}

	private void ShowWarningIcon()
	{
		this._isWarningShown = true;
		Vector3 localPosition = this.warningIcon.transform.localPosition;
		localPosition.x = ((!this.playerController.isLeft) ? 2f : -2f);
		this.warningIcon.transform.localPosition = localPosition;
		this.warningIcon.SetActive(true);
	}
}
