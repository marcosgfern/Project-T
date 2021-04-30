using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager {
    private static float MinDistanceForSwipe =
        Mathf.Sqrt(
            Mathf.Pow(Screen.height, 2) +
            Mathf.Pow(Screen.width, 2)
        ) * 0.04f;

    private static int minHeight = 360,
                maxHeight = Screen.height - 196;

    private bool isOnControlArea;

    private Vector2 startingPosition;
    private Vector2 currentPosition;
    private bool swipe;
    private TouchPhase? phase;

    public void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(Input.touchCount - 1);

            switch (touch.phase) {
                case TouchPhase.Began:
                    if (TouchIsInControlArea(touch)) {
                        isOnControlArea = true;
                        this.startingPosition = this.currentPosition = touch.position;
                        this.swipe = false;
                        this.phase = touch.phase;
                    } else {
                        isOnControlArea = false;
                    }
                    break;

                case TouchPhase.Moved:
                    if (this.isOnControlArea) {
                        if (Mathf.Abs((touch.position - this.startingPosition).magnitude) > MinDistanceForSwipe
                                && this.phase != TouchPhase.Moved) {
                            this.swipe = true;
                            this.phase = touch.phase;
                        }
                        this.currentPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    if (this.isOnControlArea) {
                        this.currentPosition = touch.position;
                        this.phase = touch.phase;
                    }
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

    private bool TouchIsInControlArea(Touch touch) {
        return touch.position.y > minHeight
            && touch.position.y < maxHeight;
    }
}
