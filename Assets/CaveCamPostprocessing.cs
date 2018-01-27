using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCamPostprocessing : MonoBehaviour {

	public Material caveMaterial;

	private float startTime;
	private float endTime;
	private float previousEndTime;

	void Start() {
		
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			previousEndTime = endTime;
			startTime = Time.time;
		}
		if (Input.GetKey (KeyCode.Space)) {
			endTime = Time.time;
		}

		caveMaterial.SetFloat ("_PreviousTimeEnd", previousEndTime);
		caveMaterial.SetFloat ("_TimeManual", Time.time);
		caveMaterial.SetFloat ("_TimeStart", startTime);
		caveMaterial.SetFloat ("_TimeEnd", endTime);
	}

	/*void OnRenderImage(RenderTexture src, RenderTexture dest) {
		

		//Graphics.Blit(src, dest, caveMaterial);
	}*/
}
