using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmittorUI : MonoBehaviour {


	public Image emittorRed;
	public GameObject player;

	public float emittorTimeRemaining = 30.0f;
	public float emittorOldTime = 30.0f;

	public bool emittorCanStart = false;
	// Use this for initialization
	void Start () {
		emittorTimeRemaining = player.GetComponent<PlayerController> ().emittorWaitTime;
		emittorOldTime = emittorTimeRemaining;
	}


	public void UpdateEmittorTimer()
	{
		if (emittorCanStart && gameObject.tag == "EmittorUI") {
			emittorTimeRemaining -= Time.fixedDeltaTime;
			emittorRed.fillAmount = emittorTimeRemaining / emittorOldTime;
			//Debug.Log ("Time remaining: " + emittorTimeRemaining);
			if (emittorTimeRemaining < 0) {
				//Debug.Log ("Emittor Restored");
				emittorTimeRemaining = emittorOldTime;
				emittorRed.fillAmount = emittorTimeRemaining / emittorOldTime;
			}
		} else if (emittorCanStart && gameObject.tag == "EmittorUI") {
			emittorTimeRemaining = emittorOldTime;
			emittorRed.fillAmount = emittorTimeRemaining / emittorOldTime;

		}
	}
}
