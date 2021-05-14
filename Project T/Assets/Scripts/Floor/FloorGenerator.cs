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

        private int maxEnemiesPerRoom = 4;
        private float meleeShooterRatio = 0.8f;
        private float specialColorRatio = 0.9f;
        private float redBlueRatio = 0.6f;

        public FloorGenerator(GameObject roomPrefab, Transform floor) {
            this.roomPrefab = roomPrefab;
            this.floor = floor;
            this.roomMap = new Dictionary<Coordinate, Room>();
        }


        public Dictionary<Coordinate, Room> Generate(int level) {
            this.roomMap.Clear();

            this.level = level;
            this.numberOfRooms = 6 + (int)(this.level * 1.5) + (int)Random.Range(0f, Mathf.Sqrt(this.level));

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
                roomCoordinate.y * roomPrefab.GetComponent<Renderer>().bounds.size.y);

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
                List<EnemyTemplate> enemies = GenerateEnemyList();
                room.Value.SetEnemies(enemies);
            }

            this.roomMap[new Coordinate(0, 0)].SetEnemies(new List<EnemyTemplate>());
        }

        private List<EnemyTemplate> GenerateEnemyList() {
            List<EnemyTemplate> enemies = new List<EnemyTemplate>();

            /*int numberOfEnemies = maxEnemiesPerRoom - (int) Mathf.Log(
                Random.Range(
                    0f, 
                    Mathf.Pow(2, maxEnemiesPerRoom) + 1),
                2);

            if(numberOfEnemies > maxEnemiesPerRoom) {
                numberOfEnemies = maxEnemiesPerRoom;
            }*/

            int numberOfEnemies = Random.Range(0, maxEnemiesPerRoom + 1);
            if (numberOfEnemies == 1) {
                numberOfEnemies = 2;
            }

            for(int i = 0; i < numberOfEnemies; i++) {
                EnemyTemplate enemy = new EnemyTemplate();

                if(Random.Range(0f, 1f) > this.meleeShooterRatio) {
                    enemy.kind = EnemyKind.Shooter;
                    
                } else {
                    enemy.kind = EnemyKind.Melee;
                }

                enemy.health = 1 + this.level + (int) Random.Range(0f, Mathf.Sqrt(this.level));

                if (Random.Range(0f, 1f) > this.specialColorRatio) {
                    if (Random.Range(0f, 1f) > this.redBlueRatio) {
                        enemy.color = EnemyHealth.DamageColor.Red;
                    } else {
                        enemy.color = EnemyHealth.DamageColor.Blue;
                    }
                } else {
                    enemy.color = EnemyHealth.DamageColor.White;
                }

                enemy.damage = 1 + (int)(this.level / 3);

                enemies.Add(enemy);
            }

            return enemies;
        }
    }
}
