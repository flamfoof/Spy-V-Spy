using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SentryBotAI : MonoBehaviour {
	
	public enum SentryState{Patrol, Emittor, Chase};

	[Header("Sentry Movement Settings")]
	public float sentrySpeed = 1.0f;
	[Tooltip("Higher acceleration stops sentry from sliding")]
	public float sentryAcceleration = 1.0f;
	public float sentryAngularSpeed = 1.0f;
	public bool sentryAutoBrake = false;


	[Header("Sentry States and bools")]

	public SentryState sentryState;
	public float emittorRemovalTimer = 5.0f;
	public float timeToNoticeEmittor = 2.0f;
	[Tooltip("Controls the trigger from going crazy")]
	public bool entryControl = false;
	public bool inSight = false;
	public float emittorDetectionZone = 28.0f;
	public float lookBehindDistance = 4.0f;

	[Header("Debugging")]
	public int segment = 10;
	//24 radius is the distance measure between each patrol points
	public float xRadius = 5.0f;
	public float yRadius = 5.0f;


	private LineRenderer debugCircle;

	private float distance = 100.0f;
	private float confusedTimer = 6.0f;
	private bool isRunning = false;
	public GameObject player;
	public GameObject emittor;

	private NavMeshAgent agent;

	List<GameObject> possiblePath = new List<GameObject>();

	//Pathfinding
	[HideInInspector]
	public GameObject[] patrolPoints;
	[HideInInspector]
	public Transform nextPoint;
	[HideInInspector]
	public Transform currentPoint;
	[HideInInspector]
	public Transform previousPoint;
	[HideInInspector]
	public PatrolPaths currentPathNode;
	[HideInInspector]
	public List<GameObject> emittorArray = new List<GameObject> ();
	[HideInInspector]
	Rigidbody rigidbody;
	private bool waitForNextSearch = false;
	public BombUI bombUI;
	public GameObject confused;
	public bool isConfused;
	public GameObject RedLightOfTerror;
	//Finding player 



	void Awake () {
		rigidbody = GetComponent<Rigidbody>();
		agent = this.GetComponent<NavMeshAgent> ();
		sentryState = SentryState.Patrol;
		patrolPoints = GameObject.FindGameObjectsWithTag ("PatrolPoints");
		player = GameObject.FindGameObjectWithTag ("Player");
		FindInitialPatrol ();
		agent.speed = sentrySpeed;
		agent.angularSpeed = sentryAngularSpeed;
		agent.autoBraking = sentryAutoBrake;
		agent.acceleration = sentryAcceleration;
		bombUI = bombUI.GetComponent<BombUI> ();
		DebugCircleRange ();
	}
	

	void Update () {
		if (bombUI.canStart) {
			agent.speed = sentrySpeed + (2 * (bombUI.oldTime / bombUI.timeRemaining));
			agent.acceleration = sentryAcceleration + (2 * (bombUI.oldTime / bombUI.timeRemaining));
			//agent.angularSpeed = sentryAngularSpeed - (100 / (bombUI.timeRemaining / bombUI.oldTime));
		}

	}

	void FixedUpdate()
	{
		if (sentryState == SentryState.Patrol) {
			PatrolArea ();

			if (emittorInStage ()) {
				//Debug.Log ("THERE IS AN EMITTOR");
				if (waitForNextSearch == false) {
					waitForNextSearch = true;
					StartCoroutine (waitToFindEmittor ());
				}
			} else if (inSight) {
				sentryState = SentryState.Chase;
				RedLightOfTerror.SetActive (true);
			}
			if (Vector3.Distance (player.transform.position, transform.position) < lookBehindDistance && inSight) {
				inSight = true;
			} 
		} else if (sentryState == SentryState.Chase) {
			if (!inSight) {
				//Debug.Log ("Lost sight");
				StartCoroutine(ReturnOriginal ());
				RedLightOfTerror.SetActive (false);
			}
			RedLightOfTerror.SetActive (true);
			LookForPlayer ();
			ChasePlayer ();
		} else if (sentryState == SentryState.Emittor) {
			LookForEmittor ();
		}


	}

	//Patrolling functions
	void FindInitialPatrol()
	{
		if (currentPoint == null) {
			int indexClosest = 0;
			for (int i = 0; i < patrolPoints.Length; i++) {
				float newDistance = Vector3.Distance (gameObject.transform.position, patrolPoints [i].transform.position);
				if (newDistance < distance) {
					indexClosest = i;
					distance = newDistance;
				}
			}
			nextPoint = patrolPoints [indexClosest].transform;
		}
	}

	void PatrolArea()
	{
		agent.SetDestination (nextPoint.position);
	}

	public void FindSurroundingPoints()
	{
		distance = xRadius;
//		Debug.Log (distance);

		for (int i = 0; i < currentPathNode.paths.Count; i++) {
			if (previousPoint != currentPathNode.paths [i].transform && currentPoint != currentPathNode.paths [i].transform)
				possiblePath.Add (currentPathNode.paths [i].gameObject);
		}

		nextPoint = possiblePath [Random.Range (0, possiblePath.Count)].transform;

		ClearPossiblePath ();
	}

	void ClearPossiblePath()
	{
		possiblePath.RemoveRange (0, possiblePath.Count);
	}



	//player chasing
	public void LookForPlayer()
	{
		if (emittorInStage ()) {
			//Debug.Log ("THERE IS AN EMITTOR");
			if (waitForNextSearch == false) {
				waitForNextSearch = true;
				StartCoroutine (waitToFindEmittor ());
			}
		} 
	}

	public void ChasePlayer()
	{
		Debug.Log ("Chasing");
		agent.SetDestination (player.transform.position);
	}

	public IEnumerator ReturnOriginal()
	{
		yield return new WaitForSeconds (0.5f);
		if (sentryState == SentryState.Chase && !inSight) {
			currentPoint = null;
			FindInitialPatrol ();
			sentryState = SentryState.Patrol;
		}

	}
		

	//emittor functions
	public void LookForEmittor()
	{
		if (emittorArray.Count == 0 && !inSight && sentryState == SentryState.Emittor) {
			currentPoint = null;
			FindInitialPatrol ();
			RedLightOfTerror.SetActive (false);
			sentryState = SentryState.Patrol;
		} else if (emittorArray.Count > 0) {
			if(emittorArray[0].gameObject != null)
				agent.SetDestination (emittorArray [0].transform.position);
			if (isConfused) {
				if (!isRunning) {
					StartCoroutine (stopConfuse ());
					Debug.Log ("Unconfusing");
				}
				isConfused = false;
			}
		} else if (inSight) {
			sentryState = SentryState.Chase;
		}
		
	}

	bool emittorInStage()
	{
		if (GameObject.FindWithTag ("Emittor") != null && Vector3.Distance(GameObject.FindWithTag ("Emittor").transform.position, transform.position) < emittorDetectionZone) {
			return true;
		} else
			return false;
				
	}

	IEnumerator stopConfuse()
	{
		isRunning = true;

		yield return new WaitForSeconds (confusedTimer);
		Debug.Log ("FINALLY CONFUSED OFF");
		if (sentryState == SentryState.Emittor) {	
			currentPoint = null;
			FindInitialPatrol ();
			RedLightOfTerror.SetActive (false);
			confused.SetActive (false);
			if(!emittorInStage())
				sentryState = SentryState.Patrol;
		}
		isRunning = false;
	}

	public IEnumerator RemoveEmittor()
	{
		agent.isStopped = true;
		rigidbody.velocity = new Vector3 (0, 0, 0);
		yield return new WaitForSeconds (emittorRemovalTimer);
		emittorArray.Clear ();
		Destroy (emittor);
		confused.SetActive (false);
		StartCoroutine (waitToFindEmittor ());
		agent.isStopped = false;

	}

	IEnumerator waitToFindEmittor()
	{
		yield return new WaitForSeconds (timeToNoticeEmittor);
		if (GameObject.FindGameObjectsWithTag ("Emittor").Length != 0 ) {
			foreach (GameObject emittors in GameObject.FindGameObjectsWithTag ("Emittor")) {
				if (emittors != null && Vector3.Distance (emittors.transform.position, transform.position) < emittorDetectionZone) {
					emittorArray.Add (emittors);
					confused.SetActive (true);
					RedLightOfTerror.SetActive (false);
				}
			}
		}
		if (emittorArray.Count > 0) {
			sentryState = SentryState.Emittor;
			isConfused = true;
		}
		waitForNextSearch = false;
	}

	void DebugCircleRange()
	{
		debugCircle = gameObject.GetComponent<LineRenderer> ();

		debugCircle.positionCount = segment + 1;
		debugCircle.useWorldSpace = false;

		float x;
		float z = 0.0f;

		float angle = 5f;

		for(int i = 0; i < (segment + 1); i++)
		{
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
			z = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;

			debugCircle.SetPosition(i, new Vector3(x, 0, z));

			angle += (360f/segment);
		}
	}
}
