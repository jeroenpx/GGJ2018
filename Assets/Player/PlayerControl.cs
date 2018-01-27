using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	// How fast do we go (units/second)
	public float velocity = 1;

	private Vector3 smoothDirectionVelocity;
	private Vector3 moveDirection;
	public float moveSmoothTime=1;

	// How much can we move left/right (1 = 45° -> tan(hoek))
	public float movePotential = 1;

	// The bat (to rotate)
	public Transform bat;
	public float tiltAmount = 45;
	private Vector3 moveDirectionPrev;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		moveDirectionPrev = moveDirection;

		float horizSpeed = Input.GetAxis ("Horizontal");
		float vertSpeed = Input.GetAxis ("Vertical");
		Vector3 goalMoveDirection = new Vector3 (horizSpeed, vertSpeed, 0);
		moveDirection = Vector3.SmoothDamp(moveDirection, goalMoveDirection, ref smoothDirectionVelocity, moveSmoothTime);

		Vector3 moveDirectionDiff = (moveDirection - moveDirectionPrev)/Time.deltaTime;

		Vector3 direction = Vector3.forward + movePotential * moveDirection;

		// Move
		transform.Translate (direction*velocity);

		// Rotate bat
		bat.transform.rotation = Quaternion.AngleAxis(moveDirectionDiff.x*tiltAmount, Vector3.back) * Quaternion.LookRotation(direction, Vector3.up);
	}
}
