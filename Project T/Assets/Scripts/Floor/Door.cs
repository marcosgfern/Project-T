using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    /* Class Door is used as a component for door game objects. */
    public class Door : MonoBehaviour {

        protected Direction direction; //Used for knowing to which room the player will move into after touching the door.
        protected bool isClosed;
        protected bool isLocked;

        protected bool isPlayerIn = false;

        private Animator animator;
        private SpriteManager spriteManager;

        private void Awake() {
            this.animator = GetComponent<Animator>();
            this.spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
        }

        private void Start() {
            this.spriteManager.SetColor(Color.clear);
        }

        public void SetDirection(Direction direction) {
            this.direction = direction;
        }

        /* Opens the door both visually and logically. */
        public void Open() {
            if(!this.isPlayerIn) {
                this.isClosed = false;
                this.animator.SetBool("Closed", false);
            }
        }

        /* Closes the door both visually and logically. */
        public void Close() {
            this.isClosed = true;
            this.animator.SetBool("Closed", true);
        }

        public void SetLock(bool isLocked) {
            this.isLocked = isLocked;
        }

        /* Sends a message to Room to move player to the room of the door's direction. */
        public virtual void PlayerEnter() {
            this.isPlayerIn = true;

            if (!this.isClosed) {
                SendMessageUpwards("LeaveRoom", this.direction);
            }
        }

        /* Opens door (if is unlocked) when the player leaves door's collider.
         * (When player is moved to a new room, the door where it spawns is closed
         * so the player doesn't go back to previous room.)
         */
        public void PlayerExit() {
            this.isPlayerIn = false;

            if (!this.isLocked) {
                this.Open();
            }
        }

        public void Fade(Color startingColor, Color targetColor, float duration) {
            StartCoroutine(this.spriteManager.Fading(startingColor, targetColor, duration));
        }
    }
}


