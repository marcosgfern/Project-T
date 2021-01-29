using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public GameObject roomPrefab;
    private FloorGenerator generator;
    private Dictionary<Coordinate, Room> roomMap;

    private void Start() {
        this.generator = new FloorGenerator(10, this.roomPrefab, this.gameObject.transform);
        this.roomMap = this.generator.Generate(854353174);
    }

    private class FloorGenerator {

        private GameObject roomPrefab;
        private Transform floor; 

        private int level;
        private Dictionary<Coordinate, Room> roomMap;
        private int numberOfRooms;

        public FloorGenerator (int level, GameObject roomPrefab, Transform floor) {
            this.level = level;
            this.roomPrefab = roomPrefab;
            this.floor = floor;
            this.roomMap = new Dictionary<Coordinate, Room>();
        }


        public Dictionary<Coordinate, Room> Generate(int seed) {
            roomMap.Clear();

            Random.InitState(seed);

            this.numberOfRooms = 6 + (int)(level * 1.5) + (int)Random.Range(0, Mathf.Sqrt(level));

            GenerateRooms();

            return this.roomMap;
        }

        private void AddRoom(Coordinate roomCoordinate) {
            Vector2 roomPosition = new Vector2(
                roomCoordinate.x * roomPrefab.GetComponent<Renderer>().bounds.size.x,
                roomCoordinate.y * roomPrefab.GetComponent<Renderer>().bounds.size.y
                );

            GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity) as GameObject;
            room.name = roomCoordinate.ToString();
            room.transform.parent = this.floor;

            Room roomScript = room.GetComponent<Room>();

            //Set neighbours

            this.roomMap.Add(roomCoordinate, roomScript);
        }

        private void GenerateRooms() {
            Coordinate firstRoom = new Coordinate(0, 0);
            AddRoom(firstRoom);

            List<Coordinate> availablePlaces = AvailableNeighbours(firstRoom);

            for (int i = 0; i < this.numberOfRooms - 1; i++) {
                int placeIndex = Random.Range(0, availablePlaces.Count - 1);
                Coordinate newRoom = availablePlaces[placeIndex];
                AddRoom(newRoom);
                availablePlaces.RemoveAt(placeIndex);

                foreach (Coordinate neighbour in AvailableNeighbours(newRoom)) {
                    if (!availablePlaces.Contains(neighbour)) {
                        availablePlaces.Add(neighbour);
                    }
                }

                availablePlaces.RemoveAll(place => !IsUsable(place));
            }
        }

        private List<Coordinate> AvailableNeighbours(Coordinate room) {
            List<Coordinate> availableNeighbours = new List<Coordinate>();

            foreach (Coordinate neighbour in room.GetNeighbours()) {
                if (IsAvailable(neighbour)) {
                    availableNeighbours.Add(neighbour);
                }
            }

            return availableNeighbours;
        }

        private bool IsUsable(Coordinate room) {
            int numberOfNeighbours = 0;

            foreach (Coordinate neighbour in room.GetNeighbours()) {
                if (this.roomMap.ContainsKey(neighbour)) {
                    numberOfNeighbours++;
                }
            }

            return numberOfNeighbours == 1;
        }

        private bool IsAvailable(Coordinate room) {
                return !this.roomMap.ContainsKey(room);
        }
    }

    private class Coordinate {
        public int x, y;

        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
        }

        //Change list to array
        public List<Coordinate> GetNeighbours() {
            List<Coordinate> neighbours = new List<Coordinate>();

            neighbours.Add(new Coordinate(this.x, this.y + 1));
            neighbours.Add(new Coordinate(this.x + 1, this.y));
            neighbours.Add(new Coordinate(this.x, this.y - 1));
            neighbours.Add(new Coordinate(this.x - 1, this.y));

            return neighbours;
        }

        //To remove
        public bool IsOutOfBounds(int bounds) {
            return (this.x < 0 ||
                this.x >= bounds ||
                this.y < 0 ||
                this.y >= bounds);
        }

        override public string ToString() {
            return "[" + this.x + "," + this.y + "]";
        }

        public override bool Equals(object obj) {
            if(obj == null) {
                return false;
            }

            if(!(obj is Coordinate)) {
                return false;
            }

            return this.x == ((Coordinate)obj).x 
                && this.y == ((Coordinate)obj).y;
        }

        public override int GetHashCode() {
            return (this.x + "," + this.y).GetHashCode();
        }
    }

    
}
