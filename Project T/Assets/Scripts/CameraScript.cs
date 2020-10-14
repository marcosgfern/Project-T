using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public SpriteRenderer background;

    // Start is called before the first frame update
    void Start() {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = background.bounds.size.x / background.bounds.size.y;

        if (screenRatio >= targetRatio) {
            Camera.main.orthographicSize = background.bounds.size.y / 2;
        } else {
            Camera.main.orthographicSize = background.bounds.size.y / 2 * (targetRatio / screenRatio);
        }
    }
}
