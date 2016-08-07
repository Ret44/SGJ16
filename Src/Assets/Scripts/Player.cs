using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum PlayerState : int
{
    Roam = 0,
    Call = 1,
    Dead = 2,

    Count,
    None
}

public class Player : MonoBehaviour
{
	#region Variables

	public static Player instance;
    public PlayerState state;
 //   public GameObject playerModel;
    public Transform sphereTransform;

    private bool _dead;
    public bool dead { get { return _dead; } }

    public float range;
    private float _hp;
	public float HP
	{
		get { return _hp; }
		set { _hp = value; }
	}
	private float _maxHP = 30.0f;
	private float _recoveryRate = 30.0f;
    public float speed;
    public Vector3 input;

    public Vector3 targetScale;
    public GameObject objective;
    public SphereCollider sphereCollider;

    [SerializeField]
    private Vector3 _cameraOffset;

	[SerializeField]
	private AudioClip[] _sounds_comeWithMe = null;

    [SerializeField]
    private float _stateTimer;

	private float _maxRotationRate = Mathf.PI * 1.5f;

	[SerializeField]
	private Transform _transform = null;
    [SerializeField]
    private Rigidbody _rigidbody;

    [System.Serializable]
    public struct PlayerStateInfo
    {
        public PlayerState state;
        public GameObject stateGO;
    }

    [SerializeField]
    private PlayerStateInfo[] _stateInfos = null;
    [HideInInspector]
    [SerializeField]
    private int _stateCount = 0;

	//[SerializeField]
	//private Transform _callObject;

	#endregion Variables

	#region Monobehaviour Methods

	public void Awake()
    {
        instance = this;
     //   targetScale = _callObject.transform.localScale;
        state = PlayerState.Roam;
        _cameraOffset = this.transform.position - Camera.main.transform.position;
        _rigidbody = this.GetComponent<Rigidbody>();
    }


	public void Update()
	{
		if (GameManager.Instance.CurrentGameState == GameManager.GameState.GS_GAME)
		{
			ProcessStates();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Finish")
		{
			BoxCollider tmpCollider = other.gameObject.GetComponent<BoxCollider>(); 
			if(tmpCollider != null)
			{
				
			}
			GameManager.Instance.TryFinish();
		}
	}

	#endregion Monbehaviour Methods

	#region Methods

	public void CallEnd()
    {
        sphereTransform.localScale = new Vector3(0f, 1f, 0f);
		sphereCollider.enabled = false;
    }

    public void CallFinished()
    {
        ChangeState(PlayerState.Roam);
    }

    public void ModelTweenEnd()
    {
        _stateInfos[(int)state].stateGO.transform.DOScale(targetScale, 0.25f).OnComplete(CallFinished);  
    }

    public void SendCall()
    {
        if (!dead)
        {
			targetScale = Vector3.one;

            sphereTransform.DOScaleX(range * 2.5f, 0.5f);
            sphereTransform.DOScaleZ(range * 2.5f, 0.5f).OnComplete(CallEnd);

			sphereCollider.radius = range;
            sphereCollider.enabled = true;
            _stateInfos[(int)state].stateGO.transform.DOScale(0.5f, 0.25f).SetEase(Ease.InBounce).OnComplete(ModelTweenEnd);

			Audio.Instance.PlaySound(_sounds_comeWithMe);
        }
    }
    private GameObject GetActiveModel()
    {
        return _stateInfos[(int)state].stateGO;
    }
    private void SetModelActive(PlayerState state, bool active)
    {
        int index = (int)state;
        if (index >= 0 && index < _stateCount)
        {
            _stateInfos[index].stateGO.SetActive(active);
        }
    }
    private void ChangeState(PlayerState newState)
    {
        SetModelActive(state, false);

        state = newState;
        SetModelActive(state, true);
   
        _stateTimer = 0.0f;

        switch (state)
        {
            case PlayerState.Roam:
                break;
            case PlayerState.Call:
                SendCall();
                break;
            case PlayerState.Dead:             
                break;
        }
    }

    public void Die()
    {
        if (!dead)
        {
            _dead = true;
            _stateInfos[(int)state].stateGO.transform.DOScaleX(0.4f, 0.1f);
            _stateInfos[(int)state].stateGO.transform.DOScaleZ(0.01f, 0.1f);
            _stateInfos[(int)state].stateGO.transform.DOLocalRotate(new Vector3(-90f, 0f, 0f), 0.1f);
            _stateInfos[(int)state].stateGO.transform.DOLocalMoveY(-0.25f, 0.1f);
            ParticleController.SpawnBlood(this.transform.position);
        }
    }

    void ProcessStates()
    {
		float deltaTime = Time.deltaTime;

        switch(state)
        {
            case PlayerState.Roam:
            case PlayerState.Call:
                Vector3 translateVector = input * speed * Time.deltaTime;
                
               //_rigidbody.MovePosition(translateVector);
				//
				Vector3 position = this.transform.position;
				position += input * speed * deltaTime;
				this.transform.position = position;

				Vector3 forward = _transform.forward;
				forward = Vector3.RotateTowards(forward, input, _maxRotationRate * deltaTime, 0.0f);
				_transform.forward = forward;


				//    if (translateVector != Vector3.zero)
				//    {
				//        Debug.Log(translateVector.ToString());
				//        this.transform.Translate(translateVector);
				//        SetCamera();
				//        //this.transform.rotation = Quaternion.LookRotation(velocity);
				//        Debug.DrawRay(this.transform.position, velocity, Color.red);
				//    }


				// this.transform.localPosition = Vector3.zero;
				if (_hp < 0.0f)
				{
					Die();
				}

				if (_hp < _maxHP)
				{
					_hp += _recoveryRate * deltaTime;
				}
                break;
            case PlayerState.Dead:
                break;
        }
    }

	public void ResetPlayer()
	{
		_hp = _maxHP;
		_dead = false;
	}

	#endregion Methods
}
