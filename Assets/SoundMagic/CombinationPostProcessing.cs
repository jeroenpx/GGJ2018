using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CombinationPostProcessing : MonoBehaviour {

	// This camera
	private Camera cam;

	// A very simple shader which is affected by lights and has no real textures applied
	public Shader simpleLightShader;

	// All the FG lights
	private List<Light> fglights;

	// The internal camera to use (for the RenderTexture)
	public Camera internalCam;

	// The Material to combine both renders
	public Material combinationMaterial;

	void Awake() {
		// Get the camera
		cam = GetComponent<Camera> ();

		// Collect all FG lights
		GameObject[] fglightsgo = GameObject.FindGameObjectsWithTag("FGLight");
		fglights = new List<Light> ();
		foreach (GameObject fglightgo in fglightsgo) {
			fglights.Add (fglightgo.GetComponent<Light> ());
		}
	}

	void ToggleFGLights(bool toggleOn) {
		foreach (Light l in fglights) {
			l.enabled = toggleOn;
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {

		// -- Prepare the internal camera --
		internalCam.CopyFrom(cam);
		internalCam.SetReplacementShader (simpleLightShader, "RenderType");
		// Disable FG lights
		ToggleFGLights(false);
		// Set the render texture
		RenderTexture caveLightRender = new RenderTexture ( Screen.width, Screen.height, 24);
		internalCam.targetTexture = caveLightRender;
		internalCam.backgroundColor = Color.black;
		try {
			internalCam.Render ();

			// Combine stuff now.
			combinationMaterial.SetTexture("_EmitTex", caveLightRender);
			Graphics.Blit(src, dest, combinationMaterial);

		} finally {
			caveLightRender.Release();
			internalCam.targetTexture = null;
		}
		ToggleFGLights(true);
	}
}
