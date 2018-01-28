using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreKeeper : MonoBehaviour {

	public Text scoreText;
	public RectTransform energyImage;
	public Canvas canvas;
	public RectTransform endPoint;
	public GameObject markerPrefab;
	public Text lifeText;

	public float scoreDistanceFactor;

	public float startZ;

	public float life = 10;

	// Pixels of one unit in progress bar
	private float W = 38;

	public int energy = 5;
	private float reducing = 0;

	public float timeTillEnergyGone = 3;

	void Start () {
		startZ = transform.position.z;
	}
	public void IncreaseEnergy(Vector3 worldpos) {
		Vector3 posOnScreen = Camera.main.WorldToViewportPoint(worldpos)- new Vector3(.5f, .5f, 0);

		GameObject newMarker = GameObject.Instantiate (markerPrefab);
		newMarker.transform.parent = canvas.transform;
		FireFlyMarker marker = newMarker.GetComponent<FireFlyMarker> ();
		float halfCanvasH = canvas.pixelRect.height / canvas.scaleFactor / 2;
		float halfCanvasW = canvas.pixelRect.width / canvas.scaleFactor / 2;

		marker.fromPos = new Vector2(posOnScreen.x*Screen.width/canvas.scaleFactor, posOnScreen.y*Screen.height/canvas.scaleFactor);
		marker.toPos = new Vector2((energy-2)*W, (-halfCanvasH) + 30);

		StartCoroutine (WillAddEnergy ());
	}

	IEnumerator WillAddEnergy() {
		yield return new WaitForSeconds (.6f);

		reducing -= 1;
		energy += 1;
	}

	public IEnumerator DieRoutine() {
		yield return new WaitForSeconds(0.15f);
		Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	public void Die() {
		if (life == 0) {
			StartCoroutine (DieRoutine ());
			Time.timeScale = 0.1f;
		} else {
			life--;
		}

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

		if (reducing > 0) {
			reducing = Mathf.Max (0, reducing - Time.deltaTime / timeTillEnergyGone);
		} else {
			reducing = Mathf.Min (0, reducing + Time.deltaTime / timeTillEnergyGone);
		}

		// Update energy
		energyImage.sizeDelta = new Vector2((energy+reducing) * W, energyImage.sizeDelta.y);


		lifeText.text = "Lives: "+life;

	}
}
