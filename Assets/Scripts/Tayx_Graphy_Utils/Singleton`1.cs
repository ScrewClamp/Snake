using System;
using UnityEngine;

namespace Tayx.Graphy.Utils
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		private static object _lock = new object();

		private static bool _applicationIsQuitting = false;

		public static T Instance
		{
			get
			{
				if (Singleton<T>._applicationIsQuitting)
				{
					return (T)((object)null);
				}
				object @lock = Singleton<T>._lock;
				T instance;
				lock (@lock)
				{
					if (Singleton<T>._instance == null)
					{
						Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
						if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
						{
							instance = Singleton<T>._instance;
							return instance;
						}
						if (Singleton<T>._instance == null)
						{
							UnityEngine.Debug.Log(string.Concat(new object[]
							{
								"[Singleton] An instance of ",
								typeof(T),
								" is trying to be accessed, but it wasn't initialized first. Make sure to add an instance of ",
								typeof(T),
								" in the scene before  trying to access it."
							}));
						}
					}
					instance = Singleton<T>._instance;
				}
				return instance;
			}
		}

		private void Awake()
		{
			if (Singleton<T>._instance != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				Singleton<T>._instance = base.GetComponent<T>();
			}
		}

		public void OnDestroy()
		{
			Singleton<T>._applicationIsQuitting = true;
		}
	}
}
