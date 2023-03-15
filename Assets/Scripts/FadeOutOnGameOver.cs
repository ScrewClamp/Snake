using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class FadeOutOnGameOver : MonoBehaviour
{
	public float fadeDuration;

	private GameState _gameState;

	private Collider2D[] _colliders;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameManager");
		this._gameState = gameObject.GetComponent<GameState>();
		this._gameState.OnRequestGameOverEvent.AddListener(new UnityAction(this.DisableGameObject));
	}

	public void DisableGameObject()
	{
		this.SetCollidersEnabled(false);
		this.FadeOutMeshes();
		this.SetSpritesEnabled(false);
	}

	private void SetCollidersEnabled(bool isEnabled)
	{
		Collider2D[] componentsInChildren = base.GetComponentsInChildren<Collider2D>();
		Collider2D[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Collider2D collider2D = array[i];
			collider2D.enabled = isEnabled;
		}
	}

	private void FadeOutMeshes()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sharedMaterial.DOFade(0f, this.fadeDuration).OnComplete(delegate
			{
				base.gameObject.SetActive(false);
			});
		}
	}

	private void SetSpritesEnabled(bool isEnabled)
	{
		SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}

	public void Reset()
	{
		this.SetCollidersEnabled(true);
		this.EnableMeshes();
		this.SetSpritesEnabled(true);
	}

	private void EnableMeshes()
	{
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Color color = componentsInChildren[i].sharedMaterial.color;
			color.a = 1f;
			componentsInChildren[i].sharedMaterial.color = color;
		}
	}
}
