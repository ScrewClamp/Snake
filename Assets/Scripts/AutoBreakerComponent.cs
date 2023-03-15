using System;
using UnityEngine;
using UnityEngine.Events;

public class AutoBreakerComponent : PowerUp
{
	public Sprite autoBreakerSprite;

	public float duration;

	public float speedIncreaseOffset;

	public SoundManager soundManager;

	private GameObject _head;

	private float _remainingAutoBreakerDuration;

	private Color _oldColor;

	private PlayerController _playerController;

	private PlayerSpeed _playerSpeed;

	private GameState _gameState;

	private LevelManager _levelManager;

	private bool _isPlayerNear;

	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		this._spriteRenderer = base.gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>();
		this._spriteRenderer.sprite = this.autoBreakerSprite;
		this._spriteRenderer.color = Color.red;
		GameObject gameObject = GameObject.Find("GameManager");
		this._gameState = gameObject.GetComponent<GameState>();
		GameObject gameObject2 = GameObject.Find("LevelManager");
		this._levelManager = gameObject2.GetComponent<LevelManager>();
		this._playerController = base.GetComponent<PlayerController>();
		this._playerSpeed = base.GetComponent<PlayerSpeed>();
		this._remainingAutoBreakerDuration = this.duration;
		this._head = GameObject.FindGameObjectWithTag("PlayerChainHead");
		this._oldColor = this._head.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
		GameObject gameObject3 = GameObject.Find("CameraHolder");
		CameraSpeed component = gameObject3.GetComponent<CameraSpeed>();
		component.SetAutoBreakerEnabled(true);
		this._playerController.SetAutoBreakerEnabled(true);
		this._playerSpeed.SetRelativeSpeed(this.speedIncreaseOffset);
		this._playerSpeed.SetAcceleration(0f);
		this._playerController.isTouchOn = false;
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.Finish));
		this._levelManager.LevelUpEvent += new Action<int>(this.FinishBooster);
	}

	private void Update()
	{
		this._remainingAutoBreakerDuration -= Time.deltaTime;
		if (this._remainingAutoBreakerDuration < 0f && !this._playerController.isTransitionStarted && !this._isPlayerNear)
		{
			this.Finish();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("PlayerTap"))
		{
			this._playerController.SimulateTap();
		}
		else if (collision.gameObject.CompareTag("PlayerNear"))
		{
			this._isPlayerNear = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("PlayerNear"))
		{
			this._isPlayerNear = false;
		}
	}

	private void FinishBooster(int newLevel)
	{
		this.Finish();
	}

	public sealed override void Finish()
	{
		GameObject gameObject = GameObject.Find("CameraHolder");
		CameraSpeed component = gameObject.GetComponent<CameraSpeed>();
		component.SetAutoBreakerEnabled(false);
		this._playerController.SetAutoBreakerEnabled(false);
		this._playerSpeed.SetRelativeSpeed(0f);
		this._playerSpeed.ResetAcceleration();
		this._playerController.isTouchOn = true;
		this._head.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = this._oldColor;
		this._gameState.OnGameOverEvent.RemoveListener(new UnityAction(this.Finish));
		this._levelManager.LevelUpEvent -= new Action<int>(this.FinishBooster);
		this._spriteRenderer.sprite = null;
		this._spriteRenderer.color = Color.white;
		UnityEngine.Object.Destroy(this);
	}
}
