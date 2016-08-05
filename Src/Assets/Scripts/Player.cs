using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public static Player instance;

    public float speed;
    public Vector3 velocity;
    
    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        this.transform.Translate(velocity * speed);
    }
}
