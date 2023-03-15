using System;
using UnityEngine;
using UnityEngine.UI;

public class NewSkinAnimationController : MonoBehaviour
{
	[SerializeField]
	private Image _skinIcon;

	[SerializeField]
	private MenuConfig _menuConfig;

	[SerializeField]
	private SoundManager _soundManager;

	private Animator _animator;

	private Image _skinIconImage;

	public void Start()
	{
		this._animator = base.GetComponent<Animator>();
		AbstractChallengeProgress.OnItemUnlocked = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnItemUnlocked, new Action<ChallengeItem>(this.NewSkinUnlocked));
	}

	private void NewSkinUnlocked(ChallengeItem item)
	{
		UnityEngine.Debug.Log("new skin unlocked");
		this._skinIcon.sprite = item.challengeIcon;
		this._animator.SetTrigger("Activate");
		this._menuConfig.AddNewSkin();
		this._soundManager.PlayNewSkinUlocked();
	}
}
