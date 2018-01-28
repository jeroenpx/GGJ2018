using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	public AudioSource impactSound;

	void OnCollisionEnter(Collision coll) {
		impactSound.Play ();
		transform.parent.GetComponent<PlayerControl>().ReturnAfterDeath ();
	}
}
