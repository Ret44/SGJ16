using UnityEngine;
using System.Collections;
using DG.Tweening;
public enum DummyAIState : int
{
    Idle = 0,
    Roam = 1,
    Follow = 2,
    Confusion = 3,
    Wow = 4,
	Dying = 5,
	Arrived = 6,

	Count,
	None
}
public class Dummy : MonoBehaviour
{
	#region Variables

	[SerializeField]
	private Transform _transform = null;
	public Transform Transform {  get { return _transform; } }
	[SerializeField]
    private Rigidbody _rigidbody = null;
	public Rigidbody Rigidbody { get { return _rigidbody; } }

	private Vector3 initialPosition;

	private DummyAIState _aiState = DummyAIState.Idle;
	public DummyAIState AiState { get { return _aiState; } }

    private bool _dead;
	public bool dead { get { return _dead; } }


	private float _stateTimer = 0.0f;
	private float _stateLength_idle = 3.0f;
	private float _stateLength_roam = 2.0f;

	private float _idleState_interval = 1.0f;
	private float _idleState_minDistance = 0.5f;
	private float _idleState_maxDistance = 2.0f;

	private float _roamState_sideFactor = 0.4f;
	private float _roamState_maxRoationSpeed = Mathf.PI;
	private float _roamState_walkSpeed = 2.0f;

	private float _followState_breakFactor = 0.2f;
	private float _followState_walkSpeed = 0.0f;
	private float _followState_initialWalkSpeed = 5.0f;
	private float _followState_stopTreshold = 0.2f;

	private float _wowState_interval = 3.0f;
	private float _wowState_minRadius = 4.0f;
	private float _wowState_maxRadius = 10.0f;
	private float _wowState_walkSpeed = 6.0f;

	private float _arrivedState_interval = 4.0f;
	private float _arrivedState_radiusMin = 2.0f;
	private float _arrivedState_radiusMax = 8.0f;
	private float _arrivedState_radius = 0.0f;
	private float _arrivedState_walkSpeed = 2.0f;

	private Vector3 _exhibitPosition = Vector3.zero;
	private Vector3 _finishPosition = Vector3.zero;

	private int _playerLayer = 0;

	[System.Serializable]
	public struct DummyStateInfo
	{
		public DummyAIState state;
		public GameObject stateGO;
	}

	[SerializeField]
	private DummyStateInfo[] _dummyStateInfos = null;
	[HideInInspector]
	[SerializeField]
	private int _dummyStateCount = 0;

	private Vector3 _targetPosition = Vector3.zero;
	private Vector3 _targetDirection = Vector3.zero;

	#endregion Variables

	#region Monobehaviour Methods

	void OnValidate()
	{
		Validate();
	}

