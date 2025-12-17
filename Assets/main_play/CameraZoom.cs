using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float targetZoom = 2f;
    [SerializeField] private float moveSpeed = 3f;

    public void StartZoom(Vector3 targetPosition, Action onComplete)
    {
        StartCoroutine(ZoomToTarget(targetPosition, onComplete));
    }

    private IEnumerator ZoomToTarget(Vector3 targetPosition, Action onComplete)
    {
        float startZoom = cam.orthographicSize;
        Vector3 startPos = cam.transform.position;

        float t = 0f;

        while (t < 0.5f)
        {
            t += Time.deltaTime * zoomSpeed;

            cam.orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);
            cam.transform.position = Vector3.Lerp(startPos, new Vector3(targetPosition.x, targetPosition.y, startPos.z), t * moveSpeed);

            yield return null;
        }

        // ????? ???????? ????????, ??? ????????
        onComplete?.Invoke();
    }
}
