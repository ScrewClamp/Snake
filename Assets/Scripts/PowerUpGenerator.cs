using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpGenerator : MonoBehaviour
{
	public List<GameObject> powerUpPrefabs = new List<GameObject>();

	public float maxDistanceFromCamera = 15f;

	public float leftPosition = -2f;

	public float rightPosition = 2f;

	[Range(0f, 20f), SerializeField]
	private float _periodInSeconds;

	[Range(-3f, 0f), SerializeField]
	private float _minDeltaPeriodInSeconds;

	[Range(0f, 3f), SerializeField]
	private float _maxDeltaPeriodInSeconds;

	private GameState _gameState;

	private Transform _cameraHolder;

	private Queue<GameObject> _powerUps = new Queue<GameObject>();

	private float _elapsedTime;

	private float _currentPeriodInSeconds;

	private bool _shouldGenerate;

	private bool _isGenerationEnabled;

	public float PeriodInSeconds
	{
		get
		{
			return this._periodInSeconds;
		}
		set
		{
			this._periodInSeconds = value;
		}
	}

	public float MinDeltaPeriodInSeconds
	{
		get
		{
			return this._minDeltaPeriodInSeconds;
		}
		set
		{
			this._minDeltaPeriodInSeconds = value;
		}
	}

	public float MaxDeltaPeriodInSeconds
	{
		get
		{
			return this._maxDeltaPeriodInSeconds;
		}
		set
		{
			this._maxDeltaPeriodInSeconds = value;
		}
	}

	private void Awake()
	{
		this._cameraHolder = Camera.main.transform.parent;
		this._gameState = base.GetComponent<GameState>();
		this._gameState.OnGameIdleEvent.AddListener(new UnityAction(this.DisablePowerUpGenerator));
		this._gameState.OnGameStartedEvent.AddListener(new UnityAction(this.EnablePowerUpGenerator));
	}

	public void OnParseFinish()
	{
		this.ReinitCurrentPeriod();
	}

	public void OnBlockCreated(Vector3 blockPosition)
	{
		if (this._shouldGenerate && this._currentPeriodInSeconds != 0f)
		{
			this.ResetTimer();
			this.GeneratePowerUp(blockPosition);
			this.CheckToDestroyPowerUp();
		}
	}

	private void ResetTimer()
	{
		this._shouldGenerate = false;
		this.ResetElapsedTime();
		this.ReinitCurrentPeriod();
	}

	private void GeneratePowerUp(Vector3 blockPosition)
	{
		int index = UnityEngine.Random.Range(0, this.powerUpPrefabs.Count);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.powerUpPrefabs[index]);
		int num = UnityEngine.Random.Range(0, this.powerUpPrefabs.Count);
		float x = (num % 2 != 0) ? this.rightPosition : this.leftPosition;
		float y = blockPosition.y;
		Vector3 position = new Vector3(x, y, 0f);
		gameObject.transform.position = position;
		this._powerUps.Enqueue(gameObject);
	}

	private void CheckToDestroyPowerUp()
	{
		if (this._powerUps.Count > 0)
		{
			GameObject gameObject = this._powerUps.Peek();
			if (this.IsOutsideCameraView(gameObject))
			{
				this._powerUps.Dequeue();
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	private bool IsOutsideCameraView(GameObject firstCollectible)
	{
		float num = firstCollectible.transform.position.y - this._cameraHolder.position.y;
		return num < -this.maxDistanceFromCamera;
	}

	private void ResetElapsedTime()
	{
		this._elapsedTime = 0f;
	}

	private void ReinitCurrentPeriod()
	{
		this._currentPeriodInSeconds = this._periodInSeconds + UnityEngine.Random.Range(this._minDeltaPeriodInSeconds, this._maxDeltaPeriodInSeconds);
	}

	private void Update()
	{
		this.UpdateTimer();
	}

	private void EnablePowerUpGenerator()
	{
		this._isGenerationEnabled = true;
	}

	private void DisablePowerUpGenerator()
	{
		this._isGenerationEnabled = false;
		this._elapsedTime = 0f;
	}

	private void UpdateTimer()
	{
		if (this._gameState.IsInGame())
		{
			this._elapsedTime += Time.deltaTime;
		}
		this._shouldGenerate = (this._elapsedTime >= this._currentPeriodInSeconds && this._isGenerationEnabled);
	}
}
