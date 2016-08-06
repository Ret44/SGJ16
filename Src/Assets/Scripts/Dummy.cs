﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
public enum DummyAIState
{
    Idle,
    Heard,
    Follow,
    Confusion,
}
public class Dummy : MonoBehaviour {

    public Vector3 startPosition;
    public Vector3 followPosition;
    public Vector3 velocity;
    public float runningSpeed;
    public float idleSpeed;
    public float speed;
    public DummyAIState AiState;
    private Rigidbody rBody;
    private Vector3 targetScale;
    public Transform dummyModel;

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
    }

    public void FollowPoint()
    {
      //  dummyModel.transform.DOScale(targetScale, 0.15f).SetEase(Ease.InBounce).OnComplete(FollowPoint);
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Call")
        {
            AiState = DummyAIState.Follow;
            followPosition = other.gameObject.transform.parent.position;
            this.transform.LookAt(followPosition);
            speed = runningSpeed;
          //  dummyModel.transform.DOScale(new Vector3(0.5f, 1.2f, 0.5f), 0.15f).SetEase(Ease.InBounce).OnComplete(FollowPoint);
        }
    }

    	
	// Update is called once per frame
    void Update()
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
        if(AiState == DummyAIState.Follow)
        {
            rBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            rBody.velocity = Vector3.zero;
          //  this.transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            // this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(velocity), 0.01f);
            speed -= 0.01f;
            if (speed <= idleSpeed)
                AiState = DummyAIState.Idle;
        }
        dummyModel.localPosition = Vector3.zero;
    }
}
