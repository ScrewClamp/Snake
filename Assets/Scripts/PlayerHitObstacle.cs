using System;
using UnityEngine;

public class PlayerHitObstacle : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			base.GetComponent<BoxCollider2D>().enabled = false;
			GameObject gameObject = GameObject.FindGameObjectWithTag("GameManager");
			GameState component = gameObject.GetComponent<GameState>();
			component.RequestGameOver();
		}
	}
}
