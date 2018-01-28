using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportScenes : MonoBehaviour
{

    public Transform lastEndpoint;

	public Transform player;

	public float lookAhead;

	public List<string> availablePieces;

	public bool loading = false;

	void Awake() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void AddPiece() {
		if (!loading) {
			loading = true;
			Debug.Log ("Loading level!!!");
			int piece = Random.Range (0, availablePieces.Count - 1);
			SceneManager.LoadScene (availablePieces [piece], LoadSceneMode.Additive);
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (mode == LoadSceneMode.Additive) {
			GameObject enter = GameObject.FindGameObjectWithTag ("Enter");
			GameObject exit = GameObject.FindGameObjectWithTag ("Exit");
			GameObject level = GameObject.FindGameObjectWithTag ("Level");
			enter.tag = "Processed";
			exit.tag = "Processed";
			level.tag = "Processed";
			Transform nextStartPoint = enter.transform;
			Vector3 shift = lastEndpoint.position - nextStartPoint.position;
			level.transform.Translate (shift);
			lastEndpoint = exit.transform;
			loading = false;
		}
	}

    void Update () {
		if (player.position.z + lookAhead > lastEndpoint.position.z) {
			AddPiece ();
		}
	}
}
