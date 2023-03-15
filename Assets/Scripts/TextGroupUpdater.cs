using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

public class TextGroupUpdater : MonoBehaviour
{
	public TextMesh mainTextMesh;

	public TextMesh shadowTextMesh;

	public bool shouldAnimateAlpha;

	public void SetText(string text)
	{
		this.mainTextMesh.text = text;
		this.shadowTextMesh.text = text;
	}

	public string GetText()
	{
		return this.mainTextMesh.text;
	}

	public void SetColor(Color color)
	{
		this.mainTextMesh.color = color;
	}

	private void Start()
	{
		if (this.shouldAnimateAlpha)
		{
			DOTween.To(() => this.mainTextMesh.color.a, delegate(float alpha)
			{
				this.SetAlpha(alpha);
			}, 0f, 1f).SetEase(Ease.InSine);
		}
	}

	public void SetAlpha(float alpha)
	{
		Color color = this.shadowTextMesh.color;
		Color color2 = this.mainTextMesh.color;
		color.a = alpha;
		color2.a = alpha;
		this.shadowTextMesh.color = color;
		this.mainTextMesh.color = color2;
	}
}
