using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class TouchManager is used to receive raw tactile input and interpret it as necessary. */
public class TouchManager {
    private static float MinDistanceForSwipe =
        Mathf.Sqrt(
            Mathf.Pow(Screen.height, 2) +
            Mathf.Pow(Screen.width, 2)
        ) * 0.025f; // Used to prevent small swipes made by mistake to register.

    private static int MinHeight = 360,
                MaxHeight = Screen.height - 196; // Used so the touches and swipes made on the UI don't register as player controls.

    private bool isOnControlArea;

    private Vector2 startingPosition;
    private Vector2 currentPosition;
    private bool isSwipe;
    private TouchPhase? phase;

    /* Interprets touch state. Called by PlayerController in every frame update. */
    public void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(Input.touchCount - 1);

            switch (touch.phase) {
                case TouchPhase.Began:
                    if (IsTouchInControlArea(touch)) {
                        isOnControlArea = true;
                        this.startingPosition = this.currentPosition = touch.position;
                        this.isSwipe = false;
                        this.phase = touch.phase;
                    } else {
                        isOnControlArea = false;
                    }
                    break;

                case TouchPhase.Moved:
                    if (this.isOnControlArea) {
                        if (Mathf.Abs((touch.position - this.startingPosition).magnitude) > MinDistanceForSwipe
                                && this.phase != TouchPhase.Moved) {
                            this.isSwipe = true;
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
        return this.isSwipe;
    }

    public TouchPhase? GetPhase() {
        return this.phase;
    }

    private bool IsTouchInControlArea(Touch touch) {
        return touch.position.y > MinHeight
            && touch.position.y < MaxHeight;
    }
}
