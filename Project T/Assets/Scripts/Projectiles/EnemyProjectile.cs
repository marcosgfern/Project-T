using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Subclass of proyectile for enemy proyectiles. */
public class EnemyProjectile : Projectile {

    /* Overrides proyectile function so the damage is added when the collision is player. */
    protected override void SendDamage(Collision2D collision) {
        Debug.Log("SendDamage");
        if (collision.collider.CompareTag("Player")) {
            Debug.Log("SendMessage");
            collision.collider.SendMessageUpwards("AddDamage", this.damage);
        }
    }
}
