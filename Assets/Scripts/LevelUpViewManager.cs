using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class LevelUpViewManager : MonoBehaviour
{
	private sealed class _HandleLevelUp_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal LevelUpViewManager _this;

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

		public _HandleLevelUp_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				if (this._this._gameState.IsInGame())
				{
					this._this.PlayLevelUpParticle();
					this._this._soundManager.PlayLevelUp();
				}
				this._current = new WaitForSeconds(0.3f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._this._gameState.IsInGame())
				{
					this._this._levelManager.OnLevelUp();
				}
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

	[SerializeField]
	private SoundManager _soundManager;

	[SerializeField]
	private LevelManager _levelManager;

	[SerializeField]
	private GameObject _finishLine;

	[SerializeField]
	private PlayerLiveCalculator _playerLiveCalculator;

	[SerializeField]
	private ParticleSystem _levelUpParticle;

	private GameState _gameState;

	private void Start()
	{
		this._gameState = base.GetComponent<GameState>();
		this._playerLiveCalculator.PlayerReachedFinishLineEvent += new Action(this.OnPlayerReachedFinishLine);
		this._levelManager.EnoughScoreToLevelUpEvent += new Action(this.OnEnoughScoreToLevelUp);
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.HideFinishLine));
	}

	private void OnEnoughScoreToLevelUp()
	{
		this._finishLine.SetActive(true);
		Vector3 position = this._finishLine.transform.position;
		position.y = Camera.main.transform.parent.position.y + 10f;
		this._finishLine.transform.position = position;
		this._finishLine.GetComponentInChildren<Collider2D>().enabled = true;
	}

	private void HideFinishLine()
	{
		this._finishLine.GetComponentInChildren<Collider2D>().enabled = false;
		this._finishLine.SetActive(false);
	}

	private void OnPlayerReachedFinishLine()
	{
		if (this._gameState.IsInGame())
		{
			base.StartCoroutine(this.HandleLevelUp());
		}
	}

	private IEnumerator HandleLevelUp()
	{
		LevelUpViewManager._HandleLevelUp_c__Iterator0 _HandleLevelUp_c__Iterator = new LevelUpViewManager._HandleLevelUp_c__Iterator0();
		_HandleLevelUp_c__Iterator._this = this;
		return _HandleLevelUp_c__Iterator;
	}

	private void PlayLevelUpParticle()
	{
		this._levelUpParticle.Play();
	}
}
