using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHelper : MonoBehaviour {

	public GameObject particle;
	public GameObject angryLight;
	public GameObject playerBombUI;
	public GameObject exit;
	public GameObject AudioSauce;
	

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Vent") {
//			Debug.Log ("Creak creak");
			other.GetComponent<Rigidbody> ().isKinematic = false;
			other.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -15, 0);
			Physics.gravity = new Vector3 (0, -15.0f, 0);
		}

		if (other.tag == "Bomb") {
			particle.SetActive (true);
			angryLight.SetActive (true);
			playerBombUI.SetActive (true);
			playerBombUI.GetComponent<BombUI> ().canStart = true;
			exit.SetActive (true);
			AudioSauce.SetActive (true);
		}
		if (other.tag == "Exit") {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			SceneManager.LoadScene (2);
		}

	}
}
