using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public Toggle soundToggleButton;

	[Space(10f)]
	public AudioSource backgroundMusicSource;

	public AudioSource blockExplosion;

	public AudioSource blockComboExplosion;

	public AudioSource collectibleHit;

	public AudioSource btnClick;

	public AudioSource newSkinUnlocked;

	public AudioSource gameOver;

	public AudioSource passingBetweenBlocks;

	public AudioSource skinSelect;

	public AudioSource levelUp;

	private float _previousTimeScale;

	private void Start()
	{
		bool flag = PlayerPrefs.GetInt("IsSoundOn", 1) == 1;
		this.soundToggleButton.isOn = flag;
		this.Mute(flag);
		this.soundToggleButton.onValueChanged.AddListener(new UnityAction<bool>(this.Mute));
	}

	private void Update()
	{
		bool flag = this._previousTimeScale != Time.timeScale;
		if (flag)
		{
			this.passingBetweenBlocks.pitch = Time.timeScale;
			this.levelUp.pitch = Time.timeScale;
			this._previousTimeScale = Time.timeScale;
		}
	}

	private void Mute(bool isSoundOn)
	{
		PlayerPrefs.SetInt("IsSoundOn", (!isSoundOn) ? 0 : 1);
		this.backgroundMusicSource.mute = !isSoundOn;
		this.backgroundMusicSource.mute = !isSoundOn;
		this.blockExplosion.mute = !isSoundOn;
		this.blockComboExplosion.mute = !isSoundOn;
		this.collectibleHit.mute = !isSoundOn;
		this.btnClick.mute = !isSoundOn;
		this.newSkinUnlocked.mute = !isSoundOn;
		this.gameOver.mute = !isSoundOn;
		this.passingBetweenBlocks.mute = !isSoundOn;
		this.skinSelect.mute = !isSoundOn;
		this.levelUp.mute = !isSoundOn;
		if (isSoundOn)
		{
			this.backgroundMusicSource.UnPause();
		}
		else
		{
			this.backgroundMusicSource.Pause();
		}
	}

	public void PlayBlockExplosion()
	{
		this.Play(this.blockExplosion);
	}

	public void PlayBlockComboExplosion()
	{
		this.Play(this.blockComboExplosion);
	}

	public void PlayCollectibleHit()
	{
		this.Play(this.collectibleHit);
	}

	public void PlayBtnClick()
	{
		this.Play(this.btnClick);
	}

	public void PlayNewSkinUlocked()
	{
		this.Play(this.newSkinUnlocked);
	}

	public void PlayGameOver()
	{
		this.Play(this.gameOver);
	}

	public void PlayPassingBetweenBlocks()
	{
		this.Play(this.passingBetweenBlocks);
	}

	public void PlaySkinSelect()
	{
		this.Play(this.skinSelect);
	}

	public void PlayLevelUp()
	{
		this.Play(this.levelUp);
	}

	private void Play(AudioSource audioSource)
	{
		if (!audioSource.mute)
		{
			audioSource.Play();
		}
	}
}
