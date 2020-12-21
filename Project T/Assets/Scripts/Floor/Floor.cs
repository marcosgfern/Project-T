using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public GameObject roomPrefab;

    private int level;
    private Room[,] roomMatrix;
    private int numberOfRooms;

    private void Start() {

    }

    public void Generate(int seed) {
        this.numberOfRooms = 6 + level;
        this.roomMatrix = new Room[this.numberOfRooms, this.numberOfRooms];

        Random.InitState(seed);

        int firstRoomX = Random.Range(1, this.numberOfRooms - 1);
        int firstRoomY = Random.Range(1, this.numberOfRooms - 1);

        PlaceRooms(firstRoomX, firstRoomY, this.numberOfRooms);


    }

    private void PlaceRooms(int roomXCoord, int roomYCoord, int roomsLeft) {
        if (roomsLeft == 0) {
            return;
        } else if (roomsLeft == 1) {
            roomMatrix[roomXCoord, roomYCoord] = new Room();
            return;
        }

        roomMatrix[roomXCoord, roomYCoord] = new Room();
        roomsLeft--;






    }
}
