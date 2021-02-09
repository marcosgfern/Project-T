using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Room : MonoBehaviour {

        public GameObject doorPrefab, stairsPrefab;

        private Door upDoor, rightDoor, downDoor, leftDoor;
        private bool completed;

        private GameObject[] enemies;

        private SpriteManager spriteManager;

        private void Awake() {
            this.spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
        }

        private void Start() {
            this.spriteManager.SetColor(Color.clear);
        }

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

        public void SetStairs() {
            if(upDoor == null) {
                this.upDoor = InstantiateStairs("UpDoorPoint");
            } else if(rightDoor == null) {
                this.rightDoor = InstantiateStairs("RightDoorPoint");
            } else if(downDoor == null) {
                this.downDoor = InstantiateStairs("DownDoorPoint");
            } else if(leftDoor == null) {
                this.leftDoor = InstantiateStairs("LeftDoorPoint");
            } else {
                Debug.Log("Couldn't place stairs. There is a door in every direction.");
            }
        }

        private Stairs InstantiateStairs(string parentPointName) {
            Transform parentPoint = this.gameObject.transform.Find(parentPointName);
            GameObject door = Instantiate(stairsPrefab, parentPoint.transform);
            return door.GetComponent<Stairs>();
        }

        public void MovePlayerIn(GameObject player, Direction? direction) {
            if (!this.completed) {
                if(this.enemies != null && this.enemies.Length > 0) {
                    CloseDoors();
                } else {
                    this.completed = true;
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

        private Door GetDoor(Direction? direction) {
            switch (direction) {
                case Direction.Up: return upDoor;
                case Direction.Right: return rightDoor;
                case Direction.Down: return downDoor;
                case Direction.Left: return leftDoor;
                default: return null;
            }
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
}

