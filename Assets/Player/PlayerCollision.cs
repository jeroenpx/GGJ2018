using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	void OnCollisionEnter(Collision coll) {
		Debug.Log ("Hit something");
		transform.parent.GetComponent<PlayerControl>().ReturnAfterDeath ();
	}
}
