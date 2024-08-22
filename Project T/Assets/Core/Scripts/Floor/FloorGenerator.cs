using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    /* Class FloorGenerator is responsible for the random generation of a floor. */
    public class FloorGenerator {

        private GameObject roomPrefab, fullHeartPrefab, halfHeartPrefab;
        private Transform floor;

        private Dictionary<Coordinate, Room> roomMap;
        private int numberOfRooms;
        private int level;

        private int minEnemiesPerRoom = 2;
        private int maxEnemiesPerRoom = 5;
        private float emptyRoomRatio = 0.80f;
        private float meleeShooterBaseRatio = 0.66f;
        private float meleeShooterRatioVariation = 0.14f;
        private float specialColorBaseRatio = 0.55f;
        private float specialColorRatioVariation = 0.35f;
        private float redBlueRatio = 0.6f;
        private float fullHalfRatio = 0.5f;

        public FloorGenerator(Transform floor, GameObject roomPrefab, GameObject fullHeartPrefab, GameObject halfHeartPrefab) {
            this.floor = floor;
            this.roomPrefab = roomPrefab;
            this.fullHeartPrefab = fullHeartPrefab;
            this.halfHeartPrefab = halfHeartPrefab;
            this.roomMap = new Dictionary<Coordinate, Room>();
        }

        /* Generates floor of given level.
         * Returns dictionary of resulting rooms.
         */
        public Dictionary<Coordinate, Room> Generate(int level) {
            this.roomMap.Clear();

            this.level = level;
            this.numberOfRooms = 6 + (int)(this.level * 1.5) + (int)Random.Range(0f, Mathf.Sqrt(this.level));

            GenerateRooms();

            PlaceDoors();

            GenerateEnemies();

            return this.roomMap;
        }

        /* Places rooms in a pseudo-random way, so every room is accessible. */
        private void GenerateRooms() {
            Coordinate firstRoom = new Coordinate(0, 0);
            AddRoom(firstRoom);

            List<Coordinate> availablePlaces = GetAvailableNeighbours(firstRoom);

            for (int i = 0; i < this.numberOfRooms - 1; i++) {
                int placeIndex = Random.Range(0, availablePlaces.Count - 1);
                Coordinate newRoom = availablePlaces[placeIndex];
                AddRoom(newRoom);
                availablePlaces.RemoveAt(placeIndex);

                foreach (Coordinate neighbour in GetAvailableNeighbours(newRoom)) {
                    if (!availablePlaces.Contains(neighbour)) {
                        availablePlaces.Add(neighbour);
                    }
                }

                availablePlaces.RemoveAll(place => !IsUsable(place));
            }
        }

        /* Instantiates room in scene and adds its Room component to dictionary. */
        private void AddRoom(Coordinate roomCoordinate) {
            Vector2 roomPosition = new Vector2(
                roomCoordinate.x * roomPrefab.GetComponent<Renderer>().bounds.size.x,
                roomCoordinate.y * roomPrefab.GetComponent<Renderer>().bounds.size.y);

            GameObject room = Object.Instantiate(roomPrefab, roomPosition, Quaternion.identity) as GameObject;
            room.name = roomCoordinate.ToString();
            room.transform.parent = this.floor;

            this.roomMap.Add(roomCoordinate, room.GetComponent<Room>());
        }

        /* Returns empty coordinates adjacent to @roomCoordinate. */
        private List<Coordinate> GetAvailableNeighbours(Coordinate roomCoordinate) {
            List<Coordinate> availableNeighbours = new List<Coordinate>();

            foreach (Coordinate neighbour in roomCoordinate.GetNeighbours()) {
                if (IsAvailable(neighbour)) {
                    availableNeighbours.Add(neighbour);
                }
            }

            return availableNeighbours;
        }

        /* Returns if a coordinate is usable.
         * Coordinate is considered usable when it has one and only one non empty adjacent coordinate.
         */
        private bool IsUsable(Coordinate roomCoordinate) {
            int numberOfNeighbours = 0;

            foreach (Coordinate neighbour in roomCoordinate.GetNeighbours()) {
                if (this.roomMap.ContainsKey(neighbour)) {
                    numberOfNeighbours++;
                }
            }

            return numberOfNeighbours == 1;
        }

        private bool IsAvailable(Coordinate roomCoordinate) {
            return !this.roomMap.ContainsKey(roomCoordinate);
        }

        /* Places doors in every room, in every direction where the room has a neighbour.
         * Places stairs in one of the rooms farthest to the starting room.
         */
        private void PlaceDoors() {
            Coordinate farthestCoordinate = null;
            int maxDistance = 0;
            foreach (KeyValuePair<Coordinate, Room> room in this.roomMap) {
                if (room.Key.GetDistanceToOrigin() >= maxDistance) {
                    farthestCoordinate = room.Key;
                    maxDistance = room.Key.GetDistanceToOrigin();
                }

                foreach (Direction direction in System.Enum.GetValues(typeof(Direction))) {
                    if (this.roomMap.ContainsKey(room.Key.GetNeighbour(direction))) {
                        room.Value.SetDoor(direction);
                    }
                }
            }

            this.roomMap[farthestCoordinate].SetStairs();
        }

        /* Sets an enemy list to every room. */
        private void GenerateEnemies() {
            foreach (KeyValuePair<Coordinate, Room> room in this.roomMap) {
                if (!room.Key.IsOrigin()) {
                    List<EnemyTemplate> enemies = GenerateEnemyList();
                    room.Value.SetEnemies(enemies);

                    if (enemies.Count == 0) {
                        GenerateHeart(room.Value);
                    }
                }
            }
        }

        /* Generates a pseudo-random enemy list for a room.*/ 
        private List<EnemyTemplate> GenerateEnemyList() {
            List<EnemyTemplate> enemies = new List<EnemyTemplate>();

            int numberOfEnemies;

            if(Random.Range(0f, 1f) > this.emptyRoomRatio) {
                numberOfEnemies = 0;
            } else {
                numberOfEnemies = Random.Range(this.minEnemiesPerRoom, this.maxEnemiesPerRoom);
            }

            for(int i = 0; i < numberOfEnemies; i++) {
                EnemyTemplate enemy = new EnemyTemplate();

                if (Random.Range(0f, 1f) > this.meleeShooterBaseRatio + this.meleeShooterRatioVariation / this.level) {
                    enemy.kind = EnemyKind.Shooter;
                    
                } else {
                    enemy.kind = EnemyKind.Melee;
                }

                enemy.health = 1 + this.level/2 + (int) Random.Range(0f, Mathf.Sqrt(this.level/2));

                if (Random.Range(0f, 1f) > this.specialColorBaseRatio + this.specialColorRatioVariation / this.level) {
                    if (Random.Range(0f, 1f) > this.redBlueRatio) {
                        enemy.color = EnemyHealth.DamageColor.Red;
                    } else {
                        enemy.color = EnemyHealth.DamageColor.Blue;
                    }
                } else {
                    enemy.color = EnemyHealth.DamageColor.White;
                }

                enemy.damage = this.level > 6 ? 2 : 1;

                enemies.Add(enemy);
            }

            return enemies;
        }

        private void GenerateHeart(Room room) {
            GameObject prefab;
            if (Random.Range(0f, 1f) > this.fullHalfRatio) {
                prefab = this.fullHeartPrefab;
            } else {
                prefab = this.halfHeartPrefab;
            }

            Object.Instantiate(prefab, room.transform);
        }
    }
}
