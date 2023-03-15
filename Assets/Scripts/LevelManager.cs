using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
	private sealed class _ResetLevelUp_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal LevelManager _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _ResetLevelUp_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(0.3f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this._playerLiveCalculator.isLevelUpScoreAlreadyChecked = false;
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const string CurrentLevelIndexKey = "CurrentLevelIndex";







	public Action LevelScoreUpdatedEvent;

	[SerializeField]
	private PlayerLiveCalculator _playerLiveCalculator;

	[SerializeField]
	private GameState _gameState;

	private int _currentLevelIndex;

	private int _targetHpForLevelUp = 5;

	private int _currentTargetHp;

	public event Action<int> LevelUpEvent;

	public event Action JustBeforeLevelUpEvent;

	public event Action EnoughScoreToLevelUpEvent;

	public int CurrentLevelIndex
	{
		get
		{
			return this._currentLevelIndex;
		}
		set
		{
			this._currentLevelIndex = value;
		}
	}

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

	public int CurrentTargetHp
	{
		get
		{
			return this._currentTargetHp;
		}
		private set
		{
			this._currentTargetHp = value;
		}
	}

	private void Awake()
	{
		this._currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 0);
		PlayerLiveCalculator expr_17 = this._playerLiveCalculator;
		expr_17.GameScoreChangedEvent = (Action<int>)Delegate.Combine(expr_17.GameScoreChangedEvent, new Action<int>(this.OnGameScoreChanged));
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
	}

	private void OnGameOver()
	{
		this.CurrentTargetHp = 0;
		this._playerLiveCalculator.isLevelUpScoreAlreadyChecked = false;
	}

	private void OnGameScoreChanged(int newScore)
	{
		if (!this._playerLiveCalculator.isLevelUpScoreAlreadyChecked)
		{
			this.CurrentTargetHp = newScore;
			this.OnGameScoreChangedAfterDelay();
		}
	}

	private void OnGameScoreChangedAfterDelay()
	{
		this.DispatchLevelScoreUpdatedEvent();
		bool flag = this._targetHpForLevelUp <= this.CurrentTargetHp;
		if (flag && this._gameState.IsInGame())
		{
			this._playerLiveCalculator.isLevelUpScoreAlreadyChecked = true;
			this.CurrentTargetHp = 0;
			if (this.EnoughScoreToLevelUpEvent != null)
			{
				this.EnoughScoreToLevelUpEvent();
			}
		}
	}

	private IEnumerator ResetLevelUp()
	{
		LevelManager._ResetLevelUp_c__Iterator0 _ResetLevelUp_c__Iterator = new LevelManager._ResetLevelUp_c__Iterator0();
		_ResetLevelUp_c__Iterator._this = this;
		return _ResetLevelUp_c__Iterator;
	}

	public void OnLevelUp()
	{
		this._currentLevelIndex++;
		PlayerPrefs.SetInt("CurrentLevelIndex", this._currentLevelIndex);
		this.DispatchJustBeforeLevelUpEvent();
		this.DispatchLevelUpEvent();
		base.StartCoroutine(this.ResetLevelUp());
	}

	private void DispatchJustBeforeLevelUpEvent()
	{
		if (this.JustBeforeLevelUpEvent != null)
		{
			this.JustBeforeLevelUpEvent();
		}
	}

	private void DispatchLevelUpEvent()
	{
		if (this.LevelUpEvent != null)
		{
			this.LevelUpEvent(this._currentLevelIndex);
		}
	}

	private void DispatchLevelScoreUpdatedEvent()
	{
		if (this.LevelScoreUpdatedEvent != null)
		{
			this.LevelScoreUpdatedEvent();
		}
	}
}
