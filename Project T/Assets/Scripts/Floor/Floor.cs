using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public GameObject roomPrefab;

    private int level;
    private Room[,] roomMatrix;
    private int numberOfRooms;

    private void Start() {
        this.level = 0;
        Generate(854353874);
        PrintFloor();
    }

    public void Generate(int seed) {
        Random.InitState(seed);

        this.numberOfRooms = 6 + level + (int) Random.Range(0, Mathf.Sqrt(level));
        this.roomMatrix = new Room[this.numberOfRooms, this.numberOfRooms];

        RoomCoordinate firstRoom = new RoomCoordinate(
            Random.Range(1, this.numberOfRooms - 1), 
            Random.Range(1, this.numberOfRooms - 1)
        );
        AddRoom(firstRoom);

        List<RoomCoordinate> availablePlaces = AvailableNeighbours(firstRoom);

        for(int i = 0; i < this.numberOfRooms - 1; i++) {
            int placeIndex = Random.Range(0, availablePlaces.Count - 1);
            RoomCoordinate newRoom = availablePlaces[placeIndex];
            AddRoom(newRoom);
            availablePlaces.RemoveAt(placeIndex);

            foreach (RoomCoordinate neighbour in AvailableNeighbours(newRoom)) {
                if (!availablePlaces.Exists(room => room.X() == neighbour.X() && room.Y() == neighbour.Y())) {
                    availablePlaces.Add(neighbour);
                }
            }
        }



    }

    private void AddRoom(RoomCoordinate roomCoordinate) {
        Vector2 roomPosition = new Vector2(
            roomCoordinate.X() * roomPrefab.GetComponent<Renderer>().bounds.size.x,
            roomCoordinate.Y() * roomPrefab.GetComponent<Renderer>().bounds.size.y
            );

        GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity) as GameObject;

        this.roomMatrix[roomCoordinate.X(), roomCoordinate.Y()] = room.GetComponent<Room>();
    }

    private List<RoomCoordinate> AvailableNeighbours(RoomCoordinate room) {
        List<RoomCoordinate> availableNeighbours = new List<RoomCoordinate>();

        foreach (RoomCoordinate neighbour in room.Neighbours()) {
            if (IsAvailable(neighbour)) {
                availableNeighbours.Add(neighbour);
            }
        }

        return availableNeighbours;
    }

    private bool IsAvailable(RoomCoordinate room) {
        if(room.X() < 0 ||
           room.X() >= this.numberOfRooms ||
           room.Y() < 0 ||
           room.Y() >= this.numberOfRooms) {
            return false;
        }

        return this.roomMatrix[room.X(), room.Y()] == null;
    }

    private void PrintFloor() {
        string floor = "\n\n";
        for(int y = numberOfRooms - 1; y <= 0; y--) {
            for(int x = 0; x < numberOfRooms; x++) {
                if(roomMatrix[x,y] == null) {
                    floor += "O";
                } else {
                    floor += "G";
                }
            }
            floor += "\n";
        }
        floor += "\n\n";

        Debug.Log(floor);
    }


    private class RoomCoordinate {
        private int x, y;

        public RoomCoordinate (int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int X() {
            return this.x;
        }

        public int Y() {
            return this.y;
        }

        public RoomCoordinate Up() {
            return new RoomCoordinate(this.x, this.y + 1);
        }

        public RoomCoordinate Right() {
            return new RoomCoordinate(this.x + 1, this.y);
        }

        public RoomCoordinate Down() {
            return new RoomCoordinate(this.x, this.y - 1);
        }

        public RoomCoordinate Left() {
            return new RoomCoordinate(this.x - 1, this.y);
        }

        public List<RoomCoordinate> Neighbours() {
            List<RoomCoordinate> neighbours = new List<RoomCoordinate>();

            neighbours.Add(Up());
            neighbours.Add(Right());
            neighbours.Add(Down());
            neighbours.Add(Left());

            return neighbours;
        }
    }
}
