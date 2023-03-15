using System;
using UnityEngine;

public class FollowComponent : MonoBehaviour
{
	public FollowableComponent following;

	public int index;

	private void Update()
	{
		Vector3 pointAtDistanceFromHead = this.following.GetPointAtDistanceFromHead(this.index);
		base.transform.position = pointAtDistanceFromHead;
	}
}
