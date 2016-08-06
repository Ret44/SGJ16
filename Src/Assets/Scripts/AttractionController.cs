using UnityEngine;
using System.Collections;

public class AttractionController : MonoBehaviour {

    public bool Deadly;
    public Rigidbody rBody;


    public void OnCollisionEnter(Collision collision)
    {
        if (!Deadly)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                ParticleController.SpawnBreaking(this.transform.position);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (rBody.velocity.magnitude > 3)
            {

            }
        }
    }

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

   	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
