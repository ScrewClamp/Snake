using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BlockGenerator : MonoBehaviour
{
	private sealed class _RotateOver_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _startIndex___0;

		internal int _i___0;

		internal GameObject[] blocksArray;

		internal GameObject _currentBlock___1;

		internal TurnOverBlock _turnOverBlock___1;

		internal int _blockNewHp___1;

		internal Vector3 _newPostition___1;

		internal int _endIndex___0;

		internal GameObject _currentBlock___2;

		internal TurnOverBlock _turnOverBlock___2;

		internal int _blockNewHp___2;

		internal BlockGenerator _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _RotateOver_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._startIndex___0 = 0;
				this._i___0 = this._startIndex___0;
				break;
			case 1u:
				this._i___0 = (this._i___0 + 1) % this.blocksArray.Length;
				if (this._i___0 == this._startIndex___0)
				{
					this._endIndex___0 = (this._i___0 + 4) % this.blocksArray.Length;
					goto IL_150;
				}
				break;
			case 2u:
				this._i___0 = (this._i___0 + 1) % this.blocksArray.Length;
				if (this._i___0 == this._endIndex___0)
				{
					this._this.HandleBlockColorChange();
					this._PC = -1;
					return false;
				}
				goto IL_150;
			default:
				return false;
			}
			this._currentBlock___1 = this.blocksArray[this._i___0];
			this._turnOverBlock___1 = this._currentBlock___1.GetComponent<TurnOverBlock>();
			this._blockNewHp___1 = this._this.CalculateBlockHp();
			this._currentBlock___1.GetComponent<ScoreComponent>().SetScore(this._blockNewHp___1);
			BlockGenerator.ChangeRendererVisibility(this._currentBlock___1, true);
			this._currentBlock___1.GetComponentInChildren<Collider2D>().enabled = true;
			this._newPostition___1 = this._currentBlock___1.transform.position;
			this._newPostition___1.x = 0f;
			this._currentBlock___1.transform.position = this._newPostition___1;
			this._turnOverBlock___1.TurnOver(this._blockNewHp___1);
			this._current = new WaitForSeconds(0.02f);
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
			IL_150:
			this._currentBlock___2 = this.blocksArray[this._i___0];
			this._turnOverBlock___2 = this._currentBlock___2.GetComponent<TurnOverBlock>();
			this._blockNewHp___2 = this._this.CalculateBlockHp();
			this._currentBlock___2.GetComponent<ScoreComponent>().SetScore(this._blockNewHp___2);
			this._turnOverBlock___2.TurnOver(this._blockNewHp___2);
			this._current = new WaitForSeconds(0.02f);
			if (!this._disposing)
			{
				this._PC = 2;
			}
			return true;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public Action<Vector3, int> BlockCreatedEvent;

	public GameObject player;

	public GameObject block;

	public GameState gameState;

	public float maxDistanceFromCamera = 15f;

	public int initialBlockCount = 30;

	[Range(0f, 100f), SerializeField]
	private float _blockHpDifficulty;

	[Range(1f, 50f), SerializeField]
	private int _blockHpRelativeRange;

	[Range(1f, 100f), SerializeField]
	private float _noiseRangePercent;

	[Range(0f, 100f), SerializeField]
	private float _noiseProbability;

	[Range(0f, 100f), SerializeField]
	private float _noiseLowerBoundProbability;

	[Range(0f, 100f), SerializeField]
	private float _easyBlockProbability;

	[Range(0f, 1f), SerializeField]
	private float _easyBlockAndPlayerLiveCountRatio;

	[SerializeField]
	private float _neighbourBlockDistance;

	[SerializeField]
	private Color _blockColorGreen;

	[SerializeField]
	private Color _blockColorYellow;

	[SerializeField]
	private Color _blockColorRed;

	[SerializeField]
	private Material _blockMaterialGreen;

	[SerializeField]
	private Material _blockMaterialYellow;

	[SerializeField]
	private Material _blockMaterialRed;

	private Transform _cameraHolder;

	private ScoreComponent _playerLiveComponent;

	private PlayerLiveCalculator _playerLiveCalculator;

	private Vector3 _lastPosition = Vector3.zero;

	private int _previousBrickHp;

	private int _previousLiveCount;

	private StringWave _stringWave;

	private ColorChanger[] _blockColorChnagerArray;

	private GameObject[] _blockArray;

	private ScoreComponent[] _scoreComponents;

	private FloatingWaveBlock[] _floatingWaveBlockArray;

	private readonly Queue<GameObject> _blocks = new Queue<GameObject>();

	private bool _requestBlockColorChange;

	public float BlockHpDifficulty
	{
		get
		{
			return this._blockHpDifficulty;
		}
		set
		{
			this._blockHpDifficulty = value;
		}
	}

	public int BlockHpRelativeRange
	{
		get
		{
			return this._blockHpRelativeRange;
		}
		set
		{
			this._blockHpRelativeRange = value;
		}
	}

	public float NoiseRangePercent
	{
		get
		{
			return this._noiseRangePercent;
		}
		set
		{
			this._noiseRangePercent = value;
		}
	}

	public float NoiseProbability
	{
		get
		{
			return this._noiseProbability;
		}
		set
		{
			this._noiseProbability = value;
		}
	}

	public float NoiseLowerBoundProbability
	{
		get
		{
			return this._noiseLowerBoundProbability;
		}
		set
		{
			this._noiseLowerBoundProbability = value;
		}
	}

	public float EasyBlockProbability
	{
		get
		{
			return this._easyBlockProbability;
		}
		set
		{
			this._easyBlockProbability = value;
		}
	}

	public float EasyBlockAndPlayerLiveCountRatio
	{
		get
		{
			return this._easyBlockAndPlayerLiveCountRatio;
		}
		set
		{
			this._easyBlockAndPlayerLiveCountRatio = value;
		}
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
		this._cameraHolder = Camera.main.transform.parent;
		this._playerLiveComponent = this.player.GetComponent<ScoreComponent>();
		this._playerLiveCalculator = this.player.GetComponent<PlayerLiveCalculator>();
		PlayerLiveCalculator expr_44 = this._playerLiveCalculator;
		expr_44.PlayerLiveUpdatedByBlockEvent = (Action<int>)Delegate.Combine(expr_44.PlayerLiveUpdatedByBlockEvent, new Action<int>(this.OnPlayerLiveUpdatedByBlock));
		this._playerLiveCalculator.onPlayerLiveCountChanged.AddListener(new UnityAction<int>(this.OnPlayerLiveCountChanged));
		this.gameState.OnGameOverEvent.AddListener(new UnityAction(this.OnGameOver));
		this._stringWave = base.GetComponent<StringWave>();
		this._requestBlockColorChange = true;
		this.CreateBlocks();
		this.InitBlockFloatingWave();
		this.HandleBlockColorChange();
	}

	private void OnPlayerLiveCountChanged(int liveCount)
	{
		this._requestBlockColorChange = true;
	}

	private void InitBlockFloatingWave()
	{
		for (int i = 1; i < this._floatingWaveBlockArray.Length; i++)
		{
			this.InitBlockFloatingByIndex(i);
		}
	}

	private void InitBlockFloatingByIndex(int currentBlockIndex)
	{
		FloatingWaveBlock floatingWaveBlock = this._floatingWaveBlockArray[currentBlockIndex];
		int num = (currentBlockIndex + this._floatingWaveBlockArray.Length - 1) % this._floatingWaveBlockArray.Length;
		FloatingWaveBlock floatingWaveBlock2 = this._floatingWaveBlockArray[num];
		Vector4 nextRotation = floatingWaveBlock2.GetComponent<FloatingWaveBlock>().GetNextRotation();
		floatingWaveBlock.GetComponent<FloatingWaveBlock>().SetRotation(nextRotation);
	}

	private void OnGameOver()
	{
		this._previousLiveCount = 0;
		this._requestBlockColorChange = true;
		this._playerLiveComponent.SetScore(0);
		this.UpdateBlocks();
	}

	private void OnPlayerLiveUpdatedByBlock(int liveCount)
	{
		this._previousLiveCount = this._playerLiveComponent.GetScore();
	}

	public int CalculateBlockHp()
	{
		int playerLiveCount = this.GetPlayerLiveCount();
		int num = Mathf.Max(1, (int)((float)playerLiveCount + (float)this._blockHpRelativeRange * this._blockHpDifficulty / 100f));
		int num2 = Mathf.Max(1, num - this._blockHpRelativeRange);
		int result;
		if (this.ShouldAddEasyBlocks())
		{
			result = this.GenerateEasyBlocks(playerLiveCount);
		}
		else if (this.ShouldAddNoise())
		{
			result = this.GenerateNoise(num2, num);
		}
		else
		{
			result = UnityEngine.Random.Range(num2, num);
		}
		this.PreventBlockHpDupplication(ref result);
		return result;
	}

	private int GetPlayerLiveCount()
	{
		return (this._previousLiveCount != 0) ? ((this._previousLiveCount + this._playerLiveComponent.GetScore()) / 2) : this._playerLiveComponent.GetScore();
	}

	private bool ShouldAddEasyBlocks()
	{
		return UnityEngine.Random.value <= this._easyBlockProbability / 100f;
	}

	private int GenerateEasyBlocks(int playerLiveCount)
	{
		return Mathf.Max(1, (int)((float)playerLiveCount * this._easyBlockAndPlayerLiveCountRatio));
	}

	private void PreventBlockHpDupplication(ref int blockHp)
	{
		if (blockHp == this._previousBrickHp)
		{
			if (blockHp == 1)
			{
				blockHp++;
			}
			else
			{
				blockHp--;
			}
		}
		this._previousBrickHp = blockHp;
	}

	private bool ShouldAddNoise()
	{
		float num = UnityEngine.Random.Range(0f, 100f);
		return num < this._noiseProbability;
	}

	private int GenerateNoise(int minHp, int maxHp)
	{
		bool flag = UnityEngine.Random.value <= this._noiseLowerBoundProbability / 100f;
		int max = (int)(this._noiseRangePercent * (float)this._blockHpRelativeRange / 100f);
		int result;
		if (flag)
		{
			result = Mathf.Max(1, minHp - UnityEngine.Random.Range(0, max));
		}
		else
		{
			result = maxHp + UnityEngine.Random.Range(0, max);
		}
		return result;
	}

	private void CreateBlocks()
	{
		this._scoreComponents = new ScoreComponent[this.initialBlockCount];
		this._blockArray = new GameObject[this.initialBlockCount];
		this._blockColorChnagerArray = new ColorChanger[this.initialBlockCount];
		this._floatingWaveBlockArray = new FloatingWaveBlock[this.initialBlockCount];
		for (int i = 0; i < this.initialBlockCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.block);
			this._blockArray[i] = gameObject;
		}
		for (int j = 0; j < this.initialBlockCount; j++)
		{
			GameObject gameObject2 = this._blockArray[j];
			Transform transform = gameObject2.transform;
			this._lastPosition += new Vector3(0f, this._neighbourBlockDistance, 0f);
			transform.position = this._lastPosition;
			transform.rotation = Quaternion.identity;
			gameObject2.name = "block_" + j;
			int num = this.CalculateBlockHp();
			ScoreComponent component = gameObject2.GetComponent<ScoreComponent>();
			component.SetScore(num);
			this._scoreComponents[j] = component;
			HitBlock expr_106 = gameObject2.GetComponent<HitBlock>();
			expr_106.onBlockHit = (Action<float, GameObject, bool>)Delegate.Combine(expr_106.onBlockHit, new Action<float, GameObject, bool>(this._stringWave.OnBlockHit));
			this._blocks.Enqueue(gameObject2);
			ColorChanger component2 = gameObject2.GetComponent<ColorChanger>();
			this._blockColorChnagerArray[j] = component2;
			FloatingWaveBlock component3 = transform.GetChild(0).GetComponent<FloatingWaveBlock>();
			component3.blockIndex = j;
			this._floatingWaveBlockArray[j] = component3;
			this.DispatchBlockCreatedEvent(transform.position, num);
		}
	}

	public List<GameObject> GetBlocks()
	{
		return new List<GameObject>(this._blocks.ToArray());
	}

	private void Update()
	{
		GameObject firstBlock = this._blocks.Peek();
		if (this.IsOutsideCameraView(firstBlock))
		{
			this.MoveUp(firstBlock);
		}
		this.PaintBlocks();
	}

	private void PaintBlocks()
	{
		bool flag = false;
		if (flag)
		{
			this.PaintAllBlocksByColor(this._blockColorGreen);
		}
		else if (this._requestBlockColorChange)
		{
			this._requestBlockColorChange = false;
			this.HandleBlockColorChange();
		}
	}

	private void HandleBlockColorChange()
	{
		int score = this._playerLiveComponent.GetScore();
		ColorChanger[] blockColorChnagerArray = this._blockColorChnagerArray;
		for (int i = 0; i < blockColorChnagerArray.Length; i++)
		{
			ColorChanger colorChanger = blockColorChnagerArray[i];
			this.ChangeBlockColor(colorChanger, score);
		}
	}

	private void PaintAllBlocksByColor(Color color)
	{
		ColorChanger[] blockColorChnagerArray = this._blockColorChnagerArray;
		for (int i = 0; i < blockColorChnagerArray.Length; i++)
		{
			ColorChanger colorChanger = blockColorChnagerArray[i];
			Material blockMaterial = this.GetBlockMaterial(color);
			if (blockMaterial != colorChanger.Material)
			{
				colorChanger.Material = blockMaterial;
			}
		}
	}

	private void ChangeBlockColor(ColorChanger colorChanger, int playerLiveCount)
	{
		int score = colorChanger.GetComponent<ScoreComponent>().GetScore();
		float difficulty = (playerLiveCount != 1) ? Mathf.InverseLerp(1f, (float)playerLiveCount, (float)score) : 1f;
		Material blockMaterial = this.GetBlockMaterial(difficulty);
		if (blockMaterial != colorChanger.Material)
		{
			colorChanger.Material = blockMaterial;
		}
	}

	private Material GetBlockMaterial(Color color)
	{
		if (color == this._blockColorGreen)
		{
			return this._blockMaterialGreen;
		}
		if (color == this._blockColorYellow)
		{
			return this._blockMaterialYellow;
		}
		return this._blockMaterialRed;
	}

	private Material GetBlockMaterial(float difficulty)
	{
		if (difficulty < 0.5f)
		{
			return this._blockMaterialGreen;
		}
		if (difficulty >= 0.5f && difficulty < 1f)
		{
			return this._blockMaterialYellow;
		}
		return this._blockMaterialRed;
	}

	private Color GetBlockColor(float difficulty)
	{
		if (difficulty < 0.5f)
		{
			return this._blockColorGreen;
		}
		if (difficulty >= 0.5f && difficulty < 1f)
		{
			return this._blockColorYellow;
		}
		return this._blockColorRed;
	}

	private bool IsOutsideCameraView(GameObject firstBlock)
	{
		float num = firstBlock.transform.position.y - this._cameraHolder.position.y;
		return num < -this.maxDistanceFromCamera;
	}

	private void MoveUp(GameObject firstBlock)
	{
		int num = this.CalculateBlockHp();
		this._blocks.Dequeue();
		firstBlock.transform.position = this._lastPosition + new Vector3(0f, this._neighbourBlockDistance, 0f);
		BlockGenerator.ChangeRendererVisibility(firstBlock, true);
		firstBlock.GetComponentInChildren<Collider2D>().enabled = true;
		if (firstBlock.GetComponent<ScoreComponent>().GetScore() != num)
		{
			firstBlock.GetComponent<ScoreComponent>().SetScore(num);
			this.ChangeBlockColor(firstBlock.GetComponent<ColorChanger>(), this._playerLiveComponent.GetScore());
		}
		this._lastPosition = firstBlock.transform.position;
		this._blocks.Enqueue(firstBlock);
		FloatingWaveBlock component = firstBlock.transform.GetChild(0).GetComponent<FloatingWaveBlock>();
		this.InitBlockFloatingByIndex(component.blockIndex);
		this.DispatchBlockCreatedEvent(firstBlock.transform.position, num);
	}

	private static void ChangeRendererVisibility(GameObject firstBlock, bool isVisible)
	{
		Renderer[] componentsInChildren = firstBlock.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
			renderer.enabled = isVisible;
		}
	}

	private void DispatchBlockCreatedEvent(Vector3 blockPosition, int blockHP)
	{
		if (this.BlockCreatedEvent != null)
		{
			this.BlockCreatedEvent(blockPosition, blockHP);
		}
	}

	private void UpdateBlocks()
	{
		GameObject[] blocksArray = this._blocks.ToArray();
		base.StartCoroutine(this.RotateOver(blocksArray));
	}

	public IEnumerator RotateOver(GameObject[] blocksArray)
	{
		BlockGenerator._RotateOver_c__Iterator0 _RotateOver_c__Iterator = new BlockGenerator._RotateOver_c__Iterator0();
		_RotateOver_c__Iterator.blocksArray = blocksArray;
		_RotateOver_c__Iterator._this = this;
		return _RotateOver_c__Iterator;
	}
}
