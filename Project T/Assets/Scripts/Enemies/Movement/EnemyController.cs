using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;

public class EnemyController : MonoBehaviour {

    public Transform player;

    public float movingSpeed = 1f;
    
    protected bool isStunned;
    protected Animator animator;
    private Rigidbody2D rigidbody;
    private Collider2D collider;

    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    void Awake() {
        this.animator = GetComponent<Animator>();
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start() {
        this.isStunned = false;
        this.animator.SetBool("Moving", true);
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
        //Change animation
        this.animator.SetBool("Moving", false);

        Stun();

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

        Unstun();

        //Enable physics back
        rigidbody.isKinematic = false;

        //Enable collisions back
        collider.enabled = true;
    }

    protected virtual void Stun(){
        this.isStunned = true;
    }

    protected virtual void Unstun(){
        this.isStunned = false;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.SendMessageUpwards("AddDamage", 1);
        }
    }
}
