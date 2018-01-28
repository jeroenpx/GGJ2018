using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyEatScript : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Bat") {
			PlayerControl control = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
			control.Eat ();
			Destroy (gameObject);
		}
	}
}
