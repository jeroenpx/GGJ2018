using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyMarker : MonoBehaviour {
	public Vector3 fromPos;
	public Vector3 toPos;

	private float t;
	public float duration;

	public AnimationCurve scaleCurve;
	public float scaleFactor;

	private RectTransform recttransform;

	// Use this for initialization
	void Start () {
		t = Time.time;
		recttransform = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		float percentComplete = (Time.time - t) / duration;
		if (percentComplete > 1) {
			Destroy (gameObject);
		}
		recttransform.anchoredPosition = Vector3.Lerp(fromPos, toPos, percentComplete);

		float scale = scaleFactor * scaleCurve.Evaluate (percentComplete);
		transform.localScale = new Vector3(scale, scale, scale);
	}
}
