using System;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
	public List<string> tags = new List<string>();

	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		foreach (string current in this.tags)
		{
			if (other.gameObject.CompareTag(current))
			{
				Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Renderer renderer = array[i];
					renderer.enabled = false;
				}
				Collider2D componentInChildren = base.GetComponentInChildren<Collider2D>();
				if (componentInChildren != null)
				{
					componentInChildren.enabled = false;
				}
			}
		}
	}

	public void Reset()
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			renderer.enabled = true;
		}
		Collider2D componentInChildren = base.GetComponentInChildren<Collider2D>();
		if (componentInChildren != null)
		{
			componentInChildren.enabled = true;
		}
	}
}
