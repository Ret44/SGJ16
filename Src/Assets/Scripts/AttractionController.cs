using UnityEngine;
using System.Collections;

public class AttractionController : MonoBehaviour {

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            ParticleController.SpawnBreaking(this.transform.position);
            Destroy(this.gameObject);
        }
    }

   	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
