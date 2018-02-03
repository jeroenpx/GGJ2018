using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalControl : MonoBehaviour {

	public Light affectedlight;
	public GameObject affectedMeshObject;
	private Mesh affectedMesh;


	private float maxIntensity;
	private float maxRange;
	private Color[] originalColors;

	public AnimationCurve activationEffect;
	public float activationDuration;

	public float activationStart;

	void Awake () {
		affectedMesh = affectedMeshObject.GetComponent<MeshFilter> ().mesh;

		maxIntensity = affectedlight.intensity;
		maxRange = affectedlight.range;
		int vCount = affectedMesh.vertexCount;
		Color[] colors = affectedMesh.colors;
		if (vCount != colors.Length) {
			colors = new Color[vCount];
			for (int i = 0; i < vCount; i++) {
				colors [i] = Color.white;
			}
		}
		affectedMesh.colors = colors;
		originalColors = new Color[colors.Length];
		for (int i = 0; i < colors.Length; i++) {
			originalColors [i] = colors [i];
		}
		activationStart = -1000;
	}

	// Update is called once per frame
	void Update () {
		float percentActivation = activationEffect.Evaluate(Mathf.Clamp01((Time.time-activationStart)/activationDuration));

		// 1. Set Vertex Colors of Crystal
		Color[] colors = affectedMesh.colors;
		for (int i = 0; i < colors.Length; i++) {
			colors [i] = Color.Lerp (Color.black, originalColors [i], percentActivation);
		}
		affectedMesh.colors = colors;

		// 2. Set Intensity of Light
		affectedlight.intensity = maxIntensity*percentActivation;
		affectedlight.range = maxRange*percentActivation;
	}

	public void Activate(float percent) {
		Debug.Log ("Activated!!!");
		activationStart = Time.time-(1-percent)*activationDuration;
	}
}
