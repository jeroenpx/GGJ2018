using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMagicController : MonoBehaviour {

	public Material caveMaterial;

	private float startTime;
	private float previousStartTime;

	private float keepActionTime = 2;

	void Start() {
		startTime = -1000;
		previousStartTime = -1000;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			previousStartTime = startTime;
			startTime = Time.time;
		}


		caveMaterial.SetFloat ("_TimeManual", Time.time);
		caveMaterial.SetFloat ("_TimeStart", startTime);
		caveMaterial.SetFloat ("_PreviousTimeStart", previousStartTime);
	}
}
