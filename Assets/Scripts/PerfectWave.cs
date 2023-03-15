using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PerfectWave : MonoBehaviour
{
	public int perfectWaveParticleCount;

	public GameObject blockFrictionParticle;

	public PlayerController playerController;

	public float brickMoveOffset;

	public float rotationAngle;

	public int score;

	public Action<Vector3, int> onPerfectWave;

	private GameObject[] _particles;

	private int _startIndex;

	private BlockGenerator _blockGenerator;

	private void Start()
	{
		this._blockGenerator = base.GetComponent<BlockGenerator>();
		this.CreateParticles();
	}

	private void CreateParticles()
	{
		this._particles = new GameObject[this.perfectWaveParticleCount];
		for (int i = 0; i < this.perfectWaveParticleCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.blockFrictionParticle);
			gameObject.SetActive(false);
			this._particles[i] = gameObject;
		}
		this._startIndex = 0;
	}

	public void StartWaveMotion(Vector3 curveMiddlePoint, float duration)
	{
		List<GameObject> blocks = this._blockGenerator.GetBlocks();
		int neighbourBlocksStartIndex = this.GetNeighbourBlocksStartIndex(blocks, curveMiddlePoint);
		Transform transform = blocks[neighbourBlocksStartIndex].transform;
		Transform transform2 = blocks[(neighbourBlocksStartIndex + 1) % blocks.Count].transform;
		transform.GetComponent<TurnOverBlockOnPerfectTransition>().TurnOver(this.playerController.isLeft);
		transform2.GetComponent<TurnOverBlockOnPerfectTransition>().TurnOver(this.playerController.isLeft);
		Transform child = transform.transform.GetChild(0);
		Transform child2 = transform2.transform.GetChild(0);
		Vector3 localPosition = child.localPosition;
		Vector3 localPosition2 = child2.localPosition;
		Vector3 vector = localPosition + this.brickMoveOffset * Vector3.down;
		Vector3 vector2 = localPosition2 + this.brickMoveOffset * Vector3.up;
		Vector3 eulerAngles = transform.localRotation.eulerAngles;
		Vector3 eulerAngles2 = transform2.localRotation.eulerAngles;
		Vector3 vector3 = eulerAngles + this.rotationAngle * Vector3.left;
		Vector3 vector4 = eulerAngles2 + this.rotationAngle * Vector3.right;
		Vector3 position = transform.position;
		position.z = this.blockFrictionParticle.transform.position.z;
		Quaternion rotation = (!this.playerController.isLeft) ? Quaternion.identity : Quaternion.Euler(new Vector3(0f, 0f, 180f));
		GameObject gameObject = this._particles[this._startIndex];
		ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
		gameObject.SetActive(true);
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		this._startIndex = (this._startIndex + 1) % this.perfectWaveParticleCount;
		GameObject gameObject2 = this._particles[this._startIndex];
		ParticleSystem component2 = gameObject2.GetComponent<ParticleSystem>();
		if (gameObject2.activeSelf)
		{
			gameObject2.SetActive(false);
		}
		gameObject2.SetActive(true);
		gameObject2.transform.position = position;
		gameObject2.transform.rotation = rotation;
		Color color = child.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial.color;
		Color color2 = child2.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial.color;
		component.startColor = color;
		component2.startColor = color2;
		if (this.onPerfectWave != null)
		{
			this.onPerfectWave(curveMiddlePoint, this.score);
		}
	}

	private int GetNeighbourBlocksStartIndex(List<GameObject> blocks, Vector3 curveMiddlePoint)
	{
		int num = -1;
		float num2 = float.PositiveInfinity;
		int num3 = 0;
		while (true)
		{
			GameObject gameObject = blocks[num3];
			GameObject targetBlock = blocks[(num3 + 1) % blocks.Count];
			if (this.IsPointInMiddleOfBlocks(gameObject, targetBlock, curveMiddlePoint))
			{
				break;
			}
			float num4 = Mathf.Abs(gameObject.transform.position.y - curveMiddlePoint.y);
			if (num4 < num2)
			{
				num2 = num4;
				num = num3;
			}
			num3 = (num3 + 1) % blocks.Count;
			if (num3 == 0)
			{
				goto Block_3;
			}
		}
		return num3;
		Block_3:
		return (num + blocks.Count - 1) % blocks.Count;
	}

	private bool IsPointInMiddleOfBlocks(GameObject sourceBlock, GameObject targetBlock, Vector3 point)
	{
		return (point.y <= sourceBlock.transform.position.y && point.y >= targetBlock.transform.position.y) || (point.y <= targetBlock.transform.position.y && point.y >= sourceBlock.transform.position.y);
	}

	private void OnMoveComplete(Transform tweenObject, Vector3 oldPosition, float duration)
	{
		tweenObject.DOLocalMove(oldPosition, duration / 2f, false).SetEase(Ease.InCubic);
	}

	private void OnRotateComplete(Transform tweenObject, Vector3 oldRotation, float duration)
	{
		tweenObject.DOLocalRotate(oldRotation, duration / 2f, RotateMode.Fast).SetEase(Ease.InCubic);
	}
}
