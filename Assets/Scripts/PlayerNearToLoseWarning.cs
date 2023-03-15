using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNearToLoseWarning : MonoBehaviour
{
	public GameObject player;

	public GameObject warningView;

	public GameState gameState;

	public float offsetY;

	private bool _isWarningShown;

	private bool _wasPlayerLoosing;

	private void Start()
	{
		this.gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
	}

	private void OnGameOver()
	{
		this.HideWarning();
	}

	private void Update()
	{
		if (this.gameState.IsInGame())
		{
			this.HandleWarningCondition();
		}
	}

	private void HandleWarningCondition()
	{
		bool flag = this.IsPlayerNearToLose();
		bool flag2 = flag != this._wasPlayerLoosing;
		if (flag2)
		{
			if (flag)
			{
				this.ShowWarning();
			}
			else
			{
				this.HideWarning();
			}
		}
		this._wasPlayerLoosing = flag;
	}

	private bool IsPlayerNearToLose()
	{
		float y = this.player.transform.position.y;
		float y2 = this.warningView.transform.position.y;
		return y + this.offsetY < y2;
	}

	private void ShowWarning()
	{
		if (!this._isWarningShown)
		{
			this.warningView.SetActive(true);
		}
		this._isWarningShown = true;
	}

	private void HideWarning()
	{
		if (this._isWarningShown)
		{
			this.warningView.SetActive(false);
		}
		this._isWarningShown = false;
	}
}
