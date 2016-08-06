using UnityEngine;
using System.Collections;

public class AttractionController : MonoBehaviour {

    public bool Deadly;
    public Rigidbody rBody;
    public float mag;

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
            if (rBody.velocity.magnitude > 0.75f && collision.collider.gameObject.layer == LayerMask.NameToLayer("Dummies"))
            {
                Dummy tmpDummy = collision.collider.transform.GetComponent<Dummy>();
                if(tmpDummy != null)
                {
                    tmpDummy.Die();
                } else {
                    Debug.LogWarningFormat("Dummy not found in: {0}", collision.collider.gameObject.name);
                }
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
        mag = rBody.velocity.magnitude;
	}
}
