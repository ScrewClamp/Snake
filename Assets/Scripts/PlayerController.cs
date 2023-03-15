using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{


	public RestartPopup restartPopup;

	public SoundManager soundManager;

	public VibrationManager vibrationManager;

	public float leftPosition = -2f;

	public float rightPosition = 2f;

	public float transitionPeriod = 0.3f;

	[HideInInspector]
	public bool isLeft = true;

	[HideInInspector]
	public bool isTouchOn = true;

	[HideInInspector]
	public bool isFirstTouch = true;

	[HideInInspector]
	public bool isTransitionStarted;

	private float _elapsedTime;

	private float _transitionLimitRatio = 0.99f;

	public Vector3 curveStartPositon;

	private Transform _cameraHolder;

	private CameraMovement _mainCameraMovement;

	private GameState _gameState;

	private PerfectTransitionHandler _perfectTransitionHandler;

	private PlayerSpeed _playerSpeed;

	private LevelProgress _levelProgress;

	private float _playerSpeedAlongCurve;

	[SerializeField]
	private int _tapCount;

	private bool _shouldUseComboCurve;

	private float _newTransitionPeriod;

	private bool _isAutoBreakerEnabled;

	private bool _isBlockHitOnLastTransition;

	private bool _isPassingBetweenBlocks;

	private Camera _camera;

	private float _cameraHalfHeight;

	public bool IsGameOverRequested;

	public event Action PlayerTransitionStartedEvent;

	private void Start()
	{
		this._camera = Camera.main;
		this._cameraHolder = this._camera.transform.parent;
		this._mainCameraMovement = this._cameraHolder.GetComponent<CameraMovement>();
		this._perfectTransitionHandler = base.GetComponent<PerfectTransitionHandler>();
		this._playerSpeed = base.GetComponent<PlayerSpeed>();
		this._cameraHalfHeight = this.CameraHalfHeight();
		GameObject gameObject = GameObject.FindGameObjectWithTag("LevelProgress");
		this._levelProgress = gameObject.GetComponent<LevelProgress>();
		this.SetSpeed(this._mainCameraMovement.GetDefaultSpeed());
		base.GetComponentInChildren<Collider2D>().enabled = false;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("GameManager");
		this._gameState = gameObject2.GetComponent<GameState>();
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
		this.FixPlayerSpeedAlongCurve();
	}

	private void OnGameOver()
	{
		this.ResetTapCount();
		this.IsGameOverRequested = false;
	}

	public void ResetTapCount()
	{
		this._tapCount = 0;
	}

	private void FixPlayerSpeedAlongCurve()
	{
		HermitianCurve hermitianCurve = base.GetComponents<HermitianCurve>()[0];
		HermitianCurve hermitianCurve2 = base.GetComponents<HermitianCurve>()[3];
		float num = hermitianCurve2.GetCurveLength() / this.transitionPeriod;
		this._newTransitionPeriod = hermitianCurve.GetCurveLength() / num;
		this._playerSpeedAlongCurve = num;
	}

	public void SetSpeed(float speed)
	{
		this._playerSpeed.SetSpeed(speed);
	}

	public void SetAcceleration(float acceleration)
	{
		this._playerSpeed.SetAcceleration(acceleration);
	}

	public void SetDefaultAcceleration(float acceleration)
	{
		this._playerSpeed.SetDefaultAcceleration(acceleration);
	}

	private void Update()
	{
		this.HandlePlayerMovement();
		if (this._gameState.IsInGame())
		{
			this.CheckPlayerOutsideCameraView();
			this.HandleTap();
		}
	}

	private void HandleTap()
	{
		bool flag = !this._levelProgress.IsOnLevelUpProcess();
		if (flag)
		{
			this.IncrementTapCount();
			if (this._tapCount > 0 && this.CouldHandleAnotherTransition())
			{
				this._tapCount--;
				this.HandleNewTransition();
				this.RotateCamera();
			}
		}
		else
		{
			this._tapCount = 0;
		}
	}

	private void HandlePlayerMovement()
	{
		if (this.isTransitionStarted)
		{
			this.MoveHeadAlongCurve();
		}
		else if (this._gameState.IsInGame())
		{
			if (!this.isFirstTouch && !this._playerSpeed._isSpeedWithInertiaStarted && !this._playerSpeed._isSpeedDecelerarionStarted)
			{
				this._playerSpeed.CalculateSpeed();
			}
			Vector3 b = this._playerSpeed.GetSpeed() * Vector3.up * Time.deltaTime;
			base.transform.position = base.transform.position + b;
		}
		else
		{
			Vector3 position = base.transform.position;
			position.x = this.rightPosition;
			position.y = this._cameraHolder.position.y - 3f;
			base.transform.position = position;
		}
	}

	public void SetAutoBreakerEnabled(bool isEnabled)
	{
		this._isAutoBreakerEnabled = isEnabled;
		this._playerSpeed.ResetSpeedWithInertia();
	}

	private void IncrementTapCount()
	{
		if (this.isTouchOn && (Input.GetMouseButtonDown(0) || UnityEngine.Input.GetKeyDown(KeyCode.A)) && this._tapCount < 3)
		{
			this._tapCount++;
			this.vibrationManager.VibrateOnClick();
		}
	}

	private void HandleNewTransition()
	{
		float x = (!this.isLeft) ? this.leftPosition : this.rightPosition;
		this.isTransitionStarted = true;
		float y = base.transform.position.y;
		this._elapsedTime = 0f;
		this._isBlockHitOnLastTransition = false;
		this._perfectTransitionHandler.ResetPerfectTransition();
		this.DispatchPlayerTransitionStartedEvent();
		this.curveStartPositon = new Vector3(x, y, base.transform.position.z);
		this.isLeft = !this.isLeft;
	}

	private bool CouldHandleAnotherTransition()
	{
		return !this.isTransitionStarted || this._elapsedTime / this.transitionPeriod > this._transitionLimitRatio;
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	public void SimulateTap()
	{
		this.isTouchOn = false;
		if (!this.isTransitionStarted)
		{
			this.isTransitionStarted = true;
			this._isBlockHitOnLastTransition = false;
			this._perfectTransitionHandler.ResetPerfectTransition();
			this._elapsedTime = 0f;
			this.isLeft = !this.isLeft;
			this.RotateCamera();
			this.curveStartPositon = base.transform.position;
		}
	}

	private void MoveHeadAlongCurve()
	{
		this._elapsedTime += Time.deltaTime;
		float num = (!this._shouldUseComboCurve) ? this._newTransitionPeriod : this.transitionPeriod;
		float num2 = this._elapsedTime / num;
		if (this._elapsedTime <= num)
		{
			this.CheckCombo(num2);
			HermitianCurve hermitianCurve = this.GetHermitianCurve(this._shouldUseComboCurve);
			this._perfectTransitionHandler.HandlePerfectTransition(hermitianCurve, this.curveStartPositon, num2, num);
			base.transform.position = this.GetPlayerPositionAlongCurve(hermitianCurve, num);
		}
		else
		{
			this.CheckCombo(num2);
			HermitianCurve hermitianCurve2 = this.GetHermitianCurve(this._shouldUseComboCurve);
			Vector3 playerPositionAlongCurve = this.GetPlayerPositionAlongCurve(hermitianCurve2, num);
			float num3 = this._elapsedTime - num;
			this.isTransitionStarted = false;
			this._shouldUseComboCurve = false;
			playerPositionAlongCurve.y += this._playerSpeedAlongCurve * num3;
			playerPositionAlongCurve.x = ((playerPositionAlongCurve.x >= 0f) ? this.leftPosition : this.rightPosition);
			base.transform.position = playerPositionAlongCurve;
			if (!this._isAutoBreakerEnabled)
			{
				this._playerSpeed.StartSpeedWithInertia(this._playerSpeedAlongCurve);
			}
		}
	}

	private Vector3 GetPlayerPositionAlongCurve(HermitianCurve curveComponent, float tp)
	{
		float curveLength = curveComponent.GetCurveLength();
		float num = curveLength / tp;
		float num2 = 2f * (num * tp - curveLength) / (tp * tp);
		float num3 = num - num2 * tp;
		float num4 = num3 * this._elapsedTime + num2 * this._elapsedTime * this._elapsedTime / 2f;
		float t = Math.Min(num4 / curveLength, 1f);
		Vector2 pointOnCurve = curveComponent.GetPointOnCurve(t);
		this._playerSpeedAlongCurve = curveLength / tp;
		return this.curveStartPositon + (Vector3)pointOnCurve;
	}

	private void CheckCombo(float t)
	{
		if (this._tapCount > 0 && t < 0.6f && !this._shouldUseComboCurve)
		{
			this._shouldUseComboCurve = true;
		}
	}

	public float GetPlayerSpeedAlongCurve()
	{
		return this._playerSpeedAlongCurve;
	}

	private HermitianCurve GetHermitianCurve(bool isCombo)
	{
		int num = 0;
		int num2 = 1;
		if (isCombo)
		{
			num = 2;
			num2 = 3;
		}
		return (!this.isLeft) ? base.GetComponents<HermitianCurve>()[num2] : base.GetComponents<HermitianCurve>()[num];
	}

	public void OnFirstTouch()
	{
		if (this.isFirstTouch)
		{
			base.GetComponentInChildren<Collider2D>().enabled = true;
			this.restartPopup.Hide();
			this._playerSpeed.ResetSpeed();
			this._playerSpeed.ResetAcceleration();
			if (!this._gameState.IsInGame())
			{
				this._gameState.currentState = GameState.State.InGame;
			}
			this.HandleNewTransition();
			this.RotateCamera();
		}
		this.isFirstTouch = false;
	}

	private void RotateCamera()
	{
		if (this.isLeft)
		{
			this._mainCameraMovement.RotateCameraRight();
		}
		else
		{
			this._mainCameraMovement.RotateCameraLeft();
		}
	}

	private float CalculateScreenLowerMiddlePoint()
	{
		float num = 1f;
		return this._cameraHolder.position.y - this._cameraHalfHeight - num;
	}

	private float CameraHalfHeight()
	{
		return -this._cameraHolder.position.z * Mathf.Tan(this._camera.fieldOfView * 0.5f * 0.0174532924f);
	}

	private void CheckPlayerOutsideCameraView()
	{
		float lowerMiddle = this.CalculateScreenLowerMiddlePoint();
		if (!this.IsGameOverRequested && this.IsPlayerOutsideCamera(lowerMiddle))
		{
			this.IsGameOverRequested = true;
			this._gameState.RequestGameOver();
		}
	}

	private bool IsPlayerOutsideCamera(float lowerMiddle)
	{
		return base.transform.position.y < lowerMiddle;
	}

	private void DispatchPlayerTransitionStartedEvent()
	{
		if (this.PlayerTransitionStartedEvent != null)
		{
			this.PlayerTransitionStartedEvent();
		}
	}

	public void OnBlockHit()
	{
		this._isBlockHitOnLastTransition = true;
	}
}
