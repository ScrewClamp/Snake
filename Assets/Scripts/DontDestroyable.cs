using System;
using UnityEngine;

public class DontDestroyable : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
