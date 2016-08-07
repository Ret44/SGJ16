using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class GamepadContoller : MonoBehaviour
{

    public Player player;

	private Vector3 _forward = Vector3.zero;
	private Vector3 _right = Vector3.zero;

    void Awake()
    {
        player = GetComponent<Player>();

		_forward = new Vector3(-0.5f, 0.0f, 0.5f);
		_right = new Vector3(0.5f, 0.0f, 0.5f);
	}
	
    bool ButtonPressed()
    {
        //if(Input.GetButton)
        return false;
    }
	// Update is called once per frame
	void Update ()
	{
		Vector3 input = Vector3.zero;

		input += _right * Input.GetAxis("Horizontal");
		input += _forward * Input.GetAxis("Vertical");

		input.Normalize();
		
        player.input = input;

        if(Input.GetButtonDown("Call") || Input.GetKeyDown(KeyCode.Space) )
        {
            player.SendCall();
        }
	}
}
