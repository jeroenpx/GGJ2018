using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWingFlap : MonoBehaviour {

	public AudioSource windSound;

	// Use this for initialization
	void PlayFlap () {
		windSound.Play ();
	}
}
