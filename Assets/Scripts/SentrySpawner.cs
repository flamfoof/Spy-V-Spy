using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentrySpawner : MonoBehaviour {

	public GameObject sentry;
	public PlayerController player;
	// Use this for initialization
	void Start () {
		player = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.B)) && Input.GetKey (KeyCode.Equals)) {
			GameObject newBot = Instantiate (sentry, transform.position, Quaternion.identity);
			player.shbeebMode = true;
		}
	}
}
