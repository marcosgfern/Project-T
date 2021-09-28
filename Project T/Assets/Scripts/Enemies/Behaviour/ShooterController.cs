using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for shooting enemies */
public class ShooterController : EnemyController {

    public GameObject projectilePrefab;
    public float targetDistance = 2f;
    public float shotCoolingTime = 1f;

    private float movingSpeed;
    private bool canShoot = true;
    

    private new void Awake() {
        base.Awake();
        this.movingSpeed = speed;
    }

    /* Direction is calculated so the enemy tries to go to a point in the circumference with 
     *      radius = ShooterController.targetDistance
     *      and center = EnemyController.playerTransform.position,
     * while also moving counterclockwise to the player.
     */
    override protected Vector2 CalculateDirection() {
        Vector3 vectorToPlayer = playerTransform.position - this.transform.position;

        //If the enemy is further than the target distance
        if (vectorToPlayer.magnitude >= targetDistance) {
            float distanceFactor = targetDistance / vectorToPlayer.magnitude;
            if (distanceFactor > 1) distanceFactor = 1;

            //A degree vector relative to vectorToPlayer, between 0 and 90
            float xComponent = vectorToPlayer.x * (1 - distanceFactor) - vectorToPlayer.y * (distanceFactor);
            float yComponent = vectorToPlayer.y * (1 - distanceFactor) + vectorToPlayer.x * (distanceFactor);
            return new Vector2(xComponent, yComponent);

        } else {
            float distanceFactor = vectorToPlayer.magnitude / targetDistance;
            if (distanceFactor > 1) distanceFactor = 1;

            //A degree vector relative to vectorToPlayer, between 90 and 180 degrees
            float xComponent = vectorToPlayer.x * -1f * (1 - distanceFactor) - vectorToPlayer.y * (distanceFactor);
            float yComponent = vectorToPlayer.y * -1f * (1 - distanceFactor) + vectorToPlayer.x * (distanceFactor);
            return new Vector2(xComponent, yComponent);
        }
    }

    public override void Spawn(Vector3 spawnPoint) {
        base.Spawn(spawnPoint);
        canShoot = true;
    }

    /* Shoots projectile in the direction of the player. */ 
    void Shoot() {
        if (projectilePrefab != null) {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;

            EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();
            projectileComponent.SetDamage(this.damage);
            projectileComponent.direction = playerTransform.position - transform.position;
        }
    }

    /* If shot cooling time is over, starts a new cycle of shooting. */ 
    protected override void SecondaryActions() {
        if (canShoot) {
            canShoot = false;
            this.speed = 0f;
            this.animator.SetTrigger("Shoot");
        }
    }

    /* Coroutine: waits the shot cooling time.
     * Triggered by animator.     
     */ 
    private IEnumerator ShotCooling() {
        this.speed = movingSpeed;
        this.animator.ResetTrigger("Shoot");
        yield return new WaitForSeconds(shotCoolingTime);
        canShoot = true;
    }

    /* Resets shooting loop after stun. */
    protected override void Unstun() {
        base.Unstun();
        if (canShoot) {
            this.animator.ResetTrigger("Shoot");
        } else {
            StartCoroutine(ShotCooling());
        }
    }
}
