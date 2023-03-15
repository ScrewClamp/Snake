using DG.Tweening;
using System;
using UnityEngine;

public class TurnOverBlockOnPerfectTransition : MonoBehaviour
{
	public float turnDuration;

	[SerializeField]
	private GameObject textField;

	private int _blockNewHp;

	private bool _isRotateLeft;

	private TurnOverBlock _turnOverBlock;

	private void Start()
	{
		this._turnOverBlock = base.GetComponent<TurnOverBlock>();
	}

	public void TurnOver(bool isRotateLeft)
	{
		this._turnOverBlock._isRotated = !this._turnOverBlock._isRotated;
		this.FixRotation();
		this._isRotateLeft = isRotateLeft;
		this.RotateOneQuarter();
	}

	private void FixRotation()
	{
		base.transform.rotation = ((!this._turnOverBlock._isRotated) ? this._turnOverBlock._rotatedRotation : this._turnOverBlock._originalRotation);
		Vector3 localPosition = this.textField.transform.localPosition;
		localPosition.z = ((!this._turnOverBlock._isRotated) ? Mathf.Abs(localPosition.z) : (-Mathf.Abs(localPosition.z)));
		this.textField.transform.localPosition = localPosition;
		this.textField.transform.localRotation = ((!this._turnOverBlock._isRotated) ? this._turnOverBlock._rotatedRotation : this._turnOverBlock._originalRotation);
	}

	private void RotateOneQuarter()
	{
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.y += ((!this._isRotateLeft) ? 90f : -90f);
		base.transform.DORotate(eulerAngles, this.turnDuration / 2f, RotateMode.Fast).SetEase(Ease.InQuad).OnComplete(new TweenCallback(this.RotateLastQuarter));
	}

	private void RotateLastQuarter()
	{
		this.FixTextTransform();
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.y += ((!this._isRotateLeft) ? 90f : -90f);
		base.transform.DORotate(eulerAngles, this.turnDuration / 2f, RotateMode.Fast).SetEase(Ease.OutQuad);
	}

	private void FixTextTransform()
	{
		Vector3 localPosition = this.textField.transform.localPosition;
		localPosition.z = -localPosition.z;
		this.textField.transform.localPosition = localPosition;
		Vector3 eulerAngles = this.textField.transform.localRotation.eulerAngles;
		eulerAngles.y += 180f;
		this.textField.transform.localRotation = Quaternion.Euler(eulerAngles);
	}
}
