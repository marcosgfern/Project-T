using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class FloorGenerator {

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
            this.roomMap.Clear();

            this.level = level;
            this.numberOfRooms = 6 + (int)(this.level * 1.5) + (int)Random.Range(0, Mathf.Sqrt(this.level));

            GenerateRooms();

            PlaceDoors();

            GenerateEnemies();

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

            GameObject room = Object.Instantiate(roomPrefab, roomPosition, Quaternion.identity) as GameObject;
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
            foreach (KeyValuePair<Coordinate, Room> room in this.roomMap) {
                if (room.Key.DistanceToOrigin() >= maxDistance) {
                    farthestCoordinate = room.Key;
                    maxDistance = room.Key.DistanceToOrigin();
                }

                foreach (Direction direction in System.Enum.GetValues(typeof(Direction))) {
                    if (this.roomMap.ContainsKey(room.Key.GetNeighbour(direction))) {
                        room.Value.SetDoor(direction);
                    }
                }
            }

            this.roomMap[farthestCoordinate].SetStairs();
        }

        private void GenerateEnemies() {
            foreach (KeyValuePair<Coordinate, Room> room in this.roomMap) {
                room.Value.SetEnemies(GenerateEnemyList());
            }

            this.roomMap[new Coordinate(0, 0)].SetEnemies(new List<EnemyTemplate>());
        }

        private List<EnemyTemplate> GenerateEnemyList() {
            List<EnemyTemplate> enemies = new List<EnemyTemplate>();
            enemies.Add(new EnemyTemplate(EnemyKind.Melee, 1, EnemyHealth.DamageColor.White, 1));
            enemies.Add(new EnemyTemplate(EnemyKind.Shooter, 2, EnemyHealth.DamageColor.White, 1));
            return enemies;
        }
    }
}
