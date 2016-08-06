using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class GamepadContoller : MonoBehaviour {

    public Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }
	// Use this for initialization
	void Start () {
	
	}
	
    bool ButtonPressed()
    {
        //if(Input.GetButton)
        return false;
    }
	// Update is called once per frame
	void Update () {
        player.velocity = Vector3.zero + (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

        if(Input.GetButtonDown("Call")
#if UNITY_EDITOR
			|| Input.GetKeyDown(KeyCode.Space)
#endif
			)
        {
            player.SendCall();
        }
	}
}
