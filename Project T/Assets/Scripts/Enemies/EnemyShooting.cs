using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for shooting enemies
public class EnemyShooting : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 1.5f;
    public float targetDistance = 2;
    private Vector2 movement;

    // Update is called once per frame
    void Update() {
        Vector3 vectorToPlayer = player.position - this.transform.position;

        Vector2 direction = Vector2.zero;

        

        if(vectorToPlayer.magnitude >= targetDistance) {
            float distanceFactor = targetDistance / vectorToPlayer.magnitude;
            if (distanceFactor > 1) distanceFactor = 1;

            //A degree vector relative to vectorToPlayer, between 0 and 90
            float xComponent = vectorToPlayer.x * (1 - distanceFactor) - vectorToPlayer.y * (distanceFactor);
            float yComponent = vectorToPlayer.y * (1 - distanceFactor) + vectorToPlayer.x * (distanceFactor);
            direction = new Vector2(xComponent, yComponent);

        } else {
            float distanceFactor = vectorToPlayer.magnitude / targetDistance;
            if (distanceFactor > 1) distanceFactor = 1;

            //A degree vector relative to vectorToPlayer, between 90 and 180 degrees
            float xComponent = vectorToPlayer.x * -1f * (1 - distanceFactor) - vectorToPlayer.y * (distanceFactor);
            float yComponent = vectorToPlayer.y * -1f * (1 - distanceFactor) + vectorToPlayer.x * (distanceFactor);
            direction = new Vector2 (xComponent, yComponent);
        }

        float rotation = Vector2.SignedAngle(Vector2.right, vectorToPlayer);

        this.transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.SendMessageUpwards("AddDamage", 1);
        }
    }
}