	void Start ()
	{
        initialPosition = _transform.position;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			//if (this.speed > Player.instance.speed)
			if (true)
			{
				Player.instance.HP -= 10.0f;
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		//Debug.LogFormat("Tag: {0}", other.tag);
		if (other.tag == "Call" && !dead && AiState != DummyAIState.Wow && AiState != DummyAIState.Arrived)
		{

			_targetPosition = other.gameObject.transform.parent.position;
			_targetDirection = (_targetPosition - _transform.position).normalized;

			ChangeState(DummyAIState.Follow);
			//this.transform.LookAt(followPosition);
			//speed = runningSpeed;
		}
		if (other.tag == "Objective" && !dead)
		{
			_targetPosition = other.gameObject.transform.parent.position;
			ChangeState(DummyAIState.Wow);
			//wowStateTimer = 8f;
			//wowStateTimer = 2f;
			//dummyModel.LookAt(other.transform.position);
		}
		if(other.tag == "Exhibition")
		{
			ExhibitController exhibitionController = other.gameObject.GetComponent<ExhibitController>();
			if(exhibitionController != null)
			{
				if (exhibitionController.CurrentExhibitState != ExhibitController.ExhibitState.ES_USED)
				{
					exhibitionController.AddVisitor(this);
					_exhibitPosition = exhibitionController.transform.position;
					ChangeState(DummyAIState.Wow);
				}
			}
        }
		if(other.tag == "Finish")
		{
			_finishPosition = other.gameObject.transform.position;
			ChangeState(DummyAIState.Arrived);
		}
	}

	void Update()
	{
        if (!dead)
		{
			ProcessStates();
		}
	}


	#endregion Monobehaviour Methods

	#region Methods

	private void Validate()
	{
		_transform = this.GetComponent<Transform>();
		_rigidbody = this.GetComponent<Rigidbody>();

		DummyStateInfo[] oldDummyStateInfos = _dummyStateInfos;
		int oldDummyStateCount = oldDummyStateInfos != null ? oldDummyStateInfos.Length : 0;
		_dummyStateCount = (int)DummyAIState.Count;
		if(oldDummyStateCount != _dummyStateCount)
		{
			_dummyStateInfos = new DummyStateInfo[_dummyStateCount];
		}

		for(int i = 0;i < _dummyStateCount;++i)
		{
			if(i < oldDummyStateCount)
			{
				_dummyStateInfos[i] = oldDummyStateInfos[i];
			}
			_dummyStateInfos[i].state = (DummyAIState)i;
		}
	}

	public void ResetDummy()
    {
        this.transform.position = initialPosition;
		ChangeState(DummyAIState.Idle);
        _dead = false;
    }

    public void Die()
    {
        if(!dead)
        {
			_dead = true;
			this.transform.localScale = new Vector3(1.0f,0.1f,1.0f);
			//dummyModel.DOScaleX(0.4f, 0.1f);
			//dummyModel.DOScaleZ(0.01f, 0.1f);
			//dummyModel.DOLocalRotate(new Vector3(-90f, 0f, 0f), 0.1f);
			//dummyModel.DOLocalMoveY(-0.25f, 0.1f);
			//ParticleController.SpawnBlood(this.transform.position);

		}
	}

	public void ChangeState(DummyAIState newState)
	{
		SetModelActive(_aiState, false);
        
        _aiState = newState;
		SetModelActive(_aiState, true);

		
		_stateTimer = 0.0f;

		switch(_aiState)
		{
			case DummyAIState.Idle:
				break;
			case DummyAIState.Roam:
				_targetDirection = NewRoamDirection();
				break;
			case DummyAIState.Follow:
				_followState_walkSpeed = _followState_initialWalkSpeed;
				break;
			case DummyAIState.Confusion:
				break;
			case DummyAIState.Wow:
				{
					float randomValue = Random.value;
					float randomRange = Random.Range(_wowState_minRadius, _wowState_maxRadius);
					_targetPosition = _exhibitPosition + new Vector3(Mathf.Sin(randomValue * Mathf.PI * 2.0f) * randomRange, 0.0f, Mathf.Cos(randomValue * Mathf.PI * 2.0f) * randomRange);
				}
				break;
			case DummyAIState.Dying:
				break;
			case DummyAIState.Arrived:
				{
					float randomValue = Random.value;
					float randomRange = Random.Range(_arrivedState_radiusMin, _arrivedState_radiusMax);
					_targetPosition = _finishPosition + new Vector3(Mathf.Sin(randomValue * Mathf.PI * 2.0f) * randomRange, 0.0f, Mathf.Cos(randomValue * Mathf.PI * 2.0f) * randomRange);

					CrowdController.instance.UpdateCrowdCounter();
				}
				break;
		}
	}

	private void ProcessStates()
	{
		float deltaTime = Time.deltaTime;
		_stateTimer += deltaTime;

		switch (AiState)
		{
			case DummyAIState.Idle:
				{
					if(_stateTimer > _stateLength_idle)
					{
						ChangeState(DummyAIState.Roam);
                    }

				 }
				break;
			case DummyAIState.Roam:
				{
					Vector3 forward = _transform.forward;
					forward = Vector3.RotateTowards(forward, _targetDirection, _roamState_maxRoationSpeed * deltaTime, _roamState_maxRoationSpeed * deltaTime);
					_transform.forward = forward;

					Vector3 dummyPosition = _transform.position;

					_rigidbody.MovePosition(dummyPosition + forward * _roamState_walkSpeed * deltaTime);

					if(_stateTimer > _stateLength_roam)
					{
						_stateTimer = 0.0f;
						_targetDirection = NewRoamDirection();
                    }
				}
				break;
			case DummyAIState.Follow:
				{
					Vector3 forward = _transform.forward;
					
					forward = Vector3.RotateTowards(forward, _targetDirection, _roamState_maxRoationSpeed * deltaTime, _roamState_maxRoationSpeed * deltaTime);
					_transform.forward = forward;

					Vector3 dummyPosition = _transform.position;

					_followState_walkSpeed -= _followState_breakFactor * deltaTime;

					if (_followState_initialWalkSpeed <= _followState_stopTreshold)
					{
						ChangeState(DummyAIState.Idle);
					} else {
						_rigidbody.MovePosition(dummyPosition + forward * _followState_walkSpeed * deltaTime);
					}
				}
				break;
			case DummyAIState.Confusion:
				break;
			case DummyAIState.Wow:
				{
					//wowStateTimer -= Time.deltaTime;
					//wowStateTimer2 -= Time.deltaTime;
					//if (wowStateTimer2 < 0f)
					//{
					//
					//	GetActiveModel().transform.DORotate(new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(-25, 25), this.transform.localEulerAngles.z), 1f);
					//	wowStateTimer2 = 2f;
					//}
					//if (wowStateTimer < 0f)
					//{
					//	ChangeState(DummyAIState.Idle);
					//}

					if(_stateTimer > _wowState_interval)
					{
						_stateTimer = 0.0f;

						float randomValue = Random.value;
						float randomRange = Random.Range(_wowState_minRadius, _wowState_maxRadius);
						_targetPosition = _exhibitPosition + new Vector3(Mathf.Sin(randomValue * Mathf.PI * 2.0f) * randomRange, 0.0f, Mathf.Cos(randomValue * Mathf.PI * 2.0f) * randomRange);
					}

					Vector3 boom = _targetPosition - _transform.position;
					float distance = boom.magnitude;
					if (distance > 0.5f)
					{
						Vector3 direction = boom.normalized;
						direction.y = 0.0f;
						direction.Normalize();

						Vector3 forward = _transform.forward;
						forward = Vector3.RotateTowards(forward, direction, _roamState_maxRoationSpeed * deltaTime, _roamState_maxRoationSpeed * deltaTime);
						_transform.forward = forward;

						Vector3 dummyPosition = _transform.position;
						_rigidbody.MovePosition(dummyPosition + forward * _wowState_walkSpeed * deltaTime);
					}
				}
				break;
			case DummyAIState.Dying:
				break;
			case DummyAIState.Arrived:
				{
					if (_stateTimer > _arrivedState_interval)
					{
						_stateTimer = 0.0f;

						float randomValue = Random.value;
						float randomRange = Random.Range(_arrivedState_radiusMin, _arrivedState_radiusMax);
						_targetPosition = _finishPosition + new Vector3(Mathf.Sin(randomValue * Mathf.PI * 2.0f) * randomRange, 0.0f, Mathf.Cos(randomValue * Mathf.PI * 2.0f) * randomRange);
					}

					Vector3 boom = _targetPosition - _transform.position;
					float distance = boom.magnitude;
					if (distance > 0.5f)
					{
						Vector3 direction = boom.normalized;
						direction.y = 0.0f;
						direction.Normalize();

						Vector3 forward = _transform.forward;
						forward = Vector3.RotateTowards(forward, direction, _roamState_maxRoationSpeed * deltaTime, _roamState_maxRoationSpeed * deltaTime);
						_transform.forward = forward;

						Vector3 dummyPosition = _transform.position;
						_rigidbody.MovePosition(dummyPosition + forward * _arrivedState_walkSpeed * deltaTime);
					}
				}
				break;
		}
	}
   
	private void SetModelActive(DummyAIState state, bool active)
	{
		int index = (int)state;
		if(index >= 0 && index < _dummyStateCount)
		{
			_dummyStateInfos[index].stateGO.SetActive(active);
		}
	}

	private GameObject GetActiveModel()
	{
		return _dummyStateInfos[(int)_aiState].stateGO;
	}

	private Vector3 NewRoamDirection()
	{
		Vector3 result = _transform.forward + _transform.right * Random.Range(-_roamState_sideFactor, _roamState_sideFactor);

		result.y = 0.0f;

		result.Normalize();

		return result;
	}

	#endregion Methods
}
