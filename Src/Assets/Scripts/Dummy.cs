using UnityEngine;
using System.Collections;

public enum DummyAIState
{
    Idle,
    Follow
}
public class Dummy : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 followPosition;
    public Vector3 velocity;
    public float runningSpeed;
    public float idleSpeed;
    public DummyAIState AiState;

    public Transform dummyModel;

	// Use this for initialization
	void Start () {
        startPosition = this.transform.position;
	}

    public void reset()
    {
        this.transform.position = startPosition;
        AiState = DummyAIState.Idle;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Call")
        {
            AiState = DummyAIState.Follow;
            followPosition = other.gameObject.transform.parent.position;
        }
    }

    	
	// Update is called once per frame
    void Update()
    {
        if (AiState == DummyAIState.Idle)
        {
            Vector3 dir = Random.insideUnitSphere * 2;
            velocity = new Vector3(Mathf.Lerp(velocity.x, dir.x, 0.1f), 0f, Mathf.Lerp(velocity.z, dir.z, 0.1f));
            this.transform.Translate(velocity * idleSpeed * Time.deltaTime);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.01f);
        }
        if(AiState == DummyAIState.Follow)
        {
            this.transform.LookAt(followPosition);
            this.transform.Translate(transform.forward * runningSpeed * Time.deltaTime);
           // this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.01f);
        }
        dummyModel.localPosition = Vector3.zero;
    }
}
