using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for shooting enemies
public class ShooterController : EnemyController {

    public GameObject projectilePrefab;
    public float targetDistance = 2f;
    public float shotCoolingTime = 1f;

    private float movingSpeed;
    private bool canShoot = true;
    

    private new void Start() {
        base.Start();
        this.movingSpeed = speed;
    }


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

        void Shoot() {
        if (projectilePrefab != null) {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;

            EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();
            projectileComponent.SetDamage(this.damage);
            projectileComponent.direction = playerTransform.position - transform.position;
        }
    }

    protected override void SecondaryActions() {
        if (canShoot) {
            canShoot = false;
            this.speed = 0f;
            this.animator.SetTrigger("Shoot");
        }
    }

    private IEnumerator ShotCooling() {
        this.speed = movingSpeed;
        this.animator.ResetTrigger("Shoot");
        yield return new WaitForSeconds(shotCoolingTime);
        canShoot = true;
    }

    protected override void Unstun() {
        base.Unstun();
        StartCoroutine(ShotCooling());
    }
}
