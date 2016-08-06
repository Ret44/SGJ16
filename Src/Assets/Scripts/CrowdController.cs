using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdController : MonoBehaviour {

    public static CrowdController instance;
    public Dummy[] dummies;
	private int _dummyCount = 0;

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
	private float _soundIntervalMin = 0.3f;
	private float _soundIntervalMax = 0.6f;

	void Awake()
    {
        instance = this;
        dummies = this.transform.GetComponentsInChildren<Dummy>();

		_dummyCount = dummies.Length;
    }

    public static void ResetAll()
    {
        for(int i =0 ; i< instance.dummies.Length; i++)
        {
            instance.dummies[i].reset();
        }
    }
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	void OnValidate()
	{
		Validate();
	}

	// Update is called once per frame
	void Update () {
		ProcessCrowdSounds();
    }

	#region Methods

	public void Validate()
	{
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

		for(int i = 0;i <_dummyCount;++i)
		{
			int dummyStateIndex = (int)dummies[i].AiState;
			++_dummyAiStateInfos[dummyStateIndex].stateCount;
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

		if(currentMaxStateIndex != -1)
		{
			DummyAIState choosenState = (DummyAIState)currentMaxStateIndex;
			switch(choosenState)
			{
				case DummyAIState.Idle:
					break;
				case DummyAIState.Heard:
					break;
				case DummyAIState.Follow:
					break;
				case DummyAIState.Confusion:
					break;
				case DummyAIState.Wow:
					break;
			}

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

	#endregion Methpds
}
