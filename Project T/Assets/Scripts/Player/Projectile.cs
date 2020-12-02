using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;

public class Projectile : MonoBehaviour {

    public Vector2 direction;
    public float speed = 5;
    public int damage = 1;

    // Start is called before the first frame update
    void Start() {
        float rotation = Vector2.SignedAngle(Vector2.right, this.direction);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    // Update is called once per frame
    void Update() {
        this.transform.Translate(this.direction.normalized * this.speed * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Enemy")) {
            collision.collider.SendMessageUpwards("AddDamage", new Damage(damage, DamageColor.Blue));
        }
        
        Destroy(gameObject);
    }
}
