using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingHeart : MonoBehaviour {

    public int healingPoints = 1;

    private void Heal(Collider2D collision) {
        collision.SendMessage("AddHealth", healingPoints);
        Destroy(this.gameObject);
    }
}
