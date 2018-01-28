using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyScript : MonoBehaviour {

	private Vector3 startPos;

	private Vector3 nextPos;
	private float timeNextPos;
	private Vector3 currentDampVelocity;

	public float smoothTime;

	public float timeToNextPos;
	public float radius;

	private Vector3 lastFramePos;

	void Start() {
		startPos = transform.position;
		nextPos = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > timeNextPos){
			timeNextPos = Time.time + timeToNextPos;
			nextPos = Random.insideUnitSphere * radius;

		}
		lastFramePos = transform.position;
		transform.position = Vector3.SmoothDamp(transform.position, nextPos+transform.parent.position, ref currentDampVelocity, smoothTime);
		transform.rotation = Quaternion.LookRotation (transform.position - lastFramePos, Vector3.up);
		Debug.DrawLine (lastFramePos, transform.position, Color.green, 5);
	}
}
