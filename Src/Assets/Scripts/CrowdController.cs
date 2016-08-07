using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdController : MonoBehaviour
{
	#region Variables

	public static CrowdController instance;
    public Dummy[] dummies;

	private Dummy[] _dummies = null;
	private int _dummyCount = 0;

	[SerializeField]
	private Dummy _prefab = null;

	[SerializeField]
	private Transform _dummyParent = null;

	[System.Serializable]
	public struct DummyAiStateInfo
	{
		public DummyAIState state;
		public AudioClip[] stateSounds;
		[System.NonSerialized]
		public int stateCount;
	}

	[SerializeField]
	private DummyAiStateInfo[] _dummyAiStateInfos = null;
	[SerializeField]
	private int _dummyAiStateInfoCount = 0;

	private DummyAIState _lastChoosenDummyAIState = DummyAIState.None;
	private float _soundTimer = 0.0f;
	private float _soundTimeInterval = 0.0f;
	private float _soundIntervalMin = 0.6f;
	private float _soundIntervalMax = 1.1f;

	//[SerializeField]
	//private Vector3 _spawnPosition = Vector3.zero;
	//[SerializeField]
	//[Range(0.0f,1.0f)]
	//private float _spawnDirection = 0.0f;
	[SerializeField]
	private int _spawnRowsSize = 1;
	private float _spaceDistance = 2.0f;

	public const int crowdSize = 90;

	private bool _wasInited = false;

	private ComponentBasedObjectPool<Dummy> _dummyPool= null;

	[SerializeField]
	private TextMesh _couterTextMesh = null;

	#endregion Variables

	#region Monobehaviour Methods

	void OnValidate()
	{
		Validate();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		for(int i = 0;i < crowdSize;++i)
		{
			Vector3 tmpPosition = GetSpawnPositionAtIndex(i);
			Gizmos.DrawWireSphere(tmpPosition, 0.5f);
        }
	}

	void Awake()
    {
		InitCrowdController();
    }

	void OnEnable()
	{

	}

	void Update ()
	{
		ProcessCrowdSounds();
    }

	#endregion Monobehaviour Methods

	#region Methods

	private void InitCrowdController()
	{
		if (!_wasInited)
		{
			instance = this;
			dummies = this.transform.GetComponentsInChildren<Dummy>();

			_dummyPool = new ComponentBasedObjectPool<Dummy>(_prefab, _dummyParent, crowdSize);

			_dummyCount = dummies.Length;

			_wasInited = true;
		}
	}

	public static void ResetAll()
	{
		for (int i = 0; i < instance.dummies.Length; i++)
		{
			instance.dummies[i].ResetDummy();
		}
	}

	public void ResetCrowdController()
	{
		InitCrowdController();

		if (_dummies != null)
		{
			for(int i = 0;i < crowdSize;++i)
			{
				_dummies[i].gameObject.SetActive(false);
				_dummies[i] = null;
            }
		} else {
			_dummies = new Dummy[crowdSize];
		}

		for(int i = 0;i < crowdSize;++i)
		{
			_dummies[i] = _dummyPool.GetPooledObject();
			_dummies[i].Transform.position = GetSpawnPositionAtIndex(i);
			_dummies[i].gameObject.SetActive(true);
		}
	}

	public void Validate()
	{
		_spawnRowsSize = Mathf.Max(1, _spawnRowsSize);

		DummyAiStateInfo[] oldDummyAiStateInfos = _dummyAiStateInfos;
		int oldDummyAiStateInfoCount = oldDummyAiStateInfos != null ? oldDummyAiStateInfos.Length : 0;
		_dummyAiStateInfoCount = (int)DummyAIState.Count;

		if (oldDummyAiStateInfoCount != _dummyAiStateInfoCount)
		{
			_dummyAiStateInfos = new DummyAiStateInfo[_dummyAiStateInfoCount];
		}
		for (int i = 0; i < _dummyAiStateInfoCount; ++i)
		{
			if (i < oldDummyAiStateInfoCount)
			{
				_dummyAiStateInfos[i] = oldDummyAiStateInfos[i];
			}
			_dummyAiStateInfos[i].state = (DummyAIState)i;
		}
	}

	private void ProcessCrowdSounds()
	{
		for(int i = 0;i < _dummyAiStateInfoCount;++i)
		{
			_dummyAiStateInfos[i].stateCount = 0;
		}

		for(int i = 0;i < crowdSize;++i)
		{
			if (_dummies[i] != null && !_dummies[i].dead)
			{
				int dummyStateIndex = (int)_dummies[i].AiState;

				++_dummyAiStateInfos[dummyStateIndex].stateCount;
			}
		}

		int currentMaxStateIndex = -1;
		int currentMax = 0;
		for(int i = 0;i < _dummyAiStateInfoCount;++i)
		{
			if(currentMax < _dummyAiStateInfos[i].stateCount)
			{
				currentMaxStateIndex = i;
				currentMax = _dummyAiStateInfos[i].stateCount;
            }
		}

		float bennyFactor = Mathf.Clamp01( _dummyAiStateInfos[(int)DummyAIState.Follow].stateCount / (float)_dummyCount );
		Audio.Instance.SetMusicVolume(bennyFactor);

		if(currentMaxStateIndex != -1)
		{
			DummyAIState choosenState = (DummyAIState)currentMaxStateIndex;


			if(_lastChoosenDummyAIState == choosenState)
			{
				_soundTimer += Time.deltaTime;
				if(_soundTimer > _soundTimeInterval)
				{
					_soundTimer = 0.0f;
					_soundTimeInterval = Random.Range(_soundIntervalMin, _soundIntervalMax);

					Audio.Instance.PlaySound(_dummyAiStateInfos[currentMaxStateIndex].stateSounds);
				}
			} else {
				_soundTimer = 0.0f;
				_soundTimeInterval = Random.Range(_soundIntervalMin, _soundIntervalMax);
			}

			_lastChoosenDummyAIState = choosenState;
		}
	}

	private Vector3 GetSpawnPositionAtIndex(int index)
	{
		Vector3 result = transform.position;

		int columnIndex = Mathf.FloorToInt(index / _spawnRowsSize);
		int rowIndex = index % _spawnRowsSize;

		//Vector3 forward = new Vector3(Mathf.Cos(_spawnDirection * Mathf.PI * 2.0f),0.0f, Mathf.Sin(_spawnDirection * Mathf.PI * 2.0f));
		//Vector3 right = new Vector3(Mathf.Cos((_spawnDirection + 0.25f) * Mathf.PI * 2.0f), 0.0f, Mathf.Sin( (_spawnDirection + 0.25f) * Mathf.PI * 2.0f));

		result += transform.forward * _spaceDistance * rowIndex + transform.right * _spaceDistance * columnIndex;

		return result;
	}

	public int GetArrivedCount()
	{
		int result = 0;
		for(int i = 0;i < crowdSize;++i)
		{
			if(_dummies[i].AiState == DummyAIState.Arrived)
			{
				++result;
			}
		}
		return result;
	}

	public void UpdateCrowdCounter()
	{
		if(_couterTextMesh != null)
		{
			int couter = GetArrivedCount();

			_couterTextMesh.text = string.Format("{0}", couter);
		}
	}

	#endregion Methpds
}
