using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;
using Floors;

/* Class EnemyController is a superclass for enemy behaviour. */
public abstract class EnemyController : MonoBehaviour {

    public static Room CurrentRoom;

    public float speed = 1f;
    public int damage = 1;
    
    protected Transform playerTransform;

    protected bool isStunned;

    protected Animator animator;
    private Rigidbody2D enemyRigidbody;
    private Collider2D enemyCollider;

    private SpriteManager spriteManager;

    private EnemyHealthController healthController;

    protected void Awake() {
        this.playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        this.animator = GetComponent<Animator>();
        this.enemyRigidbody = GetComponent<Rigidbody2D>();
        this.enemyCollider = GetComponent<Collider2D>();
        this.healthController = GetComponent<EnemyHealthController>();

        this.spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
        this.spriteManager.SetMainColor(healthController.GetDamageColor());

    }

    /* Sets enemy's stats to the given @health, @color and @damage. */ 
    public void ResetEnemy(int health, DamageColor color, int damage) {
        this.healthController.SetHealth(health);
        this.healthController.SetDamageColor(color);
        this.spriteManager.SetMainColor(color);
        this.damage = damage;
    }

    /* Moves inactive enemy to @spawnPoint and actives it. */ 
    public virtual void Spawn(Vector3 spawnPoint) {
        this.transform.position = spawnPoint;
        this.transform.eulerAngles = new Vector3(0, 0, -90);
        this.gameObject.SetActive(true);
        StartCoroutine(AwakingTime());
    }

    /* Deactivates defeated enemy, and updates enemy count of the current room the enemy was in. */
    public void Die() {
        this.animator.SetBool("Moving", false);
        this.gameObject.SetActive(false);
        CurrentRoom.UpdateEnemyCount();
    }

    /* Defines behaviour of enemy. */
    protected void Update() {
        if (!this.isStunned) {
            RotateEnemyToPlayer();
            Move();
            SecondaryActions();
        }
    }

    private void RotateEnemyToPlayer() {
        Vector2 directionToPlayer = playerTransform.position - this.transform.position;
        float angleToPlayer = Vector2.SignedAngle(Vector2.right, directionToPlayer);
        this.transform.eulerAngles = new Vector3(0, 0, angleToPlayer);
    }

    /* Moves enemy in a direction (CalculateDirection()) proportionally to deltaTime and speed.  */ 
    private void Move() {
        Vector3 deltaVector = CalculateDirection().normalized * this.speed * Time.deltaTime;
        this.transform.Translate(deltaVector, Space.World);
    }

    /* Calculates the enemy's direction for a given frame. */
    protected abstract Vector2 CalculateDirection();

    /* Used for different action performed by different kinds of enemy. */
    protected abstract void SecondaryActions();


    public void StartInvulnerabilityTime() {
        StartCoroutine(InvulnerabilityTime());
    }

    /* Start cycle of invulnerability:
     * Stuns enemy -> Makes hit flash -> starts invulnerability flickering -> unstuns enemy.
     */
    protected IEnumerator InvulnerabilityTime() {
        Stun();

        yield return StartCoroutine(this.spriteManager.HitFlash());
        yield return StartCoroutine(this.spriteManager.InvulnerabilityFlash(1f));
        this.spriteManager.ResetColor();

        Unstun();
    }

    protected IEnumerator AwakingTime() {
        Stun();

        yield return StartCoroutine(
            this.spriteManager
            .Fading(
                Color.clear, 
                Color.white, 
                0.75f));

        Unstun();
    }

    /* Stops enemy's movement and normal behaviour. */
    protected virtual void Stun(){
        this.isStunned = true;

        this.animator.SetBool("Moving", false);

        enemyRigidbody.isKinematic = true;
        enemyRigidbody.velocity = Vector2.zero;
        enemyRigidbody.angularVelocity = 0f;

        this.enemyCollider.enabled = false;
    }

    /* Restarts enemy's normal behaviour. */
    protected virtual void Unstun(){
        this.isStunned = false;

        this.animator.SetBool("Moving", true);

        enemyRigidbody.isKinematic = false;

        this.enemyCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        SendDamage(collision);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        SendDamage(collision);
    }

    /* Sends damage to collision when this one has Player tag. */
    private void SendDamage(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            collision.collider.SendMessageUpwards("AddDamage", this.damage);
        }
    }
}

