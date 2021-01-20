using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public GameObject doorPrefab;

    private Room upRoom, rightRoom, downRoom, leftRoom;
    private Door upDoor, rightDoor, downDoor, leftDoor;
    private bool completed;

    public void SetNeighbourUp(Room neighbour) {

    }

    private void OpenDoors() {
        if (upDoor != null) upDoor.Open();
        if (rightDoor != null) rightDoor.Open();
        if (downDoor != null) downDoor.Open();
        if (leftDoor != null) leftDoor.Open();
    }

    private void CloseDoors() {
        if (upDoor != null) upDoor.Close();
        if (rightDoor != null) rightDoor.Close();
        if (downDoor != null) downDoor.Close();
        if (leftDoor != null) leftDoor.Close();
    }
}
