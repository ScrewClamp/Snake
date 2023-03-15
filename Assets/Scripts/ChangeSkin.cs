using System;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
	[SerializeField]
	private GameObject _playerHead;

	private void Awake()
	{
		AbstractChallengeProgress.OnSelectSkin = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnSelectSkin, new Action<ChallengeItem>(this.ChangePlayerSkin));
	}

	private void ChangePlayerSkin(ChallengeItem item)
	{
		MeshRenderer componentInChildren = this._playerHead.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			componentInChildren.sharedMaterial = item.material;
			componentInChildren.transform.localEulerAngles = item.playerRotation;
		}
		base.GetComponent<BodyChain>().SetChainMaterial(item.material);
	}
}
