using System;
using UnityEngine;

public class HitBlock : MonoBehaviour
{
	public Action<float, GameObject, bool> onBlockHit;

	private PlayerController _playerController;

	private PerfectTransitionHandler _perfectTransitionHandler;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		this._playerController = gameObject.GetComponent<PlayerController>();
		this._perfectTransitionHandler = gameObject.GetComponent<PerfectTransitionHandler>();
	}

	public void SetupBlockHit()
	{
		PerfectTransitionHandler expr_06 = this._perfectTransitionHandler;
		expr_06.OnPlayerHitBlock = (Action)Delegate.Combine(expr_06.OnPlayerHitBlock, new Action(this.OnBlockHit));
	}

	private void OnBlockHit()
	{
		PerfectTransitionHandler expr_06 = this._perfectTransitionHandler;
		expr_06.OnPlayerHitBlock = (Action)Delegate.Remove(expr_06.OnPlayerHitBlock, new Action(this.OnBlockHit));
		float playerSpeedAlongCurve = this._playerController.GetPlayerSpeedAlongCurve();
		bool isLeft = this._playerController.isLeft;
		this._playerController.OnBlockHit();
		this.DispatchOnBlockHit(playerSpeedAlongCurve, isLeft);
	}

	private void DispatchOnBlockHit(float speed, bool isLeft)
	{
		if (this.onBlockHit != null)
		{
			this.onBlockHit(speed, base.gameObject, isLeft);
		}
	}
}
