using UnityEngine;
using System.Collections;
using DG.Tweening;
public enum DummyAIState : int
{
    Idle = 0,
    Heard,
    Follow,
    Confusion,
    Wow,

	Count,
	None
}
public class Dummy : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 followPosition;
    public Vector3 velocity;
    public float runningSpeed;
    public float idleSpeed;
    public float speed;
    public DummyAIState AiState;
    public bool dead;
    private Rigidbody rBody;
    private Vector3 targetScale;
    public Transform dummyModel;

    public float wowStateTimer;
    public float wowStateTimer2;

       

	// Use this for initialization
	void Start () {
        startPosition = this.transform.position;
        rBody = GetComponent<Rigidbody>();
        targetScale = dummyModel.transform.localScale;
	}

    public void reset()
    {
        this.transform.position = startPosition;
        AiState = DummyAIState.Idle;
        dead = false;
    }

    public void Die()
    {
        if(!dead)
        {
            dead = true;
            dummyModel.DOScaleX(0.4f, 0.1f);
            dummyModel.DOScaleZ(0.01f, 0.1f);
            dummyModel.DOLocalRotate(new Vector3(-90f, 0f, 0f), 0.1f);
            dummyModel.DOLocalMoveY(-0.25f, 0.1f);
//            ParticleController.SpawnBlood(this.transform.position);

        }
    }

    public void FollowPoint()
    {
      //  dummyModel.transform.DOScale(targetScale, 0.15f).SetEase(Ease.InBounce).OnComplete(FollowPoint);
       
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           if(this.speed > Player.instance.speed)
           {
               Player.instance.HP -= 10;
           }
        }
    }    
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Call" && !dead)
        {
        //    RaycastHit hit;
          //  if(Physics.Raycast(this.transform.position, other.transform.position, out hit))
        //    {
        //        if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        // //       {
                    AiState = DummyAIState.Follow;
                    followPosition = other.gameObject.transform.parent.position;
                    this.transform.LookAt(followPosition);
                    speed = runningSpeed;
          //      }
         //   }

           
          //  dummyModel.transform.DOScale(new Vector3(0.5f, 1.2f, 0.5f), 0.15f).SetEase(Ease.InBounce).OnComplete(FollowPoint);
        }
        if(other.tag == "Objective" && !dead)
        {
            AiState = DummyAIState.Wow;
            wowStateTimer = 8f;
            wowStateTimer = 2f;
            dummyModel.LookAt(other.transform.position);
        }
    }

    	
	// Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            //  GetComponent<Rigidbody>().angularDrag = 0;
            if (AiState == DummyAIState.Idle)
            {
                speed = idleSpeed;
                Vector3 dir = Random.insideUnitSphere * 2;
                velocity = new Vector3(Mathf.Lerp(velocity.x, dir.x, 0.1f), 0f, Mathf.Lerp(velocity.z, dir.z, 0.1f));
                this.transform.Translate(velocity * speed * Time.deltaTime);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.01f);
            }
            if (AiState == DummyAIState.Follow)
            {
                rBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
                rBody.velocity = Vector3.zero;
                //  this.transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
                // this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.01f);
                speed -= 0.01f;
                if (speed <= idleSpeed)
                    AiState = DummyAIState.Idle;
            }
            if (AiState == DummyAIState.Wow)
            {
                wowStateTimer -= Time.deltaTime;
                wowStateTimer2 -= Time.deltaTime;
                if (wowStateTimer2 < 0f)
                {
                    dummyModel.DORotate(new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + Random.Range(-25, 25), this.transform.localEulerAngles.z), 1f);
                    wowStateTimer2 = 2f;
                }
                if (wowStateTimer < 0f)
                {
                    AiState = DummyAIState.Idle;
                }
            }
            dummyModel.localPosition = Vector3.zero;
            
        }
    }
}
