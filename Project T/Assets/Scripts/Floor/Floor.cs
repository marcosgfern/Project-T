using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Floor : MonoBehaviour {

        public GameObject roomPrefab;

        private FloorGenerator generator;
        private Dictionary<Coordinate, Room> roomMap;
        private int level;

        private Coordinate currentRoom;
        private GameObject player;
        private CameraController cameraController;

        private void Awake() {
            Random.InitState(854353474);
            this.level = 0;
            this.generator = new FloorGenerator(this.roomPrefab, this.gameObject.transform);
            this.player = GameObject.FindGameObjectsWithTag("Player")[0];
            this.cameraController = Camera.main.GetComponent<CameraController>();
        }

        private void Start() {
            GenerateNextFloor();            
        }

        private void GenerateNextFloor() {
            foreach(Transform child in this.gameObject.transform) {
                Destroy(child.gameObject);
            }

            this.roomMap = this.generator.Generate(this.level++);
            MoveToRoom(new Coordinate(0, 0));
        }

        public void MoveToRoom(Direction direction) {
            this.roomMap[this.currentRoom].FadeOut(cameraController.GetPanningDuration());
            this.currentRoom = this.currentRoom.GetNeighbour(direction);
            Room room = this.roomMap[this.currentRoom];
            cameraController.MoveToRoom(room.gameObject.transform);
            room.FadeIn(cameraController.GetPanningDuration());
            room.MovePlayerIn(this.player, direction.Opposite());
        }

        private void MoveToRoom(Coordinate coordinate) {
            this.currentRoom = coordinate;
            Room room = this.roomMap[coordinate];
            cameraController.MoveToRoom(room.gameObject.transform);
            room.FadeIn(cameraController.GetPanningDuration());
            room.MovePlayerIn(this.player, null);
        }

        private class FloorGenerator {

            private GameObject roomPrefab;
            private Transform floor;

            private Dictionary<Coordinate, Room> roomMap;
            private int numberOfRooms;
            private int level;

            public FloorGenerator(GameObject roomPrefab, Transform floor) {
                this.roomPrefab = roomPrefab;
                this.floor = floor;
                this.roomMap = new Dictionary<Coordinate, Room>();
            }


            public Dictionary<Coordinate, Room> Generate(int level) {
                roomMap.Clear();

                this.level = level;
                this.numberOfRooms = 6 + (int)(this.level * 1.5) + (int)Random.Range(0, Mathf.Sqrt(this.level));

                GenerateRooms();

                PlaceDoors();

                return this.roomMap;
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

            private void AddRoom(Coordinate roomCoordinate) {
                Vector2 roomPosition = new Vector2(
                    roomCoordinate.x * roomPrefab.GetComponent<Renderer>().bounds.size.x,
                    roomCoordinate.y * roomPrefab.GetComponent<Renderer>().bounds.size.y
                    );

                GameObject room = Instantiate(roomPrefab, roomPosition, Quaternion.identity) as GameObject;
                room.name = roomCoordinate.ToString();
                room.transform.parent = this.floor;

                this.roomMap.Add(roomCoordinate, room.GetComponent<Room>());
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

            private void PlaceDoors() {
                Coordinate farthestCoordinate = null;
                int maxDistance = 0;
                foreach (KeyValuePair<Coordinate, Room> room in roomMap) {
                    if(room.Key.DistanceToOrigin() >= maxDistance) {
                        farthestCoordinate = room.Key;
                        maxDistance = room.Key.DistanceToOrigin();
                    }

                    foreach (Direction direction in System.Enum.GetValues(typeof(Direction))) {
                        if (roomMap.ContainsKey(room.Key.GetNeighbour(direction))) {
                            room.Value.SetDoor(direction);
                        }
                    }
                }

                roomMap[farthestCoordinate].SetStairs();
            }
        }
    }
}

