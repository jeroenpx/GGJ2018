using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaveTrigger : MonoBehaviour {

	public PlayerScoreKeeper scoreHandler;
	public SoundMagicController magicController;
	
	// Update is called once per frame
	void Update () {
		// Input.GetKeyDown (KeyCode.Space)
		if (Input.GetButtonDown("Jump")) {
			if(scoreHandler.ShrinkEnergy()) {
				magicController.DoMagic ();
			}
		}
	}
}
