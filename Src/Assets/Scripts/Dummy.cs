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
	private float _roamState_walkSpeed = 1.0f;

	private float _followState_breakFactor = 0.2f;
	private float _followState_walkSpeed = 0.0f;
	private float _followState_initialWalkSpeed = 3.0f;
	private float _followState_stopTreshold = 0.2f;

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
				Player.instance.HP -= 10;
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Call" && !dead)
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

	private void ChangeState(DummyAIState newState)
	{
		SetModelActive(_aiState, false);
		SetModelActive(_aiState, true);

		_aiState = newState;
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
				break;
			case DummyAIState.Dying:
				break;
		}
	}

	private void ProcessStates()
	{
		float deltaTime = Time.deltaTime;

		switch (AiState)
		{
			case DummyAIState.Idle:
				{
					if(_stateTimer > _stateLength_idle)
					{
						ChangeState(DummyAIState.Roam);
                    }

					//speed = idleSpeed;
					//Vector3 dir = Random.insideUnitSphere * 2;
					//velocity = new Vector3(Mathf.Lerp(velocity.x, dir.x, 0.1f), 0f, Mathf.Lerp(velocity.z, dir.z, 0.1f));
					//this.transform.Translate(velocity * speed * Time.deltaTime);
					//this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.01f);
				}
				break;
			case DummyAIState.Roam:
				{
					Vector3 forward = _transform.forward;
					forward = Vector3.MoveTowards(forward, _targetDirection, _roamState_maxRoationSpeed * deltaTime);
					_transform.forward = forward;

					Vector3 dummyPosition = _transform.position;

					_rigidbody.MovePosition(dummyPosition + forward * _roamState_walkSpeed * deltaTime);

					_stateTimer = 0.0f;
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
				}
				break;
			case DummyAIState.Dying:
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
