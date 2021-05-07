using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Floors {
    public class Floor : MonoBehaviour {

        public GameObject roomPrefab;

        public MinimapRoomLayout minimap;

        public GameObject floorNumberIndicator;

        private FloorGenerator generator;
        private Dictionary<Coordinate, Room> roomMap;
        private int level;

        private Coordinate currentRoom;
        private GameObject player;
        private CameraController cameraController;

        private void Awake() {
            //Random.InitState(854353474);
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
            this.minimap.GenerateTileLayout(this.roomMap);
            this.floorNumberIndicator.GetComponent<TextMeshProUGUI>().SetText("Floor " + this.level);
            MoveToRoom(new Coordinate(0, 0), null);
        }

        public void MoveToRoom(Direction direction) {
            this.roomMap[this.currentRoom].FadeOut(cameraController.GetPanningDuration());
            MoveToRoom(this.currentRoom.GetNeighbour(direction), direction);
        }

        private void MoveToRoom(Coordinate coordinate, Direction? direction) {
            this.currentRoom = coordinate;
            Room room = this.roomMap[this.currentRoom];
            this.minimap.MoveToRoom(this.currentRoom);
            DiscoverNeighbours();
            cameraController.MoveToRoom(room.gameObject.transform);
            room.FadeIn(cameraController.GetPanningDuration());
            room.MovePlayerIn(this.player, direction.Opposite());
        }

        private void DiscoverNeighbours() {
            foreach(Coordinate coordinate in this.currentRoom.GetNeighbours()) {
                if (this.roomMap.ContainsKey(coordinate)) {
                    this.roomMap[coordinate].SetState(RoomState.Discovered, true);
                }
            }
        }
    }
}

