using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackButton : MonoBehaviour
{
	public static List<Action> listeners = new List<Action>();

	private void Awake()
	{
		BackButton.listeners.Add(new Action(this.ExitGame));
	}

	private void ExitGame()
	{
		UnityEngine.Debug.Log(" Application.Quit(); ");
		Application.Quit();
	}

	private void Update()
	{
		if ((UnityEngine.Input.GetKeyDown(KeyCode.Escape) || UnityEngine.Input.GetKeyDown(KeyCode.Z)) && BackButton.listeners.Count > 0)
		{
			UnityEngine.Debug.Log(" listeners.Peek() ");
			BackButton.listeners.Last<Action>()();
		}
	}

	public static void RemoveLast()
	{
		if (BackButton.listeners.Count > 0)
		{
			BackButton.listeners.RemoveAt(BackButton.listeners.Count - 1);
		}
	}
}
