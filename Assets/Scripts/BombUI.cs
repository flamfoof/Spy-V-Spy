using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BombUI : MonoBehaviour {

	public Image bombRed;
	public GameObject player;
	public float timeRemaining = 30.0f;
	public float oldTime = 30.0f;

	public bool canStart = false;


	void Start()
	{
		oldTime = timeRemaining;

		Debug.Log (oldTime);
	}
	public void UpdateBombTimer()
	{
		if (canStart && gameObject.tag == "BombUI") {
			timeRemaining -= Time.fixedDeltaTime;
			//Debug.Log ("Time remaining: " + timeRemaining);
			bombRed.fillAmount = timeRemaining / oldTime;
			if (timeRemaining < 0) {
				//Game Over
				//Debug.Log("Game Over");
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				SceneManager.LoadScene(3);
			}
		}


	}
}
