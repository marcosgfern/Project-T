using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject room;

    void Start() {
        AdjustOrtographicSize();
        MoveToRoom(room);
    }

    public void MoveToRoom(GameObject room) {
        this.transform.position = new Vector3(room.transform.position.x, room.transform.position.y - 0.45f, this.transform.position.z);
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
