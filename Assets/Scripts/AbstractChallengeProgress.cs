using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractChallengeProgress : MonoBehaviour
{
	protected ChallengeItem challengeItem;

	public static Action<ChallengeItem> OnItemUnlocked;

	public static Action<ChallengeItem> OnSelectSkin;

	protected ChallengeManager _challengeManager;

	[SerializeField]
	private ChallengeProgressPopup _challengeProgressPopup;

	[SerializeField]
	private ChallengeCompletedPopup _challengeCompletedPopup;

	[SerializeField]
	private UnlockSkinByAd _unlockSkinByAd;

	[SerializeField]
	private SkinSelectorAnimation _selectedSkinImage;

	[SerializeField]
	private GameObject _newSkinMark;

	private Transform _selectorPlaceholder;

	private Vector3 _oldPosition;

	private string _uniqueChallengeName;

	private string _uniqueAdCounter;

	private void Start()
	{
		this.challengeItem = base.GetComponent<ChallengeItemComponent>().challengeItem;
		this._selectorPlaceholder = base.transform.parent.Find("SelectorPlaceholder");
		this._uniqueChallengeName = this.GetChallengeName() + base.gameObject.name + "Completed";
		this._uniqueAdCounter = this._uniqueChallengeName + "ad";
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameManager");
		this._challengeManager = gameObject.GetComponent<ChallengeManager>();
		if (this.challengeItem.isUnlockedByDefault && !PlayerPrefs.HasKey(this._uniqueChallengeName))
		{
			PlayerPrefs.SetInt(this._uniqueChallengeName, 1);
		}
		this.InitializeChallengeProgress();
		this.InitializeSelectedSkin();
		bool flag = PlayerPrefs.HasKey(this._uniqueChallengeName);
		if (flag)
		{
			if (this.IsNewSkin())
			{
				this._newSkinMark.SetActive(true);
			}
			this.challengeItem.SetCompleted(true);
			Image component = base.GetComponent<Image>();
			component.color = Color.white;
		}
		else
		{
			Image component2 = base.GetComponent<Image>();
			component2.color = Color.grey;
			this.RegisterListeners();
		}
	}

	private bool IsNewSkin()
	{
		return !PlayerPrefs.HasKey(this._uniqueChallengeName + "Selected");
	}

	private void InitializeSelectedSkin()
	{
		if (PlayerPrefs.HasKey("selectedSkin"))
		{
			int @int = PlayerPrefs.GetInt("selectedSkin");
			if (base.gameObject.name == @int.ToString())
			{
				this.SelectNewSkin();
			}
		}
		else if (base.gameObject.name == "1")
		{
			PlayerPrefs.SetInt(this._uniqueChallengeName + "Selected", 1);
			this.SelectNewSkin();
			PlayerPrefs.SetInt("selectedSkin", 1);
			PlayerPrefs.SetInt(this._uniqueChallengeName, 1);
		}
	}

	protected abstract void InitializeChallengeProgress();

	protected abstract void RegisterListeners();

	protected abstract string GetChallengeName();

	protected void UpdateChallengeProgress(int progress)
	{
		this.challengeItem.currentProgress = progress;
		bool flag = PlayerPrefs.HasKey(this._uniqueChallengeName);
		if (this.challengeItem.IsChallengeCompleted() && !flag)
		{
			this.UnlockItem();
		}
	}

	private void UnlockItem()
	{
		if (AbstractChallengeProgress.OnItemUnlocked != null)
		{
			this._newSkinMark.SetActive(true);
			PlayerPrefs.SetInt(this._uniqueChallengeName, 1);
			Image component = base.GetComponent<Image>();
			component.color = Color.white;
			AbstractChallengeProgress.OnItemUnlocked(this.challengeItem);
		}
	}

	private void UnlockItemWithoutAnimations()
	{
		this.challengeItem.SetCompleted(true);
		this._newSkinMark.SetActive(true);
		PlayerPrefs.SetInt(this._uniqueChallengeName, 1);
		Image component = base.GetComponent<Image>();
		component.color = Color.white;
		int @int = PlayerPrefs.GetInt("newSkinAvailable", 0);
		PlayerPrefs.SetInt("newSkinAvailable", @int + 1);
		this._challengeCompletedPopup.InitializePopup(this.challengeItem);
		this._challengeCompletedPopup.Show();
	}

	public void OnRequestChallengeProgress()
	{
		if (this.challengeItem.IsChallengeCompleted())
		{
			this.SelectNewSkin();
		}
		else
		{
			this._unlockSkinByAd.SetCurrentChallengeProgress(this);
			this.ShowSkinProgress();
		}
	}

	public void WatchSingleAd()
	{
		int num = PlayerPrefs.GetInt(this._uniqueAdCounter, 0);
		num++;
		if (num >= this.challengeItem.watchAdCounter)
		{
			this.UnlockItemWithoutAnimations();
		}
		PlayerPrefs.SetInt(this._uniqueAdCounter, num);
	}

	public int GetRemainingAdToWatchCount()
	{
		return Math.Max(0, this.challengeItem.watchAdCounter - PlayerPrefs.GetInt(this._uniqueAdCounter, 0));
	}

	private void SelectNewSkin()
	{
		if (this.IsNewSkin())
		{
			this._newSkinMark.SetActive(false);
			int @int = PlayerPrefs.GetInt("newSkinAvailable", 0);
			PlayerPrefs.SetInt("newSkinAvailable", @int - 1);
			PlayerPrefs.SetInt(this._uniqueChallengeName + "Selected", 1);
		}
		this._selectedSkinImage.transform.SetParent(this._selectorPlaceholder);
		this._selectedSkinImage.transform.localPosition = Vector3.zero;
		PlayerPrefs.SetInt("selectedSkin", int.Parse(base.gameObject.name));
		this.DispatchSelectNewSkin();
	}

	private void ShowSkinProgress()
	{
		this._challengeProgressPopup.InitializePopup(this.challengeItem);
		this._challengeProgressPopup.Show();
	}

	private void DispatchSelectNewSkin()
	{
		if (AbstractChallengeProgress.OnSelectSkin != null)
		{
			AbstractChallengeProgress.OnSelectSkin(this.challengeItem);
		}
	}
}
