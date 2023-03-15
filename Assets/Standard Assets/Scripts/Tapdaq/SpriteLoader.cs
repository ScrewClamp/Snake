using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Tapdaq
{
	public class SpriteLoader : MonoBehaviour
	{
		private sealed class _LoadTexture_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal string url;

			internal WWW _www___0;

			internal Action<Texture2D> action;

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

			public _LoadTexture_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._www___0 = new WWW(this.url);
					this._current = this._www___0;
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				case 1u:
					if (this.action != null)
					{
						this.action(this._www___0.texture);
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

		private static SpriteLoader instance;

		public static SpriteLoader Instance
		{
			get
			{
				if (SpriteLoader.instance == null)
				{
					SpriteLoader[] array = UnityEngine.Object.FindObjectsOfType<SpriteLoader>();
					if (array.Length > 0)
					{
						SpriteLoader.instance = array[0];
					}
					else
					{
						SpriteLoader.instance = new GameObject("SpriteLoader").AddComponent<SpriteLoader>();
					}
				}
				return SpriteLoader.instance;
			}
		}

		public void LoadTextureAsync(string url, Action<Texture2D> action)
		{
			base.StartCoroutine(this.LoadTexture(url, action));
		}

		private IEnumerator LoadTexture(string url, Action<Texture2D> action)
		{
			SpriteLoader._LoadTexture_c__Iterator0 _LoadTexture_c__Iterator = new SpriteLoader._LoadTexture_c__Iterator0();
			_LoadTexture_c__Iterator.url = url;
			_LoadTexture_c__Iterator.action = action;
			return _LoadTexture_c__Iterator;
		}
	}
}
