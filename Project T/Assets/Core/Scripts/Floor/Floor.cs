using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Floors {
    /* Class Floor is used as a component for the floor game object.
     * Serves as a node for controlling different actions 
     * related to random generation and player navigation.
     */
    public class Floor : MonoBehaviour {

        [SerializeField] private MinimapRoomLayout minimap;
        [SerializeField] private GameObject floorNumberIndicator;
        [SerializeField] private FloorFade floorFade;

        [SerializeField] private GameObject roomPrefab, fullHeartPrefab, halfHeartPrefab;

        private FloorGenerator generator;
        private Dictionary<Coordinate, Room> roomMap;
        private int level;

        private Coordinate currentRoom;
        private GameObject player;
        private CameraController cameraController;

        private void Awake() {
            //Random.InitState(854353474);
            this.level = 1;
            this.generator = new FloorGenerator(this.gameObject.transform, this.roomPrefab, this.fullHeartPrefab, this.halfHeartPrefab);
            this.player = GameObject.FindGameObjectsWithTag("Player")[0];
            this.cameraController = Camera.main.GetComponent<CameraController>();
        }

        private void Start() {
            floorFade.BlackedOut += GenerateNextFloor;
            GenerateNextFloor();            
        }

        private void MoveToNextFloor() {
            floorFade.StartFloorTransition(this.level);
        }

        private void GenerateNextFloor() {
            foreach(Transform child in this.gameObject.transform) {
                Destroy(child.gameObject);
            }

            this.floorNumberIndicator.GetComponent<TextMeshProUGUI>().SetText("Floor " + this.level);
            this.roomMap = this.generator.Generate(this.level++);
            this.minimap.GenerateTileLayout(this.roomMap);
            MoveToRoom(new Coordinate(0, 0), null);
        }

        /* Moves player from the current room to the adjacent room in @direction. */
        public void MoveToRoom(Direction direction) {
            this.roomMap[this.currentRoom].FadeOut(cameraController.GetPanningDuration());
            MoveToRoom(this.currentRoom.GetNeighbour(direction), direction);
        }

        /* Moves player to room in @coordinate and places it in the center,
         * or in the door in @direction when specified. */
        private void MoveToRoom(Coordinate coordinate, Direction? direction) {
            this.currentRoom = coordinate;
            Room room = this.roomMap[this.currentRoom];
            this.minimap.MoveToRoom(this.currentRoom);
            DiscoverNeighbours();
            cameraController.MoveToRoom(room.gameObject.transform);
            room.FadeIn(cameraController.GetPanningDuration());
            room.MovePlayerIn(this.player, direction.GetOpposite());
        }

        /* Changes neighbours of the current room with RoomState.Unknown to RoomState.Discovered. */
        private void DiscoverNeighbours() {
            foreach(Coordinate coordinate in this.currentRoom.GetNeighbours()) {
                if (this.roomMap.ContainsKey(coordinate)) {
                    this.roomMap[coordinate].SetState(RoomState.Discovered, true);
                }
            }
        }
    }
}

