using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowBat : MonoBehaviour {

	public Transform player;

	public float keepDistance=5;

	public float emitFront = 4;

	public MagicMaterialCollection caveMat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		caveMat.SetVector ("_SourcePoint", player.position+new Vector3(0, 0, emitFront));

		Vector3 target = player.position+(transform.position-player.position).normalized*keepDistance;
		transform.position = target;
		//transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, smoothTime);
	}
}
