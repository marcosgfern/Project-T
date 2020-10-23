using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for shooting enemies
public class EnemyShooting : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public float movingSpeed = 1f;
    public float shootingSpeed = 0.2f;
    public float targetDistance = 2f;
    public float timeToShoot = 1f;
    public float shotCoolingTime = 1f;

    private float speed;
    private bool canShoot = true;

    private void Start() {
        this.speed = movingSpeed;
    }

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

        this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);

        if (canShoot) {
            canShoot = false;
            StartCoroutine("Shooting");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.SendMessageUpwards("AddDamage", 1);
        }
    }

    IEnumerator Shooting() {
        //Slowing down. Preparing to shoot
        speed = shootingSpeed;
        yield return new WaitForSeconds(timeToShoot);

        Shoot(player.position, transform.position);

        yield return new WaitForSeconds(0.1f);
        speed = movingSpeed;
        yield return new WaitForSeconds(shotCoolingTime - 0.1f);

        canShoot = true;
    }

    void Shoot(Vector2 target, Vector2 shootingPoint) {
        if (projectilePrefab != null) {
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint, Quaternion.identity) as GameObject;

            EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();

            projectileComponent.direction = target - shootingPoint;
        }
    }
}
