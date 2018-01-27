using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
