using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public float duration = 0.5f;

	public List<Color> colors = new List<Color>();

	private Material _material;

	private Color _newColor;

	private void Start()
	{
		this._material = base.GetComponent<MeshRenderer>().sharedMaterial;
		this._newColor = this.colors.First<Color>();
		this.ChangeBackgroundColor();
	}

	public void ChangeBackgroundColor()
	{
		int index = UnityEngine.Random.Range(0, this.colors.Count);
		this._newColor = this.colors[index];
		this.spriteRenderer.DOColor(this._newColor, this.duration);
	}
}
