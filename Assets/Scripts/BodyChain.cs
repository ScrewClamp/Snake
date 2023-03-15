using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BodyChain : MonoBehaviour
{
	public GameObject bodyElement;

	public int maxBodyCount;

	private GameObject _playerHead;

	private FollowableComponent _followableComponent;

	private List<GameObject> _chain = new List<GameObject>();

	private MeshRenderer[] _chainMeshRenderers;

	private Material _bodyMaterial;

	private Color _newBodyColor;

	private bool _shouldSetNewColor;

	private int _chainBodyCount;

	private ChallengeItem _challengeItem;

	private void Awake()
	{
		base.GetComponent<PlayerLiveCalculator>().onPlayerLiveCountChanged.AddListener(new UnityAction<int>(this.OnPlayerLiveCountChanged));
		this._playerHead = GameObject.FindGameObjectWithTag("PlayerChainHead");
		this._followableComponent = this._playerHead.GetComponent<FollowableComponent>();
		MeshRenderer componentInChildren = this.bodyElement.GetComponentInChildren<MeshRenderer>();
		this._chainMeshRenderers = new MeshRenderer[this.maxBodyCount];
		if (componentInChildren != null)
		{
			this._bodyMaterial = componentInChildren.sharedMaterial;
		}
		this.CreateSnakeBody();
		AbstractChallengeProgress.OnSelectSkin = (Action<ChallengeItem>)Delegate.Combine(AbstractChallengeProgress.OnSelectSkin, new Action<ChallengeItem>(this.OnSelectSkin));
	}

	private void OnSelectSkin(ChallengeItem obj)
	{
		this._challengeItem = obj;
		for (int i = 0; i < this._chain.Count; i++)
		{
			GameObject gameObject = this._chain[i];
			gameObject.transform.GetChild(0).transform.localEulerAngles = obj.playerBodyRotation;
		}
	}

	private void CreateSnakeBody()
	{
		for (int i = 0; i < this.maxBodyCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bodyElement);
			gameObject.SetActive(false);
			gameObject.name = "Chain_" + i.ToString();
			this._chain.Add(gameObject);
			this._chainMeshRenderers[i] = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
		}
	}

	private void OnPlayerLiveCountChanged(int playerLiveCount)
	{
		int chainBodyCount = this._chainBodyCount;
		int num = playerLiveCount - 1;
		if (num > chainBodyCount)
		{
			int num2 = Mathf.Min(this.maxBodyCount, num) - chainBodyCount;
			for (int i = 0; i < num2; i++)
			{
				this.AddNewBodyPart(chainBodyCount, i);
				this._chainBodyCount++;
			}
		}
		else
		{
			num = Mathf.Max(0, num);
			for (int j = chainBodyCount - 1; j >= num; j--)
			{
				this._chain[j].SetActive(false);
				this._chainBodyCount--;
			}
		}
	}

	private void AddNewBodyPart(int initialChainCount, int i)
	{
		Vector3 pointAtDistanceFromHead = this._followableComponent.GetPointAtDistanceFromHead(initialChainCount + i + 1);
		FollowComponent component = this._chain[initialChainCount + i].GetComponent<FollowComponent>();
		component.gameObject.SetActive(true);
		component.following = this._followableComponent;
		component.GetComponent<FollowComponent>().index = initialChainCount + i + 1;
		MeshRenderer componentInChildren = component.GetComponentInChildren<MeshRenderer>();
		componentInChildren.sharedMaterial = this._bodyMaterial;
		if (this._shouldSetNewColor)
		{
			componentInChildren.sharedMaterial.color = this._newBodyColor;
		}
	}

	public void SetChainVisibility(bool visible)
	{
		MeshRenderer[] chainMeshRenderers = this._chainMeshRenderers;
		for (int i = 0; i < chainMeshRenderers.Length; i++)
		{
			MeshRenderer meshRenderer = chainMeshRenderers[i];
			meshRenderer.enabled = visible;
		}
	}

	public void SetChainColor(Color color, bool isNewColor)
	{
		foreach (GameObject current in this._chain)
		{
			if (current.GetComponentInChildren<MeshRenderer>() != null)
			{
				current.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = color;
			}
		}
		this._shouldSetNewColor = isNewColor;
		this._newBodyColor = color;
	}

	public void SetChainMaterial(Material material)
	{
		this._bodyMaterial = material;
		MeshRenderer[] chainMeshRenderers = this._chainMeshRenderers;
		for (int i = 0; i < chainMeshRenderers.Length; i++)
		{
			MeshRenderer meshRenderer = chainMeshRenderers[i];
			meshRenderer.sharedMaterial = material;
		}
	}
}
