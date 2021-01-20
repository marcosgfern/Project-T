using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public RectTransform upperUI, lowerUI, canvas;
    private float shift;

    void Start() {
        AdjustOrtographicSize();

        CalculateCameraYShift();
    }

    public void CalculateCameraYShift() {
        float screenShift = (lowerUI.rect.height - upperUI.rect.height) / 2 * canvas.localScale.y;
        this.shift = (Camera.main.ScreenToWorldPoint(new Vector3(0, screenShift, 0)) - Camera.main.ScreenToWorldPoint(Vector3.zero)).y;
    }

    public void MoveToRoom(GameObject room) {
        this.transform.position = new Vector3(room.transform.position.x, room.transform.position.y - this.shift, this.transform.position.z);
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
}
