using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class ResetPlayer : MonoBehaviour
{
	public float playerSpawnOffset = 6f;

	public GameOverPopup gameOverPopup;

	public RestartPopup restartPopup;

	[SerializeField]
	private SkinsPopup _skinsPopup;

	private PlayerController _playerController;

	private ScoreSystem _scoreSystem;

	private GameObject _gameManager;

	private Transform _cameraHolder;

	private Vector3 _lowerMiddle;

	private GameState _gameState;

	private GameObject _playerHead;

	private CameraMovement _mainCameraMovement;

	private float _gameOverPopupShowTimePassed;

	private bool _isGameOverPopupShown;

	private bool _startImmediately;

	private void Start()
	{
		this._playerController = base.GetComponent<PlayerController>();
		this.gameOverPopup.OnClickEvent += new Action(this.OnGameOverClick);
		this._gameManager = GameObject.FindGameObjectWithTag("GameManager");
		this._gameState = this._gameManager.GetComponent<GameState>();
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.ResetGame));
		this._cameraHolder = Camera.main.transform.parent;
		this._mainCameraMovement = this._cameraHolder.GetComponent<CameraMovement>();
		this._playerHead = GameObject.FindGameObjectWithTag("PlayerChainHead");
		this._scoreSystem = this._gameManager.GetComponent<ScoreSystem>();
	}

	private void OnGameOverClick()
	{
		this._lowerMiddle = new Vector3(0f, this._cameraHolder.position.y, 0f);
		if (this._isGameOverPopupShown)
		{
			this._isGameOverPopupShown = false;
			this._startImmediately = true;
			this.StartNewGame();
		}
	}

	private void StartNewGame()
	{
		float playerNewPosition = this.GetPlayerNewPosition();
		if (this._startImmediately)
		{
			this.StartNewGameInternal(playerNewPosition, delegate
			{
				this._skinsPopup.ShowAvailableNewSkin();
				PlayerController component = base.GetComponent<PlayerController>();
				component.isTouchOn = true;
				base.GetComponentInChildren<Collider2D>().enabled = true;
				PlayerSpeed component2 = base.GetComponent<PlayerSpeed>();
				component2.ResetSpeed();
				component2.ResetAcceleration();
				this._gameState.currentState = GameState.State.InGame;
				this._startImmediately = false;
			});
		}
	}

	private void ShowRestartPopup()
	{
		this.restartPopup.Show();
	}

	private float GetPlayerNewPosition()
	{
		this._mainCameraMovement.RotateCameraRight();
		PlayerController component = base.GetComponent<PlayerController>();
		component.isTouchOn = false;
		float rightPosition = component.rightPosition;
		component.isLeft = true;
		return rightPosition;
	}

	private void StartNewGameInternal(float newPositionX, TweenCallback callback)
	{
		this._scoreSystem.Reset();
		this.gameOverPopup.Hide();
		this.SpawnNewPlayer(newPositionX);
		Vector3 endValue = new Vector3(newPositionX, this._cameraHolder.position.y + this.playerSpawnOffset, base.transform.position.z);
		base.transform.DOMove(endValue, 0.3f, false).OnComplete(callback);
	}

	private void SpawnNewPlayer(float newPositionX)
	{
		this._playerHead.GetComponent<FollowableComponent>().ResetTrail();
		base.transform.position = new Vector3(newPositionX, this._cameraHolder.position.y, base.transform.position.z);
		base.GetComponent<BodyChain>().SetChainVisibility(true);
		base.GetComponent<ScoreComponent>().SetScore(4);
		base.GetComponent<PlayerLiveCalculator>().onPlayerLiveCountChanged.Invoke(4);
		if (base.GetComponentInChildren<SpriteRenderer>() != null)
		{
			base.GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			MeshRenderer meshRenderer = array[i];
			meshRenderer.enabled = true;
		}
		Vector3 endValue = new Vector3(newPositionX, this._lowerMiddle.y + this.playerSpawnOffset, base.transform.position.z);
		base.transform.DOMove(endValue, 0.3f, false).OnComplete(delegate
		{
			PlayerController component = base.GetComponent<PlayerController>();
			this._gameState.currentState = GameState.State.Idle;
			component.isTouchOn = true;
			component.isFirstTouch = true;
			float defaultSpeed = this._cameraHolder.GetComponent<CameraMovement>().GetDefaultSpeed();
			component.SetSpeed(defaultSpeed);
			component.SetAcceleration(0f);
		});
	}

	public void ResetPlayerPositionOnSecondChance()
	{
		if (base.transform.position.y < this._cameraHolder.position.y + this.playerSpawnOffset)
		{
			float x = (float)((base.transform.position.x <= 0f) ? (-2) : 2);
			Vector3 endValue = new Vector3(x, this._cameraHolder.position.y + this.playerSpawnOffset, base.transform.position.z);
			base.transform.DOMove(endValue, 0.3f, false).OnComplete(delegate
			{
				this._playerController.IsGameOverRequested = false;
			});
		}
		else
		{
			this._playerController.IsGameOverRequested = false;
		}
	}

	private void ResetGame()
	{
		this.ShowGameOverPopup();
	}

	private void ShowGameOverPopup()
	{
		this.gameOverPopup.Show();
		if (base.GetComponentInChildren<SpriteRenderer>() != null)
		{
			base.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			MeshRenderer meshRenderer = array[i];
			meshRenderer.enabled = false;
		}
		base.GetComponentInChildren<Collider2D>().enabled = false;
		base.GetComponent<BodyChain>().SetChainVisibility(false);
		base.Invoke("ForceShowGameOverPopup", 0.2f);
	}

	private void ForceShowGameOverPopup()
	{
		this._isGameOverPopupShown = true;
	}
}
