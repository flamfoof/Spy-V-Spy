using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	static public bool isPaused = false;

	public GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
			Pause ();
		}
	}

	public void Pause()
	{
		
		if (pauseMenu != null) {
			isPaused = !isPaused;
			if (isPaused) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				Time.timeScale = 0;
				pauseMenu.SetActive (true);
			} else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				Time.timeScale = 1;
				pauseMenu.SetActive (false);
			}
		}
	}

	public void toMainMenu ()
	{

		SceneManager.LoadScene (0);
	}
}
