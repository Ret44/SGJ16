using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdController : MonoBehaviour {

    public static CrowdController instance;
    public Dummy[] dummies;


    void Awake()
    {
        instance = this;
        dummies = this.transform.GetComponentsInChildren<Dummy>();

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
	
	// Update is called once per frame
	void Update () {
	    
	}
}
