using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButtonSound : MonoBehaviour
{
	private sealed class _PlayButtonClickOnEndOfFrame_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ToggleButtonSound _this;

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

		public _PlayButtonClickOnEndOfFrame_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForEndOfFrame();
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.soundManager.PlayBtnClick();
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

	public SoundManager soundManager;

	public Toggle toggleButton;

	private bool _isFistChange;

	private void Start()
	{
		this._isFistChange = true;
		this.toggleButton.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
	}

	private void OnValueChanged(bool isOn)
	{
		if (this._isFistChange)
		{
			this._isFistChange = false;
		}
		else
		{
			base.StartCoroutine(this.PlayButtonClickOnEndOfFrame());
		}
	}

	private IEnumerator PlayButtonClickOnEndOfFrame()
	{
		ToggleButtonSound._PlayButtonClickOnEndOfFrame_c__Iterator0 _PlayButtonClickOnEndOfFrame_c__Iterator = new ToggleButtonSound._PlayButtonClickOnEndOfFrame_c__Iterator0();
		_PlayButtonClickOnEndOfFrame_c__Iterator._this = this;
		return _PlayButtonClickOnEndOfFrame_c__Iterator;
	}
}
