using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Utils
{
	public static class ExtensionMethods
	{
		public static List<GameObject> SetAllActive(this List<GameObject> gameObjects, bool active)
		{
			foreach (GameObject current in gameObjects)
			{
				current.SetActive(active);
			}
			return gameObjects;
		}

		public static List<Image> SetOneActive(this List<Image> images, int active)
		{
			for (int i = 0; i < images.Count; i++)
			{
				images[i].gameObject.SetActive(i == active);
			}
			return images;
		}

		public static List<Image> SetAllActive(this List<Image> images, bool active)
		{
			foreach (Image current in images)
			{
				current.gameObject.SetActive(active);
			}
			return images;
		}
	}
}
