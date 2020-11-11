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
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private EnemyChasing chasingScript;
    private EnemyShooting shootingScript;
    private LifeBar lifeBar;
    private Color spriteColor;

    void Awake() {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.chasingScript = GetComponent<EnemyChasing>();
        this.shootingScript = GetComponent<EnemyShooting>();
        this.lifeBar = GetComponentInChildren<LifeBar>();
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

        this.lifeBar.SetLifePoints(this.health);

        this.animator.SetBool("Moving", true);
    }

    /*
     * Parameter damageInfo:
     * damageInfo[0] = damage
     * damageInfo[1] = color
    */
    void AddDamage(int[] damageInfo) {
        if(this.color == 0) {
            this.health = this.health - damageInfo[0];
            this.lifeBar.SetLifePoints(this.health);
        } else {
            if(this.color == damageInfo[1]) {
                this.health = this.health - damageInfo[0];
                this.lifeBar.SetLifePoints(this.health);
            }
        }

        if(health <= 0) {
            Destroy(gameObject);
        } else {
            StartCoroutine("InvulnerabilityTime");
        }
    }

    private IEnumerator InvulnerabilityTime() {
        //Change animation
        this.animator.SetBool("Moving", false);

        //Stop melee enemy movement
        if(chasingScript != null) {
            chasingScript.isStunned = true;
        }

        //Stop shooting enemy movement
        if (shootingScript != null) {
            shootingScript.isStunned = true;
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

        //Change animation
        this.animator.SetBool("Moving", true);

        //Reset melee enemy movement
        if (chasingScript != null) {
            chasingScript.isStunned = false;
        }

        //Reset shooting enemy movement
        if (shootingScript != null) {
            shootingScript.isStunned = false;
            shootingScript.CoolingTime(); //Force walking-shooting loop to restart
        }

        //Enable physics back
        rigidbody.isKinematic = false;

        //Enable collisions back
        collider.enabled = true;

    }
}
