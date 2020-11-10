using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for melee enemies
public class EnemyChasing : MonoBehaviour {

    public Transform player;
    public float moveSpeed = 1f;
    public bool isStunned;
    private Vector2 movement;

    // Update is called once per frame
    void Update() {
        if (!isStunned) {
            Vector3 direction = (player.position - this.transform.position).normalized;
            float rotation = Vector2.SignedAngle(Vector2.right, direction);

            this.transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            this.transform.eulerAngles = new Vector3(0, 0, rotation);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.SendMessageUpwards("AddDamage", 1);
        }
    }
}
