using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMaterialCollection : MonoBehaviour {

	// All materials that will share properties
	public List<Material> allAffectedMaterials;

	// The one with all the options
	public Material primaryMaterial;

	// Use this for initialization
	void Awake () {
		
	}

	[ContextMenu("Do Copy")]
	void Copy () {
		foreach (Material mat in allAffectedMaterials) {
			Texture mainTex = mat.GetTexture ("_MainTex");
			Texture normTex = mat.GetTexture ("_BumpMap");
			float gloss = mat.GetFloat("_Glossiness");
			float metalic = mat.GetFloat("_Metallic");
			float emit = mat.GetFloat("_EmitMult");
			mat.CopyPropertiesFromMaterial (primaryMaterial);
			mat.SetTexture ("_MainTex", mainTex);
			mat.SetTexture ("_BumpMap", normTex);
			mat.SetFloat ("_Glossiness", gloss);
			mat.SetFloat ("_Metallic", metalic);
			mat.SetFloat("_EmitMult", emit);


		}
	}
	
	public void SetFloat(string name, float value) {
		primaryMaterial.SetFloat (name, value);
		foreach (Material mat in allAffectedMaterials) {
			mat.SetFloat (name, value);
		}
	}

	public float GetFloat(string name) {
		return primaryMaterial.GetFloat (name);
	}

	public void SetVector(string name, Vector3 value) {
		primaryMaterial.SetVector (name, value);
		foreach (Material mat in allAffectedMaterials) {
			mat.SetVector (name, value);
		}
	}

	public Vector3 GetVector(string name) {
		return primaryMaterial.GetVector (name);
	}
}
