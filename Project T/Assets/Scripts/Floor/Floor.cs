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
    }
}

