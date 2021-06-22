using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour {

	public PlayerController player;

	void Start()
	{
		player = player.GetComponent<PlayerController> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Ground") {
			player.isGrounded = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ground") {
			player.isGrounded = false;
		}
	}

}

