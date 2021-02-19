using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public RectTransform upperUI, lowerUI, canvas;
    private float shift;
    private float panningDuration = 0.2f;

    void Start() {
        AdjustOrtographicSize();

        CalculateCameraYShift();
    }

    public float GetPanningDuration() {
        return this.panningDuration;
    }

    public void CalculateCameraYShift() {
       float screenShift = (lowerUI.rect.height - upperUI.rect.height) / 2;
        this.shift = (Camera.main.ScreenToWorldPoint(new Vector3(0, screenShift, 0)) - Camera.main.ScreenToWorldPoint(Vector3.zero)).y;
    }

    private void AdjustOrtographicSize() {
        SpriteRenderer aspectRatioModel = GetComponentInChildren<SpriteRenderer>();

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = aspectRatioModel.bounds.size.x / aspectRatioModel.bounds.size.y;

        if (screenRatio >= targetRatio) {
            Camera.main.orthographicSize = aspectRatioModel.bounds.size.y / 2;
        } else {
            Camera.main.orthographicSize = aspectRatioModel.bounds.size.y / 2 * (targetRatio / screenRatio);
        }
    }

    public void MoveToRoom(Transform room) {
        StartCoroutine(Panning(new Vector3(room.position.x, room.position.y - this.shift, this.transform.position.z), this.panningDuration));
    }

    private IEnumerator Panning(Vector3 targetPosition, float duration) {
        Vector3 startingPosition = this.transform.position;
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            this.transform.position = Vector3.Lerp(startingPosition, targetPosition, t/duration);
            yield return Time.deltaTime;
        }
        this.transform.position = targetPosition;
    }
}
