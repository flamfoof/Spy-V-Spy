//Allows multiple SceneView cameras in the editor to be setup to follow gameobjects.
//October 2012 - Joshua Berberick
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootStepsOn : MonoBehaviour { 
 
	public PlayerController player;
	public CharacterController _controller;

	 public AudioClip mySound;
	 public AudioSource mySource;

	 
	 public bool isKeyHeld;

  
	 void Start() {
	 // in method, assign the clip to the audioSource
	 mySource.clip = mySound;
	 // AudioSource.Play();
		player = player.GetComponent<PlayerController>();
	 }
	 
	 void Update() {
	 if (Input.GetKeyDown (KeyCode.P))
			{	  
			mySource.Stop();
			}	 
	 }
	 void FixedUpdate() {
		if (player.isMoving && player.isGrounded == true) {	
			isKeyHeld = true;
			if (isKeyHeld && !mySource.isPlaying) {			
				Debug.Log ("Walk again");
				StartCoroutine (playWalk ());
			}
		}

		if ((player.forwardSpeed == 0 && player.sideSpeed == 0)) {	  
			mySource.Stop ();
			isKeyHeld = false;

		} 
		if (player.isGrounded == false) {
			mySource.Stop ();
		} 
			
	}

	IEnumerator playWalk()
	{
		while(!mySource.isPlaying)
		{
			yield return new WaitForFixedUpdate();
			mySource.Play ();
		}
	}

}

	 