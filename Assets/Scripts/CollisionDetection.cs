using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionDetection : MonoBehaviour {

	public SentryBotAI sentry;

	GameObject obj;

	void Start()
	{
		sentry = sentry.GetComponent<SentryBotAI> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PatrolPoints" && sentry.entryControl == false && gameObject.name == "CollisionDetector" && sentry.sentryState == SentryBotAI.SentryState.Patrol) {
//			Debug.Log ("collided");
			sentry.entryControl = true;
			sentry.previousPoint = sentry.currentPoint;
			sentry.currentPoint = other.transform;
			sentry.currentPathNode = other.GetComponent<PatrolPaths>();
			if(sentry.currentPoint != sentry.previousPoint)
				sentry.FindSurroundingPoints ();
		}
		if (other.tag == "Emittor" && gameObject.name == "EmittorCollision") {
//			Debug.Log ("HIT EMITTOR");
			sentry.emittor = other.gameObject;
			sentry.StartCoroutine (sentry.RemoveEmittor ());
		}

		if (other.tag == "Player") {
			//Debug.Log ("You just got caught");
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			SceneManager.LoadScene (3);

		}
			
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "PatrolPoints" && sentry.entryControl == true) {
//			Debug.Log ("un-collided");
			sentry.entryControl = false;
		}
	}
}
