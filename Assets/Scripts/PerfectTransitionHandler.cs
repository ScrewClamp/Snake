using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PerfectTransitionHandler : MonoBehaviour
{
	public Action OnPlayerHitBlock;

	public SoundManager soundManager;

	public float minTimeScale = 0.18f;

	public Vector3 newCameraRotation = new Vector3(-15f, 0f, 0f);

	[Range(0f, 1f)]
	public float playerNearYRatio = 0.5f;

	[Range(0f, 1f)]
	public float playerNearZRation = 0.05f;

	[Range(0f, 1f)]
	public float slowDownRatio = 0.38f;

	private bool _isPerfectTransition;

	private bool _isPerfectTransitionChecked;

	private float _transitionDuration;

	private Camera _mainCamera;

	private Vector3 _oldCameraPosition;

	private Vector3 _oldCameraRotation;

	private Vector3 _curveMiddlePoint;

	private PerfectWave _perfectWave;

	private float _blockHitCurveParam = float.PositiveInfinity;

	private static DOGetter<float> __f__mg_cache0;

	private static DOSetter<float> __f__am_cache0;

	private static DOGetter<float> __f__mg_cache1;

	private static DOSetter<float> __f__am_cache1;

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameManager");
		this._perfectWave = gameObject.GetComponent<PerfectWave>();
		this._mainCamera = Camera.main;
	}

	public void ResetPerfectTransition()
	{
		this._isPerfectTransitionChecked = false;
	}

	public void HandlePerfectTransition(HermitianCurve curveComponent, Vector3 curveStartPositon, float curveParam, float transitionDuration)
	{
		if (!this._isPerfectTransitionChecked)
		{
			this._isPerfectTransitionChecked = true;
			this.CalculatePerfectTransition(curveStartPositon, curveComponent);
			if (this._isPerfectTransition)
			{
				this.DetectCurveMiddlePoint(curveStartPositon, curveComponent);
				this.StartPerfectTransition(transitionDuration);
			}
		}
		if (curveParam >= this._blockHitCurveParam)
		{
			this._blockHitCurveParam = float.PositiveInfinity;
			this.OnPlayerHitBlock();
		}
	}

	private void CalculatePerfectTransition(Vector3 curveStartPositon, HermitianCurve curve)
	{
		this.FastSimulatePerfectTransition(curve, curveStartPositon);
	}

	private void SimulatePerfectTransition(float hitZoneStart, float hitZoneEnd, float simulationStep, HermitianCurve curve, Vector3 curveStartPositon)
	{
		this._isPerfectTransition = true;
		Physics2D.queriesHitTriggers = true;
		for (float num = hitZoneStart; num <= hitZoneEnd; num += simulationStep)
		{
			Vector2 pointOnCurve = curve.GetPointOnCurve(num);
			Vector3 v = curveStartPositon + (Vector3)pointOnCurve;
			Collider2D[] array = Physics2D.OverlapCircleAll(v, 0.2f);
			Collider2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Collider2D collider2D = array2[i];
				bool flag = collider2D.CompareTag("Block");
				this._isPerfectTransition &= !flag;
				if (flag)
				{
					this._blockHitCurveParam = num;
					HitBlock component = collider2D.gameObject.GetComponent<HitBlock>();
					component.SetupBlockHit();
					Physics2D.queriesHitTriggers = false;
					return;
				}
			}
		}
		Physics2D.queriesHitTriggers = false;
	}

	private void FastSimulatePerfectTransition(HermitianCurve curve, Vector3 curveStartPositon)
	{
		this._isPerfectTransition = true;
		Physics2D.queriesHitTriggers = true;
		float[] array = new float[]
		{
			0.3541929f,
			0.5005737f,
			0.4357821f,
			0.5639842f
		};
		for (int i = 0; i < array.Length; i++)
		{
			float num = array[i];
			Vector2 pointOnCurve = curve.GetPointOnCurve(num);
			Vector3 v = curveStartPositon + (Vector3)pointOnCurve;
			Collider2D[] array2 = Physics2D.OverlapCircleAll(v, 0.2f);
			Collider2D[] array3 = array2;
			for (int j = 0; j < array3.Length; j++)
			{
				Collider2D collider2D = array3[j];
				bool flag = collider2D.CompareTag("Block");
				this._isPerfectTransition &= !flag;
				if (flag)
				{
					this._blockHitCurveParam = num;
					HitBlock component = collider2D.gameObject.GetComponent<HitBlock>();
					component.SetupBlockHit();
					Physics2D.queriesHitTriggers = false;
					return;
				}
			}
		}
		Physics2D.queriesHitTriggers = false;
	}

	public float GetBlockHitCurveParam()
	{
		return this._blockHitCurveParam;
	}

	private void DetectCurveMiddlePoint(Vector3 curveStartPositon, HermitianCurve curveComponent)
	{
		Vector2 pointOnCurve = curveComponent.GetPointOnCurve(0.4f);
		this._curveMiddlePoint = curveStartPositon + (Vector3)pointOnCurve;
	}

	private void StartPerfectTransition(float transitionDuration)
	{
		this.SlowDownTime(transitionDuration);
		this.MoveAndRotateCamera();
	}

	private void SlowDownTime(float transitionDuration)
	{
		this._transitionDuration = transitionDuration;
		if (PerfectTransitionHandler.__f__mg_cache0 == null)
		{
			//PerfectTransitionHandler.__f__mg_cache0 = new DOGetter<float>(Time.get_timeScale);
		}
		DOTween.To(PerfectTransitionHandler.__f__mg_cache0, delegate(float x)
		{
			Time.timeScale = x;
		}, this.minTimeScale, this._transitionDuration * this.slowDownRatio).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.FadeOutSlowMotion));
	}

	private void MoveAndRotateCamera()
	{
		Vector3 b = base.transform.position - this._mainCamera.transform.position;
		b.x = 0f;
		b.y = this.playerNearYRatio * b.y;
		b.z = this.playerNearZRation * b.z;
		this._oldCameraPosition = this._mainCamera.transform.localPosition;
		this._oldCameraRotation = this._mainCamera.transform.localRotation.eulerAngles;
		this._mainCamera.transform.DOLocalMove(this._oldCameraPosition + b, this._transitionDuration * this.slowDownRatio, false).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(this.FadeOutCamera));
		this._mainCamera.transform.DOLocalRotate(this.newCameraRotation, this._transitionDuration * this.slowDownRatio, RotateMode.Fast).SetEase(Ease.OutQuad);
	}

	private void FadeOutSlowMotion()
	{
		this._perfectWave.StartWaveMotion(this._curveMiddlePoint, this._transitionDuration * (1f - this.slowDownRatio));
		this.soundManager.PlayPassingBetweenBlocks();
		if (PerfectTransitionHandler.__f__mg_cache1 == null)
		{
			//PerfectTransitionHandler.__f__mg_cache1 = new DOGetter<float>(Time.get_timeScale);
		}
		DOTween.To(PerfectTransitionHandler.__f__mg_cache1, delegate(float x)
		{
			Time.timeScale = x;
		}, 1f, this._transitionDuration * (1f - this.slowDownRatio)).SetEase(Ease.InQuad);
	}

	private void FadeOutCamera()
	{
		this._mainCamera.transform.DOLocalMove(this._oldCameraPosition, this._transitionDuration * (1f - this.slowDownRatio), false).SetEase(Ease.Linear);
		this._mainCamera.transform.DOLocalRotate(this._oldCameraRotation, this._transitionDuration * (1f - this.slowDownRatio), RotateMode.Fast).SetEase(Ease.Linear);
	}
}
