using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager {
    private static float MinDistanceForSwipe =
        Mathf.Sqrt(
            Mathf.Pow(Screen.height, 2) +
            Mathf.Pow(Screen.width, 2)
        ) * 0.04f;

    private Vector2 startingPosition;
    private Vector2 currentPosition;
    private bool swipe;
    private TouchPhase? phase;

    public void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(Input.touchCount - 1);

            switch (touch.phase) {
                case TouchPhase.Began:
                    this.startingPosition = this.currentPosition = touch.position;
                    this.swipe = false;
                    this.phase = touch.phase;
                    break;

                case TouchPhase.Moved:
                    if(this.phase != TouchPhase.Moved
                            && Mathf.Abs((touch.position - this.startingPosition).magnitude) > MinDistanceForSwipe) {
                        this.swipe = true;
                        this.phase = touch.phase;
                    }
                    this.currentPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    this.currentPosition = touch.position;
                    this.phase = touch.phase;
                    break;
            }
        } else {
            phase = null;
        }
    }

    public Vector2 GetStartingPosition() {
        return this.startingPosition;
    }

    public Vector2 GetSwipeDirection() {
        return (this.currentPosition - this.startingPosition).normalized;
    }

    public bool IsSwipe() {
        return this.swipe;
    }

    public TouchPhase? GetPhase() {
        return this.phase;
    }
}
