using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Door : MonoBehaviour {

        protected Direction direction;
        protected bool closed;
        protected bool locked;

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

        public void Open() {
            this.closed = false;
            this.animator.SetBool("Closed", false);
        }

        public void Close() {
            this.closed = true;
            this.animator.SetBool("Closed", true);
        }

        public void SetLock(bool isLocked) {
            this.locked = isLocked;
        }

        public virtual void PlayerEnter() {
            if (!this.closed) {
                SendMessageUpwards("LeaveRoom", this.direction);
            }
        }

        public void PlayerExit() {
            if (!this.locked) {
                this.Open();
            }
        }

        public void Fade(Color startingColor, Color targetColor, float duration) {
            StartCoroutine(this.spriteManager.Fading(startingColor, targetColor, duration));
        }
    }
}


