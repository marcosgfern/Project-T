using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Floors;

/* Used as a component for minimap tiles. */
public class MinimapRoomTile : MonoBehaviour {

    public Sprite[] stateSprites;

    private Image image;

    private void Awake() {
        this.image = GetComponent<Image>();
        this.SetState(RoomState.Unknown);
    }

    /* Changes tile sprite depending on @state. */
    public void SetState(RoomState state) {
        this.image.sprite = stateSprites[(int)state];
    }
}
