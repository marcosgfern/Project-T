using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;

/* Responsible for controlling a proyectiles life cycle. */
public class Projectile : MonoBehaviour {

    public Vector2 direction;
    public float speed = 5;
    public int damage = 1;

    void Start() {
        float rotation = Vector2.SignedAngle(Vector2.right, this.direction);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    /* Moves proyectile in its direction every frame. */
    void Update() {
        this.transform.Translate(this.direction.normalized * this.speed * Time.deltaTime, Space.World);
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    /* When colliding, sends damage to collision and destroys itself. */
    void OnCollisionEnter2D(Collision2D collision) {
        SendDamage(collision);
        Destroy(gameObject);
    }

    /* If @collision is an enemy, adds blue damage to it. */
    protected virtual void SendDamage(Collision2D collision) {
        if (collision.collider.CompareTag("Enemy")) {
            collision.collider.SendMessageUpwards("AddDamage", new Damage(damage, DamageColor.Blue));
        }
    }
}
