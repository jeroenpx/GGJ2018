using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImportScenes : MonoBehaviour
{

    public Transform Endpoint;

	void Start ()
    {
		SceneManager.LoadScene("Tunnelpiece1", LoadSceneMode.Additive);
		SceneManager.LoadScene("Tunnelpiece1", LoadSceneMode.Additive);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
