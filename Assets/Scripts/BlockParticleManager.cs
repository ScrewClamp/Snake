using System;
using UnityEngine;

public class BlockParticleManager : MonoBehaviour
{
	public SoundManager soundManager;

	public PlayerLiveCalculator playerLiveCalculator;

	public GameObject comboBlockParticlePrefab;

	public GameObject destroyParticlePrefab;

	private StringWave _stringWave;

	public float comboTrasholdInSeconds;

	private float _previousBlockDestroyTime;

	private int _comboCount;

	public int ComboCount
	{
		get
		{
			return this._comboCount;
		}
	}

	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameManager");
		this._stringWave = gameObject.GetComponent<StringWave>();
		StringWave expr_1D = this._stringWave;
		expr_1D.onBlockDestroy = (Action<GameObject>)Delegate.Combine(expr_1D.onBlockDestroy, new Action<GameObject>(this.OnBlockDestroy));
	}

	private void OnBlockDestroy(GameObject block)
	{
		Vector3 position = block.transform.position;
		MeshRenderer componentInChildren = block.transform.Find("Content/View").GetComponentInChildren<MeshRenderer>();
		Material material = componentInChildren.material;
		float num = Time.time - this._previousBlockDestroyTime;
		if (num <= this.comboTrasholdInSeconds)
		{
			this.RunParticleOnBlockDestroy(this.comboBlockParticlePrefab, position, material);
			this._comboCount++;
			this.playerLiveCalculator.UpdateGameScore(this._comboCount);
		}
		else
		{
			this.RunParticleOnBlockDestroy(this.destroyParticlePrefab, position, material);
			this._comboCount = 0;
		}
		this._previousBlockDestroyTime = Time.time;
		this.DestroyBlock(block);
	}

	private void RunParticleOnBlockDestroy(GameObject prefab, Vector3 blockPosition, Material material)
	{
		this.soundManager.PlayBlockExplosion();
		bool flag = blockPosition.x < 0f;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
		Color color = material.color;
		component.transform.position += blockPosition;
		component.startColor = material.color;
		if (!flag)
		{
			component.transform.Rotate(Vector3.forward, -180f);
		}
		ParticleSystemRenderer component2 = gameObject.GetComponent<ParticleSystemRenderer>();
		component2.material = material;
		GameObject gameObject2 = gameObject.transform.Find("Bg").gameObject;
		SpriteRenderer component3 = gameObject2.GetComponent<SpriteRenderer>();
		float a = component3.color.a;
		component3.color = new Color(color.r, color.g, color.b, a);
		gameObject2.GetComponent<ParticleBgAnimator>().rotateZAngle = (float)((!flag) ? (-10) : 10);
		Vector3 position = gameObject2.transform.position;
		position.x = 0f;
		gameObject2.transform.position = position;
	}

	private void RunParticleOnCombo(Vector3 blockPosition, Material material)
	{
		Color color = material.color;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.comboBlockParticlePrefab);
		ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
		component.transform.position += blockPosition;
		ParticleSystemRenderer component2 = gameObject.GetComponent<ParticleSystemRenderer>();
		component2.material = material;
		this.soundManager.PlayBlockComboExplosion();
	}

	private void DestroyBlock(GameObject block)
	{
		Renderer[] componentsInChildren = block.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			renderer.enabled = false;
		}
		Collider2D componentInChildren = block.GetComponentInChildren<Collider2D>();
		if (componentInChildren != null)
		{
			componentInChildren.enabled = false;
		}
	}
}
