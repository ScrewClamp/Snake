using DG.Tweening;
using System;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer _meshRenderer;

	[SerializeField]
	private float _animationDuration;

	private Material _material;

	private Tweener _tween;

	private Color _currentColor;

	public Material Material
	{
		get
		{
			return this._material;
		}
		set
		{
			this._material = value;
			this._meshRenderer.sharedMaterial = value;
		}
	}

	public Color CurrentColor
	{
		get
		{
			return this._currentColor;
		}
	}

	public void ChangeColor(Color color)
	{
		this._material.DOColor(color, "_Color", this._animationDuration);
		this._currentColor = color;
	}
}
