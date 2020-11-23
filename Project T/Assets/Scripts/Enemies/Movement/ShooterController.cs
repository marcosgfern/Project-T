using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for shooting enemies
public class ShooterController : EnemyController {

    public GameObject projectilePrefab;
    public float targetDistance = 2f;
    public float shotCoolingTime = 1f;

    private float speed;
    private bool canShoot = true;
    

    private new void Start() {
        base.Start();
        this.speed = movingSpeed;
    }

    // Update is called once per frame
    void Update() {
        if (!isStunned) {
            Vector3 vectorToPlayer = player.position - this.transform.position;

            Vector2 direction = Vector2.zero;

            //If the enemy is further than the target distance
            if (vectorToPlayer.magnitude >= targetDistance) {
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
                direction = new Vector2(xComponent, yComponent);
            }

            float rotation = Vector2.SignedAngle(Vector2.right, vectorToPlayer);

            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            this.transform.eulerAngles = new Vector3(0, 0, rotation);

            if (canShoot) {
                canShoot = false;
                speed = 0;
                this.animator.SetTrigger("Shoot");
            }
        }

    }

    void Shoot() {
        if (projectilePrefab != null) {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;

            EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();

            projectileComponent.direction = player.position - transform.position;
        }
    }

    public void StartShotCooling() {
        StartCoroutine("ShotCooling");
    }

    private IEnumerator ShotCooling() {
        speed = movingSpeed;
        this.animator.ResetTrigger("Shoot");
        yield return new WaitForSeconds(shotCoolingTime);
        canShoot = true;
    }

    protected override void Unstun() {
        base.Unstun();
        StartShotCooling(); //Forcing restart of moving-shooting loop
    }
}
