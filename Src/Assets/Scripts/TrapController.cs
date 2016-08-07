using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour {
    public GameObject fireParticles;


    public void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Dummies"))
        {
            Dummy tmpD = other.GetComponent<Dummy>();
            if (tmpD.AiState != DummyAIState.OnFire)
                tmpD.ChangeState(DummyAIState.OnFire);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
