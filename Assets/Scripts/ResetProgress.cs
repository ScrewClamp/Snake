using System;
using UnityEngine;

public class ResetProgress : MonoBehaviour
{
	public LevelManager levelManager;

	public void ResetGameProgress()
	{
		PlayerPrefs.DeleteAll();
		this.levelManager.CurrentLevelIndex = 0;
	}
}
