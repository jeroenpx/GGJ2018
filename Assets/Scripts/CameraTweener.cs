using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTweener : MonoBehaviour {

    public Transform tweenableCamera;
    public List<Transform> positions;
    public float smoothing = 7f;
    private bool _goToCredits = false;
    private bool _goToMainMenu = false;


    public void SetPosition(int positionID)
    {
        StopCoroutine("MoveToPosition");
        StartCoroutine("MoveToPosition", positionID);
    }

    private IEnumerator MoveToPosition(int positionID)
    {
        Vector3 targetPosition = positions[positionID].position;
        Quaternion targetRotation = positions[positionID].rotation;

        while (Vector3.Distance(tweenableCamera.transform.position, targetPosition) > 0.05f)
        {
            tweenableCamera.transform.position = Vector3.Lerp(tweenableCamera.transform.position, targetPosition,
                smoothing * Time.deltaTime);
            tweenableCamera.transform.rotation = Quaternion.Lerp(tweenableCamera.transform.rotation, targetRotation, smoothing * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    public void GoToCredits()
    {
        SetPosition(1);
    }

    public void GoToMainMenu()
    {
        SetPosition(0);
    }

    public void GoToCave()
    {
        SetPosition(2);
        StartCoroutine(LoadGame());
    }

    public void GoToCaveTutorial()
    {
        SetPosition(2);
        StartCoroutine(LoadTutorial());
    }


    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }
    private IEnumerator LoadTutorial()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }
}
