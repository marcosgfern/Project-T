using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Door : MonoBehaviour {

        private Direction direction;
        private bool closed;
        private bool locked;

        private Animator animator;

        void Awake() {
            this.animator = GetComponent<Animator>();
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
                SendMessageUpwards("MoveToRoom", this.direction);
            }
        }

        public void PlayerExit() {
            if (!this.locked) {
                this.Open();
            }
        }
    }
}


