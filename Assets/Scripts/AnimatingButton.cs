using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatingButton : Button
{
	public Action onPointerDown;

	public Action onPointerUp;

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (this.onPointerDown != null)
		{
			this.onPointerDown();
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (this.onPointerUp != null)
		{
			this.onPointerUp();
		}
		base.OnPointerUp(eventData);
	}
}
