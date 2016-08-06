using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public static ParticleController instance;

    public GameObject breakingParticle;
	// Use this for initialization

    public static void SpawnBreaking(Vector3 pos)
    {
        Destroy(Instantiate(instance.breakingParticle, pos, Quaternion.identity) as GameObject, 2f);
    }

    void Awake()
    {
        instance = this;
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
