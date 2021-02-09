using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floors {
    public class Stairs : Door {
        override public void PlayerEnter() {
            if (!this.closed) {
                SendMessageUpwards("GenerateNextFloor");
            }
        }
    }
}

