using System;
using UnityEngine;

namespace Tayx.Graphy.Graph
{
	public abstract class Graph : MonoBehaviour
	{
		protected abstract void UpdateGraph();

		protected abstract void CreatePoints();
	}
}
