using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    /* Stairs is a subclass of class Door.
     * Instead of moving player to next room, moves it to next floor.
     */
    public class Stairs : Door {

        /* Sends a message to Floor so it starts the generation of a new floor. */
        override public void PlayerEnter() {
            if (!this.isClosed) {
                SendMessageUpwards("MoveToNextFloor");
            }
        }
    }
}

