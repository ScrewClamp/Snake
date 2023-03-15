using System;
using UnityEngine;
using UnityEngine.Events;

public class ArmorComponent : PowerUp
{
	public Sprite armorSprite;

	public SoundManager soundManager;

	public int armorHp = 5;

	private int _currentArmorHp;

	private GameObject _head;

	private Color _oldColor;

	private GameState _gameState;

	private LevelManager _levelManager;

	private SpriteRenderer _spriteRenderer;

	private void Start()
	{
		this._spriteRenderer = base.gameObject.transform.GetChild(2).GetComponent<SpriteRenderer>();
		this._spriteRenderer.sprite = this.armorSprite;
		this._spriteRenderer.color = Color.yellow;
		GameObject gameObject = GameObject.Find("GameManager");
		this._gameState = gameObject.GetComponent<GameState>();
		GameObject gameObject2 = GameObject.Find("LevelManager");
		this._levelManager = gameObject2.GetComponent<LevelManager>();
		this._currentArmorHp = this.armorHp;
		this._head = GameObject.FindGameObjectWithTag("PlayerChainHead");
		this._oldColor = this._head.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
		this._gameState.OnGameOverEvent.AddListener(new UnityAction(this.Finish));
		this._levelManager.LevelUpEvent += new Action<int>(this.FinishBooster);
	}

	public sealed override void Finish()
	{
		this._head.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = this._oldColor;
		this._gameState.OnGameOverEvent.RemoveListener(new UnityAction(this.Finish));
		this._levelManager.LevelUpEvent -= new Action<int>(this.FinishBooster);
		this._spriteRenderer.sprite = null;
		this._spriteRenderer.color = Color.white;
		UnityEngine.Object.Destroy(this);
	}

	private void FinishBooster(int newLevel)
	{
		this.Finish();
	}

	public void HitArmor()
	{
		this._currentArmorHp--;
		if (this._currentArmorHp <= 0)
		{
			this.Finish();
		}
	}
}
