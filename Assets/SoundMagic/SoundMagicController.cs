using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMagicController : MonoBehaviour {

	public Material caveMaterial;

	private float startTime;
	private float previousStartTime;

	private float keepActionTime = 2;

	private GameObject[] crystals;
	private bool[] activatedByWave;

	void Start() {
		startTime = -1000;
		previousStartTime = -1000;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			previousStartTime = startTime;
			startTime = Time.time;

			// Find all crystals
			crystals = GameObject.FindGameObjectsWithTag("Crystal");
			activatedByWave = new bool[crystals.Length];
		}

		caveMaterial.SetFloat ("_TimeManual", Time.time);
		caveMaterial.SetFloat ("_TimeStart", startTime);
		caveMaterial.SetFloat ("_PreviousTimeStart", previousStartTime);

		// How much did the current wave travel?
		float waveTravelDist = caveMaterial.GetFloat("_Speed")*(Time.time-startTime);
		Vector4 sourcePoint = caveMaterial.GetVector ("_SourcePoint");

		// What Crystals are at those locations?
		if(crystals!=null) {
			for (int i = 0; i < crystals.Length; i++) {
				if (!activatedByWave [i]) {
					// Check if the wave passed by this crystal...
					float distanceFromSource = Vector3.Distance (crystals [i].transform.position, sourcePoint);
					if (distanceFromSource < waveTravelDist) {
						activatedByWave [i] = true;
						crystals [i].GetComponent<CrystalControl> ().Activate ();
					}
				}
			}
		}
	}

	[ContextMenu("Set Cave Dark")]
	void EditorDebugSetLightDark() {
		// Basic shader light
		caveMaterial.SetFloat ("_TimeManual", 100);
		caveMaterial.SetFloat ("_TimeStart", 0);
		caveMaterial.SetFloat ("_PreviousTimeStart", 0);

		// Disable Crystals
	}

	[ContextMenu("Set Cave Full Brightness")]
	void EditorDebugSetLightBright() {
		// Basic shader light
		caveMaterial.SetFloat ("_TimeManual", 2.5f);
		caveMaterial.SetFloat ("_TimeStart", 0);
		caveMaterial.SetFloat ("_PreviousTimeStart", -.5f);

		// Enable Crystals
	}

	[ContextMenu("Set Cave Crystal Only Brightness")]
	void EditorDebugSetLightLeftOverBrightNess() {
		// Basic shader light
		caveMaterial.SetFloat ("_TimeManual", 100);
		caveMaterial.SetFloat ("_TimeStart", 0);
		caveMaterial.SetFloat ("_PreviousTimeStart", 0);

		// Enable Crystals

	}
}
