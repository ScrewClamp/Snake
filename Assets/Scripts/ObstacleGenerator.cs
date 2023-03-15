using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
	public const float leftPosition = -3.1f;

	public const float rightPosition = 3.1f;

	public int maxObstacleCount;

	public GameObject obstaclePrefab;

	public GameObject obstacleWarningPrefab;

	public GameState gameState;

	public ChangeLaneWarningManager changeLaneWarningManager;

	public PlayerController playerController;

	public float maxDistanceFromCamera = 15f;

	private Transform _cameraHolder;

	private List<GameObject> _obstacles = new List<GameObject>();

	private ScoreComponent _playerScoreComponent;

	private int _previousBlockHp;

	[SerializeField]
	private float _minPeriodInSeconds;

	[SerializeField]
	private float _maxPeriodInSeconds;

	[Range(0f, 1f), SerializeField]
	private float _laneProbabilityForPlayer;

	private float _currentGenerationPeriodInSeconds;

	private float _elapsedTime;

	private bool _isWarningShown;

	private bool _shouldGenerateObstacleWithNextBlock;

	private bool _isRequestedObstacleOnLeftLane;

	private bool _isObstacleRequested;

	private int _frontIndex;

	private int _rearIndex;

	private int _LastObstacleDistance_k__BackingField;

	public float MinPeriodInSeconds
	{
		get
		{
			return this._minPeriodInSeconds;
		}
		set
		{
			this._minPeriodInSeconds = value;
			this.ResetGenerationPeriod();
		}
	}

	public float MaxPeriodInSeconds
	{
		get
		{
			return this._maxPeriodInSeconds;
		}
		set
		{
			this._maxPeriodInSeconds = value;
			this.ResetGenerationPeriod();
		}
	}

	public float LaneProbabilityForPlayer
	{
		get
		{
			return this._laneProbabilityForPlayer;
		}
		set
		{
			this._laneProbabilityForPlayer = value;
		}
	}

	public float TimeoutToShowWarning
	{
		get
		{
			return this.changeLaneWarningManager.TimeoutToShowWarning;
		}
		set
		{
			this.changeLaneWarningManager.TimeoutToShowWarning = value;
		}
	}

	public float WarningDuration
	{
		get
		{
			return this.changeLaneWarningManager.WarningDuration;
		}
		set
		{
			this.changeLaneWarningManager.WarningDuration = value;
		}
	}

	public int LastObstacleDistance
	{
		get;
		private set;
	}

	private void Awake()
	{
		this._cameraHolder = Camera.main.transform.parent;
		this.changeLaneWarningManager.RequestObstacleEvent += new Action<bool>(this.OnRequestObstacle);
		this._playerScoreComponent = this.playerController.GetComponent<ScoreComponent>();
		this.CreateObstacles();
	}

	private void CreateObstacles()
	{
		for (int i = 0; i < this.maxObstacleCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obstaclePrefab);
			gameObject.name = "Obstacle_" + i.ToString();
			gameObject.SetActive(false);
			this._obstacles.Add(gameObject);
		}
		this._frontIndex = 0;
		this._rearIndex = 0;
	}

	private void OnRequestObstacle(bool isRequestedObstacleOnLeftLane)
	{
		this._isRequestedObstacleOnLeftLane = isRequestedObstacleOnLeftLane;
		this._isObstacleRequested = true;
		float num = 0.5f;
		this._shouldGenerateObstacleWithNextBlock = (this._elapsedTime > num);
	}

	private void Reset()
	{
		this.ResetGenerationPeriod();
		this._elapsedTime = 0f;
		this.LastObstacleDistance = 0;
		this._shouldGenerateObstacleWithNextBlock = false;
		this._isObstacleRequested = false;
	}

	private void ResetGenerationPeriod()
	{
		this._currentGenerationPeriodInSeconds = UnityEngine.Random.Range(this._minPeriodInSeconds, this._maxPeriodInSeconds);
	}

	public void Update()
	{
		if (this.gameState.IsInGame())
		{
			this._elapsedTime += Time.deltaTime;
			if (this._currentGenerationPeriodInSeconds > 0f)
			{
				this.CheckToGenerateObstacle();
			}
		}
	}

	private void CheckToGenerateObstacle()
	{
		bool flag = !this._shouldGenerateObstacleWithNextBlock && this._elapsedTime >= this._currentGenerationPeriodInSeconds;
		if (flag)
		{
			this._shouldGenerateObstacleWithNextBlock = true;
		}
	}

	public void OnBlockCreated(Vector3 blockPosition, int blockHp)
	{
		this.LastObstacleDistance++;
		this.CheckToGenerateObstacle(blockPosition);
		this.CheckToDestroyObstacle();
		this._previousBlockHp = blockHp;
	}

	private void CheckToGenerateObstacle(Vector3 blockPosition)
	{
		if (this._shouldGenerateObstacleWithNextBlock && (float)this._previousBlockHp < 0.4f * (float)this._playerScoreComponent.GetScore())
		{
			this.GenerateObstacle(blockPosition);
			this.Reset();
		}
	}

	private void GenerateObstacle(Vector3 blockPosition)
	{
		bool flag = this.IsOnLeftLain();
		float x = (!flag) ? 3.1f : -3.1f;
		float y = blockPosition.y;
		Vector3 position = new Vector3(x, y, 0f);
		GameObject gameObject = this._obstacles[this._frontIndex];
		gameObject.GetComponent<FadeOutOnGameOver>().Reset();
		gameObject.SetActive(true);
		this._frontIndex = (this._frontIndex + 1) % this._obstacles.Count;
		gameObject.transform.position = position;
		ObstacleFollower component = UnityEngine.Object.Instantiate<GameObject>(this.obstacleWarningPrefab).GetComponent<ObstacleFollower>();
		component.target = gameObject;
	}

	private bool IsOnLeftLain()
	{
		if (this._isObstacleRequested)
		{
			return this._isRequestedObstacleOnLeftLane;
		}
		bool isLeft = this.playerController.isLeft;
		bool flag = UnityEngine.Random.value < this._laneProbabilityForPlayer;
		return (!flag) ? (!isLeft) : isLeft;
	}

	private void CheckToDestroyObstacle()
	{
		GameObject gameObject = this._obstacles[this._rearIndex];
		if (gameObject.activeSelf && this.IsOutsideCameraView(gameObject))
		{
			this._obstacles[this._rearIndex].SetActive(false);
			this._rearIndex = (this._rearIndex + 1) % this._obstacles.Count;
		}
	}

	private bool IsOutsideCameraView(GameObject firstObstacle)
	{
		float num = firstObstacle.transform.position.y - this._cameraHolder.position.y;
		return num < -this.maxDistanceFromCamera;
	}
}
