using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportScenes : MonoBehaviour
{

    public Transform lastEndpoint;
	private float lastEndpointY;

	public Transform player;

	public float lookAhead;

	public List<string> availablePieces;

	public bool loading = false;

	private List<GameObject> scenes;

	void Awake() {
		scenes = new List<GameObject> ();
	}

	void AddPiece() {
		if (!loading) {
			loading = true;
			int piece = Random.Range (0, availablePieces.Count - 1);
			SceneManager.LoadScene (availablePieces [piece], LoadSceneMode.Additive);
		}
	}

	void CheckSceneLoaded() {
		GameObject level = GameObject.FindGameObjectWithTag ("Level");
		if (level != null) {
			GameObject enter = GameObject.FindGameObjectWithTag ("Enter");
			GameObject exit = GameObject.FindGameObjectWithTag ("Exit");

			enter.tag = "Processed";
			exit.tag = "Processed";
			level.tag = "Processed";
			Transform nextStartPoint = enter.transform;
			Vector3 shift = lastEndpoint.position - nextStartPoint.position;
			level.transform.Translate (shift);
			lastEndpoint = exit.transform;

			scenes.Add (level);
			if(scenes.Count>4) {
				GameObject.Destroy (scenes [0]);
				scenes.RemoveAt(0);
			}
			loading = false;
		}
	}

	void Update () {
		CheckSceneLoaded ();
		if (player.position.z + lookAhead > lastEndpoint.position.z) {
			AddPiece ();
		}
	}
}
