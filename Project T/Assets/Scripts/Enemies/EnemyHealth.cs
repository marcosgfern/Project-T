using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public int health = 1;
    //Color of the enemy (0 = white, 1 = red, 2 = blue)
    public int color = 0;

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private EnemyChasing chasingScript;
    private EnemyShooting shootingScript;
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    void Awake() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
        this.chasingScript = GetComponent<EnemyChasing>();
        this.shootingScript = GetComponent<EnemyShooting>();
    }

    void Start() {
        switch (this.color) {
            case 0:
                this.spriteColor = Color.white;
                break;

            case 1:
                this.spriteColor = new Color(0.9411765f, 0.1176471f, 0.1882353f, 1f);
                break;

            case 2:
                this.spriteColor = new Color(0.159399f, 0.4789415f, 0.801f, 1f);
                break;

            default: 
                this.spriteColor = Color.white;
                break;
        }

        this.spriteRenderer.color = this.spriteColor;
    }

    /*
     * Parameter damageInfo:
     * damageInfo[0] = damage
     * damageInfo[1] = color
    */
    void AddDamage(int[] damageInfo) {
        if(this.color == 0) {
            this.health = this.health - damageInfo[0];
        } else {
            if(this.color == damageInfo[1]) {
                this.health = this.health - damageInfo[0];
            }
        }

        if(health <= 0) {
            Destroy(gameObject);
        } else {
            StartCoroutine("InvulnerabilityTime");
        }
    }

    private IEnumerator InvulnerabilityTime() {
        //Stop melee enemy movement
        float speed = 0;
        if(chasingScript != null) {
            speed = chasingScript.moveSpeed;
            chasingScript.moveSpeed = 0;
        }

        //Stop shooting enemy movement
        float shootingSpeed = 0;
        if (shootingScript != null) {
            speed = shootingScript.movingSpeed;
            shootingScript.movingSpeed = 0;
            shootingSpeed = shootingScript.shootingSpeed;
            shootingScript.shootingSpeed = 0;
        }

        //Disable physics
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;

        //Disable collisions
        collider.enabled = false;

        //Visual feedback
        this.spriteRenderer.color = spriteColor * Color.yellow;
        yield return new WaitForSeconds(0.05f);
        this.spriteRenderer.color = spriteColor * Color.red;
        yield return new WaitForSeconds(0.05f);
        this.spriteRenderer.color = spriteColor * Color.white;
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < 5; i++) {
            this.spriteRenderer.color = spriteColor * new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(0.1f);
            this.spriteRenderer.color = spriteColor * new Color(1f, 1f, 1f, 0.6f);
            yield return new WaitForSeconds(0.1f);
        }

        this.spriteRenderer.color = spriteColor * Color.white;

        //Reset melee enemy movement
        if (chasingScript != null) {
            chasingScript.moveSpeed = speed;
        }

        //Reset shooting enemy movement
        if (shootingScript != null) {
            shootingScript.movingSpeed = speed;
            shootingScript.shootingSpeed = shootingSpeed;
        }

        //Enable physics back
        rigidbody.isKinematic = false;

        //Enable collisions back
        collider.enabled = true;
    }
}
