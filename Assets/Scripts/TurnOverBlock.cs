using DG.Tweening;
using System;
using UnityEngine;

public class TurnOverBlock : MonoBehaviour
{
	public float turnDuration;

	[SerializeField]
	private GameObject textField;

	private int _blockNewHp;

	[HideInInspector]
	public bool _isRotated;

	[HideInInspector]
	public Quaternion _originalRotation;

	[HideInInspector]
	public Quaternion _rotatedRotation;

	private void Start()
	{
		this._originalRotation = Quaternion.identity;
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.y += 180f;
		this._rotatedRotation.eulerAngles = eulerAngles;
	}

	public void TurnOver(int blockNewHp)
	{
		this._isRotated = !this._isRotated;
		this.FixRotation();
		this._blockNewHp = blockNewHp;
		this.RotateOneQuarter();
	}

	private void FixRotation()
	{
		base.transform.rotation = ((!this._isRotated) ? this._rotatedRotation : this._originalRotation);
		Vector3 localPosition = this.textField.transform.localPosition;
		localPosition.z = ((!this._isRotated) ? Mathf.Abs(localPosition.z) : (-Mathf.Abs(localPosition.z)));
		this.textField.transform.localPosition = localPosition;
		this.textField.transform.localRotation = ((!this._isRotated) ? this._rotatedRotation : this._originalRotation);
	}

	private void RotateOneQuarter()
	{
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.y += 90f;
		base.transform.DORotate(eulerAngles, this.turnDuration / 2f, RotateMode.Fast).SetEase(Ease.InExpo).OnComplete(new TweenCallback(this.RotateLastQuarter));
	}

	private void RotateLastQuarter()
	{
		this.FixTextTransform();
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.y += 90f;
		base.transform.DORotate(eulerAngles, this.turnDuration / 2f, RotateMode.Fast).SetEase(Ease.OutExpo);
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
