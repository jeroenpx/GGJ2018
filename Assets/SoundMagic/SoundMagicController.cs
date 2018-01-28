using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMagicController : MonoBehaviour {

	public MagicMaterialCollection caveMat;

	private float startTime;
	private float previousStartTime;

	private float preventActionTime = 2;

	private GameObject[] crystals;
	private bool[] activatedByWave;

	public AudioSource burstAudioEffect;

	void Start() {
		startTime = -1000;
		previousStartTime = -1000;
	}

	public void DoMagic() {
		previousStartTime = startTime;
		startTime = Time.time;

		// Find all crystals
		crystals = GameObject.FindGameObjectsWithTag("Crystal");
		activatedByWave = new bool[crystals.Length];

		// Play sound
		burstAudioEffect.Play();
	}

	void Update() {
		caveMat.SetFloat ("_TimeManual", Time.time);
		caveMat.SetFloat ("_TimeStart", startTime);
		caveMat.SetFloat ("_PreviousTimeStart", previousStartTime);

		// How much did the current wave travel?
		float waveTravelDist = caveMat.GetFloat("_Speed")*(Time.time-startTime);
		Vector4 sourcePoint = caveMat.GetVector ("_SourcePoint");
		float fadeOutDistance = caveMat.GetFloat ("_FadeOutDistance");

		// What Crystals are at those locations?
		if(crystals!=null) {
			for (int i = 0; i < crystals.Length; i++) {
				if (crystals [i] == null) {
					continue;
				}
				if (!activatedByWave [i]) {
					// Check if the wave passed by this crystal...
					float distanceFromSource = Vector3.Distance (crystals [i].transform.position, sourcePoint);
					if (distanceFromSource < waveTravelDist) {
						activatedByWave [i] = true;
						float activationPercent = Mathf.Clamp01 (1 - distanceFromSource / fadeOutDistance);
						crystals [i].GetComponent<CrystalControl> ().Activate (activationPercent);
					}
				}
			}
		}
	}

	[ContextMenu("Set Cave Dark")]
	void EditorDebugSetLightDark() {
		// Basic shader light
		caveMat.SetFloat ("_TimeManual", 100);
		caveMat.SetFloat ("_TimeStart", 0);
		caveMat.SetFloat ("_PreviousTimeStart", 0);

		// Disable Crystals
	}

	[ContextMenu("Set Cave Full Brightness")]
	void EditorDebugSetLightBright() {
		// Basic shader light
		caveMat.SetFloat ("_TimeManual", 2.5f);
		caveMat.SetFloat ("_TimeStart", 0);
		caveMat.SetFloat ("_PreviousTimeStart", -.5f);

		// Enable Crystals
	}

	[ContextMenu("Set Cave Crystal Only Brightness")]
	void EditorDebugSetLightLeftOverBrightNess() {
		// Basic shader light
		caveMat.SetFloat ("_TimeManual", 100);
		caveMat.SetFloat ("_TimeStart", 0);
		caveMat.SetFloat ("_PreviousTimeStart", 0);

		// Enable Crystals

	}
}
