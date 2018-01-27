using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreKeeper : MonoBehaviour {

	public Text scoreText;
	public RectTransform energyImage;

	public float scoreDistanceFactor;

	public float startZ;

	// Pixels of one unit in progress bar
	private float W = 38;

	public int energy = 5;
	private float reducing = 0;

	public float timeTillEnergyGone = 3;

	void Start () {
		startZ = transform.position.z;
	}

	public bool ShrinkEnergy() {
		if (energy > 0) {
			reducing += 1;
			energy -= 1;
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		// Update score
		int score = Mathf.FloorToInt((transform.position.z-startZ)*scoreDistanceFactor);
		scoreText.text = ""+score;

		reducing = Mathf.Max (0, reducing - Time.deltaTime / timeTillEnergyGone);

		// Update energy
		energyImage.sizeDelta = new Vector2((energy+reducing) * W, energyImage.sizeDelta.y);
	}
}
