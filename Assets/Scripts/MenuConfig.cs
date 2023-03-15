using System;
using UnityEngine;

public class MenuConfig : MonoBehaviour
{
	[SerializeField]
	private GameObject _exclamationMark;

	private void Start()
	{
		this.CheckNewSkinAvailable();
	}

	public bool IsNewSkinAvailable()
	{
		return PlayerPrefs.HasKey("newSkinAvailable") && PlayerPrefs.GetInt("newSkinAvailable") >= 1;
	}

	public void AddNewSkin()
	{
		int @int = PlayerPrefs.GetInt("newSkinAvailable", 0);
		PlayerPrefs.SetInt("newSkinAvailable", @int + 1);
		this._exclamationMark.SetActive(true);
	}

	public void CheckNewSkinAvailable()
	{
		this._exclamationMark.SetActive(this.IsNewSkinAvailable());
	}
}
