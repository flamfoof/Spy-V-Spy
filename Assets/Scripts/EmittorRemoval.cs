using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmittorRemoval : MonoBehaviour {

	public float emittorSelfDestruct = 10.0f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, emittorSelfDestruct);
	} 
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > 15) {
			Destroy (gameObject);
		}
	}
}
