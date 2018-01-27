using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaveTrigger : MonoBehaviour {

	public PlayerScoreKeeper scoreHandler;
	public SoundMagicController magicController;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			if(scoreHandler.ShrinkEnergy()) {
				magicController.DoMagic ();
			}
		}
	}
}
