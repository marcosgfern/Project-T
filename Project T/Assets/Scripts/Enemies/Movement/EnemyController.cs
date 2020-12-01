using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;

public class EnemyController : MonoBehaviour {

    public Transform player;

    public float movingSpeed = 1f;
    
    protected bool isStunned;
    protected Animator animator;
    private Rigidbody2D enemyRigidbody;
    private Collider2D enemyCollider;

    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    private EnemyHealthController healthController;

    void Awake() {
        this.animator = GetComponent<Animator>();
        this.enemyRigidbody = GetComponent<Rigidbody2D>();
        this.enemyCollider = GetComponent<Collider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.healthController = GetComponent<EnemyHealthController>();
    }

    protected void Start() {
        this.isStunned = false;
        this.animator.SetBool("Moving", true);
        SetColor(healthController.GetColor());
    }

    public void SetColor(DamageColor color) {
        switch (color) {
            case DamageColor.White:
                this.spriteColor = Color.white;
                break;

            case DamageColor.Red:
                this.spriteColor = new Color(0.9411765f, 0.1176471f, 0.1882353f, 1f); //Custom red
                break;

            case DamageColor.Blue:
                this.spriteColor = new Color(0.159399f, 0.4789415f, 0.801f, 1f); //Custom blue
                break;

            default:
                this.spriteColor = Color.white;
                break;
        }

        this.spriteRenderer.color = this.spriteColor;
    }

    public void InvulnerabilityTime() {
        StartCoroutine("Invulnerability");
    }

    protected IEnumerator Invulnerability() {
        Stun();

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

        Unstun();
    }

    protected virtual void Stun(){
        this.isStunned = true;

        //Change animation
        this.animator.SetBool("Moving", false);

        //Disable physics
        enemyRigidbody.isKinematic = true;
        enemyRigidbody.velocity = Vector2.zero;
        enemyRigidbody.angularVelocity = 0f;

        //Disable collisions
        GetComponent<Collider>().enabled = false;
    }

    protected virtual void Unstun(){
        this.isStunned = false;

        //Change animation
        this.animator.SetBool("Moving", true);


        //Enable physics back
        enemyRigidbody.isKinematic = false;

        //Enable collisions back
        GetComponent<Collider>().enabled = true;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.SendMessageUpwards("AddDamage", 1);
        }
    }
}
