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
			mat.CopyPropertiesFromMaterial (primaryMaterial);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
