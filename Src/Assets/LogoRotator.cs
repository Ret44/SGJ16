using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogoRotator : MonoBehaviour {

    public float rotationSpeed;
    public GameObject redLogo;
    public GameObject blueLogo;
    public GameObject whiteLogo;

    private float timeCounter;

	// Use this for initialization
	void Start () {
	    //redLogo.transform.position = new Vector3()
        timeCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
       redLogo.transform.Translate(0, 0, Time.deltaTime * rotationSpeed); // move forward
       redLogo.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

       timeCounter += Input.GetAxis("Horizontal") * Time.deltaTime; // multiply all this with some speed variable (* speed);
       float x = Mathf.Cos(timeCounter);
       float y = Mathf.Sin(timeCounter);
       float z = 0;
       transform.position = new Vector3(x, y, z);
	}
}
