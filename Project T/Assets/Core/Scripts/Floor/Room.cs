﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Room : MonoBehaviour {

        public static EnemyPool enemyPool;

        public float 
            spawnPointX = 1f, 
            spawnPointY = 1.25f, 
            spawnPointXVariation = 0.5f, 
            spawnPointYVariation = 0.85f;

        public GameObject doorPrefab, stairsPrefab;
        private Door upDoor, rightDoor, downDoor, leftDoor;

        private MinimapRoomTile minimapTile;
        private RoomState state;

        private List<EnemyTemplate> enemyTemplates;
        private List<EnemyController> enemies;

        private SpriteManager spriteManager;

        private void Awake() {
            this.spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
            this.enemyTemplates = new List<EnemyTemplate>();
            this.enemies = new List<EnemyController>();
        }

        private void Start() {
            this.spriteManager.SetColor(Color.clear);
        }

        private Door GetDoor(Direction? direction) {
            switch (direction) {
                case Direction.Up: return upDoor;
                case Direction.Right: return rightDoor;
                case Direction.Down: return downDoor;
                case Direction.Left: return leftDoor;
                default: return null;
            }
        }

        /* Instantiates and sets a door in @direction. */
        public void SetDoor(Direction direction) {
            switch (direction) {
                case Direction.Up:
                    this.upDoor = InstantiateDoor("UpDoorPoint", direction);
                    break;

                case Direction.Right:
                    this.rightDoor = InstantiateDoor("RightDoorPoint", direction);
                    break;

                case Direction.Down:
                    this.downDoor = InstantiateDoor("DownDoorPoint", direction);
                    break;

                case Direction.Left:
                    this.leftDoor = InstantiateDoor("LeftDoorPoint", direction);
                    break;
            }
        }

        private Door InstantiateDoor(string parentPointName, Direction direction) {
            Transform parentPoint = this.gameObject.transform.Find(parentPointName);
            GameObject door = Instantiate(doorPrefab, parentPoint.transform);
            door.GetComponent<Door>().SetDirection(direction);
            return door.GetComponent<Door>();
        }

        /* Instantiates and sets stairs door in any empty direction. */
        public void SetStairs() {
            if(upDoor == null) {
                this.upDoor = InstantiateStairs("UpDoorPoint");
            } else if(rightDoor == null) {
                this.rightDoor = InstantiateStairs("RightDoorPoint");
            } else if(downDoor == null) {
                this.downDoor = InstantiateStairs("DownDoorPoint");
            } else if(leftDoor == null) {
                this.leftDoor = InstantiateStairs("LeftDoorPoint");
            }
        }

        private Stairs InstantiateStairs(string parentPointName) {
            Transform parentPoint = this.gameObject.transform.Find(parentPointName);
            GameObject door = Instantiate(stairsPrefab, parentPoint.transform);
            return door.GetComponent<Stairs>();
        }

        public void SetMinimapTile(MinimapRoomTile tile) {
            this.minimapTile = tile;
            this.minimapTile.SetState(this.state);
        }

        /* Updates room's current state to @state when @state is higher.
         * Updates respective tile state when @updateTile is true.
         */
        public void SetState(RoomState state, bool updateTile) {
            if((int)state > (int)this.state) {
                this.state = state;
                if (updateTile) {
                    this.minimapTile.SetState(state);
                }
            }
        }

        /* Moves player into the room (on the door in @direction when specified).
         * If room has enemies, and wasn't already completed, closes doors and spawns enemies.
         */
        public void MovePlayerIn(GameObject player, Direction? direction) {
            EnemyController.CurrentRoom = this;

            SetState(RoomState.Discovered, false);
            this.minimapTile.SetState(RoomState.Current);

            if (!this.state.Equals(RoomState.Completed)) {
                if(this.enemyTemplates != null && this.enemyTemplates.Count > 0) {
                    CloseDoors();
                    SpawnEnemies();
                } else {
                    SetState(RoomState.Completed, false);
                }
            }

            if(GetDoor(direction) != null) {
                GetDoor(direction).Close();
                player.transform.position = GetDoor(direction).gameObject.transform.position;
            } else {
                player.transform.position = this.gameObject.transform.position;
            }
            
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        /* Sends a message to Floor so it moves player to the room in @direction. */
        public void LeaveRoom(Direction direction) {
            this.minimapTile.SetState(this.state);
            SendMessageUpwards("MoveToRoom", direction);
        }

        public void SetEnemies(List<EnemyTemplate> enemyTemplates) {
            this.enemyTemplates = enemyTemplates;
        }

        /* Updates enemy list whenever an enemy is defeated. If no enemies left, room is marked as completed. */
        public void OnEnemyDeath(EnemyController enemy) {
            enemy.Death -= OnEnemyDeath;
            enemies.Remove(enemy);

            if (enemies.Count <= 0) {
                SetState(RoomState.Completed, false);
                StartCoroutine(OpenDoorsWithDelay(0.25f));
            }
        }

        /* Spawns every enemy in the room's list */
        private void SpawnEnemies() {
            System.Random random = new System.Random();
            foreach (EnemyTemplate template in this.enemyTemplates) {
                EnemyController enemy = enemyPool.GetEnemy(template);
                this.enemies.Add(enemy);
                enemy.Death += OnEnemyDeath;
                enemy.Spawn(GetSpawnPoint(random));
            }
        }

        /* Returns a pseudo-random spawning point within one of the room's corners. */
        private Vector3 GetSpawnPoint(System.Random random) {
            float x = 0, y = 0;

            if (random.Next(2) == 1) x = 1;
            else x = -1;

            if (random.Next(2) == 1) y = 1;
            else y = -1;

            x *= spawnPointX + spawnPointXVariation * (float) random.NextDouble();
            y *= spawnPointY + spawnPointYVariation * (float)random.NextDouble();

            return this.transform.position + new Vector3(x, y, 0);
        }

        private IEnumerator OpenDoorsWithDelay(float delay) {
            yield return new WaitForSeconds(delay);
            OpenDoors();
        }

        private void OpenDoors() {
            if (upDoor != null) {
                upDoor.SetLock(false);
                upDoor.Open();
            }
            if (rightDoor != null) {
                rightDoor.SetLock(false);
                rightDoor.Open();
            }
            if (downDoor != null) {
                downDoor.SetLock(false);
                downDoor.Open();
            }
            if (leftDoor != null) {
                leftDoor.SetLock(false);
                leftDoor.Open();
            }
        }

        private void CloseDoors() {
            if (upDoor != null) {
                upDoor.Close();
                upDoor.SetLock(true);
            }
            if (rightDoor != null) {
                rightDoor.Close();
                rightDoor.SetLock(true);
            }
            if (downDoor != null) {
                downDoor.Close();
                downDoor.SetLock(true);
            }
            if (leftDoor != null) {
                leftDoor.Close();
                leftDoor.SetLock(true);
            }
        }
        
        public void FadeIn(float duration) {
            Fade(Color.clear, Color.white, duration);
        }

        public void FadeOut(float duration) {
            Fade(Color.white, Color.clear, duration);
        }

        /* Starts fading animation on itself and on its doors. */
        private void Fade(Color startingColor, Color targetColor, float duration) {
            StartCoroutine(this.spriteManager.Fading(startingColor, targetColor, duration));

            if(this.upDoor != null)
                this.upDoor.Fade(startingColor, targetColor, duration);
            if(this.rightDoor != null)
                this.rightDoor.Fade(startingColor, targetColor, duration);
            if(this.downDoor != null)
                this.downDoor.Fade(startingColor, targetColor, duration);
            if(this.leftDoor != null)
                this.leftDoor.Fade(startingColor, targetColor, duration);
        }

    }
    public enum RoomState {
        Unknown = 0,
        Discovered = 1,
        Completed = 2,
        Current = 3
    }
}

