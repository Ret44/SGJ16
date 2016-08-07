using UnityEngine;
using System.Collections;

public class ExhibitController : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private Transform _transform = null;

	[SerializeField]
	private TextMesh _countDownLabel = null;

	private float _timer = 0.0f;
	[SerializeField]
	private float _activeLength = 10.0f;

	private Dummy[] _visitors = null;
	private int _currentVisitorCount = 0;

	public enum ExhibitState
	{
		ES_IDLE = 0,
		ES_ACTIVE = 1,
		ES_USED = 2,

		ES_COUNT,
		ES_NONE
	}

	private ExhibitState _currentExhibitState = ExhibitState.ES_IDLE;
	public ExhibitState CurrentExhibitState { get { return _currentExhibitState; } }

	private float _deltaSize = 0.2f;

	[SerializeField]
	private Renderer _myRenderer = null;
	[SerializeField]
	private Material _idleMaterial = null;
	[SerializeField]
	private Material _activeMaterial = null;
	[SerializeField]
	private Material _usedMaterial = null;

	#endregion Variables

	#region Monobehaviour Methods

	void Awake()
	{
		InitializeExhibitController();
    }

	void Update()
	{
		ProcessExhibitControllerState();
    }

	#endregion Monobehaviour Methods

	#region Methods

	private void InitializeExhibitController()
	{
		_visitors = new Dummy[CrowdController.crowdSize];

		ChangeExhibitionState(ExhibitState.ES_IDLE);
    }

	public void AddVisitor(Dummy dummy)
	{
		if (_currentExhibitState != ExhibitState.ES_USED)
		{
			if (_visitors != null && _currentVisitorCount < CrowdController.crowdSize)
			{
				_visitors[_currentVisitorCount] = dummy;
				++_currentVisitorCount;

				if(_currentExhibitState == ExhibitState.ES_IDLE)
				{
					ChangeExhibitionState( ExhibitState.ES_ACTIVE );
				}
			}
		}
	}

	private void ChangeExhibitionState(ExhibitState newState)
	{
		_timer = 0.0f;
		_currentExhibitState = newState;
		switch(_currentExhibitState)
		{
			case ExhibitState.ES_IDLE:
				if (_countDownLabel != null)
				{
					_countDownLabel.text = string.Format("Come and\n check out!!!");
				}
				if (_myRenderer != null && _idleMaterial != null)
				{
					_myRenderer.material = _idleMaterial;
				}
				break;
			case ExhibitState.ES_ACTIVE:
				if (_myRenderer != null && _activeMaterial != null)
				{
					_myRenderer.material = _activeMaterial;
				}
				break;
			case ExhibitState.ES_USED:
				if (_myRenderer != null && _usedMaterial != null)
				{
					_myRenderer.material = _usedMaterial;
				}
				break;
		}
	}

	private void ProcessExhibitControllerState()
	{
		_timer += Time.deltaTime;
		switch (_currentExhibitState)
		{
			case ExhibitState.ES_IDLE:
				if(_countDownLabel != null)
				{
					_countDownLabel.transform.localScale = Vector3.one * (1.0f + Mathf.Sin(_timer) * _deltaSize);
				}
				break;
			case ExhibitState.ES_ACTIVE:

				if (_countDownLabel != null)
				{
					_countDownLabel.text = string.Format("{0}",Mathf.FloorToInt(Mathf.Max(0, _activeLength - _timer)));
				}
				if (_timer > _activeLength)
				{
					ChangeExhibitionState( ExhibitState.ES_USED );
					_timer = 0.0f;

					GameManager.Instance.ScorePoints(_currentVisitorCount);
					for(int i = 0;i < _currentVisitorCount;++i)
					{
						_visitors[i].ChangeState(DummyAIState.Idle);
					}
					_currentVisitorCount = 0;
				}

				break;
			case ExhibitState.ES_USED:
				_countDownLabel.text = "";
                break;
		}
	}

	#endregion Methods
}
