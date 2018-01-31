using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaveTrigger : MonoBehaviour {

	public PlayerScoreKeeper scoreHandler;
	public SoundMagicController magicController;

	private bool isTouching = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			DoWaveTrigger ();
		}
	}

	void DoWaveTrigger() {
		if(scoreHandler.ShrinkEnergy()) {
			magicController.DoMagic ();
		}
	}
}
