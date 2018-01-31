using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	// How fast do we go (units/second)
	public float velocity = 1;

	private Vector3 smoothDirectionVelocity;
	private Vector3 moveDirection = Vector3.forward;
	public float moveSmoothTime=1;
	public float moveSmoothTimeCrisp = 0;

	// How much can we move left/right
	public float movePotential = 1;
	public float movePotentialCrisp = 0;

	// The bat (to rotate)
	public Transform bat;
	public Animator batanim;
	public float tiltAmount = 45;
	private Vector3 moveDirectionPrev;

	private int COUNT = 10;
	private Vector3[] lastPositions;
	private Vector3[] lastVelocities;
	private Vector3[] lastCamPos;
	private int lastPositionIndex;
	public float secondsReturnOnDead = 4;
	private float savePointDelta;
	private float nextDeathSavePointTime;
	public Transform cam;

	private bool death = false;

	public float deathPart1WaitDuration = 1;
	public float deathPart2TweenDuration = 1;
	public float deathPart2WaitDuration = 1;

	public float deathShakeAmount = 2;//The amount to shake this frame.
	public float deathShakeDuration = .4f;//The duration this frame.

	public bool crispMovement = false;

	public Canvas mobileMovementCanvas;
	public RectTransform mobileRectTransform;
	public RectTransform mobileRectTransformWave;
	private int lastTouchId;
	private bool wasActivateWave = false;

	private bool debugEmulateTouchWithMouse = false;

	void Awake() {
		savePointDelta = secondsReturnOnDead / COUNT;
		ResetDeathPoints ();
		/*
		#if !UNITY_EDITOR
		#if UNITY_ANDROID
		crispMovement = true;
		#endif
		#endif
		*/

		bool showMobileControls = false;

		#if UNITY_ANDROID
		showMobileControls = true;
		#endif
		#if UNITY_IOS
		showMobileControls = true;
		#endif

		if (!showMobileControls) {
			Destroy(mobileRectTransform.gameObject);
			Destroy(mobileRectTransformWave.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}

	public void ReturnAfterDeath() {
		if (death)
			return;

		GetComponent <PlayerScoreKeeper> ().Die ();
		StartCoroutine (ReturnAfterDeathCo ());
	}

	public void Eat() {
		// Do animation
		GetComponent <PlayerScoreKeeper>().IncreaseEnergy(transform.position);
	}

	IEnumerator ReturnAfterDeathCo() {
		death = true;
		batanim.SetBool ("Death", true);
		// Make the camera shake
		Camera.main.GetComponent<CameraShake> ().ShakeCamera (deathShakeAmount, deathShakeDuration);

		// Which point to return to?
		int returnIndex = (lastPositionIndex + 1) % COUNT;
		Vector3 returnTo = lastPositions [returnIndex];
		Vector3 returnMoveTo = lastVelocities [returnIndex];
		Vector3 cameraTo = lastCamPos [returnIndex];

		// Tween rotation & position
		Vector3 moveDirectionDiff = (moveDirection - moveDirectionPrev) / Time.deltaTime;
		Quaternion rotationFrom = Quaternion.AngleAxis (moveDirectionDiff.x * tiltAmount, Vector3.back) * Quaternion.LookRotation (Vector3.forward + movePotential * moveDirection, Vector3.up);
		Quaternion rotationTo = Quaternion.identity * Quaternion.LookRotation (Vector3.forward + movePotential * returnMoveTo, Vector3.up);
		Vector3 returnFrom = transform.position;
		Vector3 cameraFrom = cam.position;

		// Wait (part 1)
		yield return new WaitForSeconds(deathPart1WaitDuration);

		// Tween parameters (part 2)
		float totalPassedTime = 0;
		while (totalPassedTime < deathPart2TweenDuration) {
			float t = totalPassedTime / deathPart2TweenDuration;
			transform.position = Vector3.Lerp(returnFrom, returnTo, t);
			bat.transform.rotation = Quaternion.Lerp (rotationFrom, rotationTo, t);
			totalPassedTime += Time.deltaTime;

			// Move camera also
			cam.position = Vector3.Lerp(cameraFrom, cameraTo, t);

			yield return new WaitForSeconds (0);
		}

		// Wait (part 3)
		yield return new WaitForSeconds(deathPart2WaitDuration);

		// Return to live
		smoothDirectionVelocity = Vector3.zero;
		moveDirection = returnMoveTo;
		moveDirectionPrev = moveDirection;
		ResetDeathPoints();
		batanim.SetBool ("Death", false);
		death = false;
	}

	/**
	 * Make sure that if you die immediatelly, you simply return to the start
	 */
	void ResetDeathPoints() {
		lastPositions = new Vector3[COUNT];
		lastVelocities = new Vector3[COUNT];
		lastCamPos = new Vector3[COUNT];
		lastPositionIndex = 0;
		for (int i = 0; i < COUNT; i++) {
			lastPositions [i] = transform.position;
			lastVelocities [i] = moveDirection;
			lastCamPos [i] = cam.position;
		}
		nextDeathSavePointTime = Time.time + savePointDelta;
	}

	Vector2 handleAccel (Vector2 accel) {
		accel *= 5;// acceleration effect factor
		if (accel.sqrMagnitude > 1) {
			return accel.normalized;
		}
		return accel;
	}
	
	// Update is called once per frame
	void Update () {
		if (!death) {
			// KEEP TRACK of last positions
			if (Time.time > nextDeathSavePointTime) {
				nextDeathSavePointTime = nextDeathSavePointTime + savePointDelta;
				Vector3 debugPos = lastPositions [lastPositionIndex];
				lastPositionIndex = (lastPositionIndex + 1) % COUNT;
				lastPositions [lastPositionIndex] = transform.position;
				lastVelocities [lastPositionIndex] = moveDirection;
				lastCamPos [lastPositionIndex] = cam.position;
				Debug.DrawLine (debugPos, transform.position, Color.red, secondsReturnOnDead);
			}

			// MOVEMENT
			moveDirectionPrev = moveDirection;

			float horizSpeed = Input.GetAxis ("Horizontal");
			float vertSpeed = Input.GetAxis ("Vertical");
			/*#if !UNITY_EDITOR
			#if UNITY_ANDROID
			Vector2 accel = handleAccel(new Vector2(Input.acceleration.x, Input.acceleration.y));
			horizSpeed = accel.x;
			vertSpeed = accel.y;
			#endif
			#endif*/

			if (mobileRectTransform != null) {

				// 1) Find Touch inside circle
				Touch[] touches = new Touch[Input.touchCount];
				for (int i = 0; i < touches.Length; i++) {
					touches [i] = Input.GetTouch (i);
				}
				bool emulateTouchWithMouse = Input.GetKey (KeyCode.Mouse1) && debugEmulateTouchWithMouse;

				// For debugging
				if (emulateTouchWithMouse) {
					touches = new Touch[1];
				}

				bool activateWave = false;

				for (int i = 0; i < touches.Length; i++) {
					Touch t = touches [i];
					Vector2 touchPos = t.position;
					if (emulateTouchWithMouse) {
						touchPos = Input.mousePosition;
					}
					Vector3 posOnScreen = Camera.main.ScreenToViewportPoint (touchPos);
					Vector2 UIpos = new Vector2 (posOnScreen.x * Screen.width / mobileMovementCanvas.scaleFactor, posOnScreen.y * Screen.height / mobileMovementCanvas.scaleFactor);
					//mobileRectTransform.anchoredPosition;
					//mobileRectTransform.pivot;

					Vector2 diff = ((Vector2)(mobileRectTransform.position / mobileMovementCanvas.scaleFactor) - UIpos) / 75;
					bool active = false;
					if (diff.sqrMagnitude < 1) {
						// In circle
						lastTouchId = t.fingerId;
						active = true;
					} else if (!emulateTouchWithMouse && t.fingerId == lastTouchId) {
						diff = diff.normalized;
						active = true;
					}
					if (active) {
						horizSpeed = -diff.x;
						vertSpeed = -diff.y;
					}

					Vector2 diffWithButton = ((Vector2)(mobileRectTransformWave.position / mobileMovementCanvas.scaleFactor) - UIpos) / 75;
					if (diffWithButton.sqrMagnitude < 1) {
						activateWave = true;
					}
				}
				if (!wasActivateWave && activateWave) {
					SendMessage ("DoWaveTrigger");
				}
				wasActivateWave = activateWave;
			}

			Vector3 goalMoveDirection = new Vector3 (horizSpeed, vertSpeed, 0);
			Vector3 goalDirectionRotated = Vector3.Normalize (moveDirection + (crispMovement?movePotentialCrisp:movePotential) * (Quaternion.LookRotation (moveDirection.normalized, Vector3.up) * goalMoveDirection));
			if (crispMovement) {
				moveDirection = goalDirectionRotated;
			} else {
				moveDirection = Vector3.SmoothDamp (moveDirection, goalDirectionRotated, ref smoothDirectionVelocity, crispMovement ? moveSmoothTimeCrisp : moveSmoothTime);
			}

			Vector3 moveDirectionDiff = (moveDirection - moveDirectionPrev) / Time.deltaTime;

			// Move
			transform.Translate (moveDirection * velocity*Time.deltaTime * 60);

			// Rotate bat
			bat.transform.rotation = Quaternion.AngleAxis (moveDirectionDiff.x * tiltAmount, Vector3.back) * Quaternion.LookRotation (moveDirection, Vector3.up);
		}
	}
}
