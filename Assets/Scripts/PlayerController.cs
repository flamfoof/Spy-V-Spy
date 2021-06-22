using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
	[Header("Movement Properties")]
	public float movementSpeed = 5.0f;
	public float mouseSensitivity = 5.0f;
	public float forwardSpeed = 0.0f;
	public float sideSpeed = 0.0f;


	[Header("Jump Properties")]
	[Range(0.1f, 20.0f)]
	public float jumpHeight = 10.0f;

	[Tooltip("Time to reach the highest point of jump. Total jump time is Apex * 2")]
	[Range(0.1f, 3.0f)]
	public float timeToJumpApex = 0.5f;

	//hidden values
	private float gravity = 0.0f;
	private float jumpSpeed = 20.0f;


	[Header("Field of View Properties")]
	public float upDownRange = 60.0f;
	
	public bool seen = false;
	public bool hasEmittor = false;
	public float emittorWaitTime = 10.0f;
	public GameObject emittor;
	public GameObject playerEmittorHold;
	public BombUI bombUI;
	public EmittorUI emittorBombUI;
	public GameObject emittorUI;
	public bool isGrounded;
	public bool isMoving = false;
	public bool shbeebMode = false;
	public GameObject shbeebHead;

	//Hidden values
	private float verticalRotation = 0;
	private float verticalVelocity = 0;
	
	
	CharacterController characterController;
	
	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		characterController = GetComponent<CharacterController>();

		//Creating physics based on the jump height and the apex(the time to reach the highest point)
		//Total jump time is apex * 2
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpSpeed = Mathf.Abs (gravity * timeToJumpApex);

		hasEmittor = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!MenuController.isPaused) {
			FollowMouse ();
		}

		//Get Right Click
		if (Input.GetMouseButton (1)) {
			PlaceEmittor ();
		}

		//gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		//jumpSpeed = Mathf.Abs (gravity * timeToJumpApex);
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpSpeed = Mathf.Abs (gravity * timeToJumpApex);
		Move ();
	}

	void FixedUpdate()
	{
		//Physics go here.
		verticalVelocity += gravity * Time.deltaTime;

		bombUI.UpdateBombTimer ();
		emittorBombUI.UpdateEmittorTimer ();

		if (shbeebMode == true) {
			shbeebHead.SetActive (true);
		}
	}

	void FollowMouse()
	{
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
	}

	void Move()
	{
		forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

		if (characterController.isGrounded && Input.GetButton ("Jump")) {
			verticalVelocity = jumpSpeed;
		} else if (characterController.isGrounded) {
			//This way, if a player were to fall off a platform, they wouldn't be goiing at the
			//same speed of a sky diver. 
			verticalVelocity = 0;
		}
		if ((forwardSpeed != 0 || sideSpeed != 0)) {
			isMoving = true;
		} else {
			isMoving = false;
		}
		Vector3 speed = new Vector3( sideSpeed, verticalVelocity, forwardSpeed);

		speed = transform.rotation * speed;

		characterController.Move( speed * Time.deltaTime );
	}

	void PlaceEmittor()
	{
		if (hasEmittor) {
			hasEmittor = false;
			playerEmittorHold.SetActive (false);
			emittorUI.SetActive (true);
			emittorBombUI.emittorCanStart = true;
			StartCoroutine (WaitForEmittor ());
			GameObject newEmittor = Instantiate (emittor, gameObject.transform.position + (transform.forward * 3), Quaternion.identity);
			newEmittor.transform.rotation = transform.rotation;
			
		}
	}

	IEnumerator WaitForEmittor()
	{
		yield return new WaitForSeconds (emittorWaitTime);
		emittorUI.SetActive (false);
		emittorBombUI.emittorCanStart = false;
		playerEmittorHold.SetActive (true);
		hasEmittor = true;
	}

}