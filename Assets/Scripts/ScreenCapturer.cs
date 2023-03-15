using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScreenCapturer : MonoBehaviour
{
	private sealed class _CaptureScreenshot_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ScreenCapturer _this;

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

		public _CaptureScreenshot_c__Iterator0()
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
				ScreenCapture.CaptureScreenshot(string.Concat(new object[]
				{
					"ScreenShots/",
					this._this.prefix,
					"_",
					Screen.width,
					"x",
					Screen.height,
					".png"
				}));
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

	public bool shoot;

	public string prefix;

	private void Update()
	{
		if (this.shoot)
		{
			this.shoot = false;
			base.StartCoroutine(this.CaptureScreenshot());
		}
	}

	private IEnumerator CaptureScreenshot()
	{
		ScreenCapturer._CaptureScreenshot_c__Iterator0 _CaptureScreenshot_c__Iterator = new ScreenCapturer._CaptureScreenshot_c__Iterator0();
		_CaptureScreenshot_c__Iterator._this = this;
		return _CaptureScreenshot_c__Iterator;
	}
}
