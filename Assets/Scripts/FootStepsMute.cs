//Allows multiple SceneView cameras in the editor to be setup to follow gameobjects.
//October 2012 - Joshua Berberick
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FootStepsMute : MonoBehaviour { 

	public PlayerController player;
	public CharacterController _controller;

	public AudioClip mySound;
	public AudioSource mySource;
 
 	void Awake() {
		player = player.GetComponent<PlayerController>();
		  }
 
	 void Start() {
	 // in method, assign the clip to the audioSource
	 mySource.clip = mySound;
	 // AudioSource.Play();
	 }
	 
	 void Update() {
	 if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D))
			{	  
			
					mySource.Stop();
			}

				if ( player.isGrounded == false) {
					mySource.Stop();
					} 
		}
	 }	 