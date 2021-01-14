using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public GameObject roomPrefab;
    private FloorGenerator generator;

    private void Start() {
        this.generator = new FloorGenerator(2, this.roomPrefab, this.gameObject.transform);
        this.generator.Generate(854353174);
    }

    private class FloorGenerator {

        private GameObject roomPrefab;
        private Transform floor; 

        private int level;
        private Room[,] roomMatrix;
        private int matrixSize;
        private int numberOfRooms;

        public FloorGenerator (int level, GameObject roomPrefab, Transform floor) {
            this.level = level;
            this.roomPrefab = roomPrefab;
            this.floor = floor;
        }


        public void Generate(int seed) {
            Random.InitState(seed);

            this.numberOfRooms = 6 + (int)(level * 1.5) + (int)Random.Range(0, Mathf.Sqrt(level));

            if (this.numberOfRooms <= 10) {
                this.matrixSize = this.numberOfRooms;
            } else {
                this.matrixSize = (int)Mathf.Sqrt(this.numberOfRooms * Mathf.Sqrt(this.numberOfRooms));
            }
            this.roomMatrix = new Room[this.matrixSize, this.matrixSize];

            RoomCoordinate firstRoom = new RoomCoordinate(
                Random.Range(1, this.matrixSize - 1),
                Random.Range(1, this.matrixSize - 1)
            );
            AddRoom(firstRoom);

            List<RoomCoordinate> availablePlaces = AvailableNeighbours(firstRoom);

            for (int i = 0; i < this.numberOfRooms - 1; i++) {
                int placeIndex = Random.Range(0, availablePlaces.Count - 1);
                RoomCoordinate newRoom = availablePlaces[placeIndex];
                AddRoom(newRoom);
                availablePlaces.RemoveAt(placeIndex);

                foreach (RoomCoordinate neighbour in AvailableNeighbours(newRoom)) {
                    if (!availablePlaces.Exists(room => room.X() == neighbour.X() && room.Y() == neighbour.Y())) {
                        availablePlaces.Add(neighbour);
                    }
                }

                availablePlaces.RemoveAll(place => !IsUsable(place));
            }



        }

        private void AddRoom(RoomCoordinate roomCoordinate) {
            Vector2 roomPosition = new Vector2(
                roomCoordinate.X() * roomPrefab.GetComponent<Renderer>().bounds.size.x,
                roomCoordinate.Y() * roomPrefab.GetComponent<Renderer>().bounds.size.y
                );

            GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity) as GameObject;
            room.name = roomCoordinate.ToString();
            room.transform.parent = this.floor;

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

        private bool IsUsable(RoomCoordinate room) {
            int numberOfNeighbours = 0;

            foreach (RoomCoordinate neighbour in room.Neighbours()) {
                if (!neighbour.IsOutOfBounds(this.roomMatrix.GetLength(0))) {
                    if (this.roomMatrix[neighbour.X(), neighbour.Y()] != null) {
                        numberOfNeighbours++;
                    }
                }
            }

            return numberOfNeighbours == 1;
        }

        private bool IsAvailable(RoomCoordinate room) {
            if (room.IsOutOfBounds(this.roomMatrix.GetLength(0))) {
                return false;
            } else {
                return this.roomMatrix[room.X(), room.Y()] == null;
            }
        }


        private class RoomCoordinate {
            private int x, y;

            public RoomCoordinate(int x, int y) {
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

            public bool IsOutOfBounds(int bounds) {
                return (this.x < 0 ||
                    this.x >= bounds ||
                    this.y < 0 ||
                    this.y >= bounds);
            }

            override public string ToString() {
                return "[" + this.x + "," + this.y + "]";
            }
        }
    }

    
}
