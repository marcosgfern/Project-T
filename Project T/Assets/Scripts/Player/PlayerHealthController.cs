using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class EnemyHealthController is used as one of the player components.
 * Manages the player's health, and is responsible for the player's heartBar to change.
 */
public class PlayerHealthController : MonoBehaviour {

    public float maxHealth = 3f;

    private float health;
    private bool invincible;

    public GameObject heartBarGameObject;
    private HeartBar heartBar;

    private PlayerController playerController;

    private void Awake() {
        this.heartBar = heartBarGameObject.GetComponent<HeartBar>();
        this.playerController = GetComponent<PlayerController>();
    }

    void Start() {
        this.heartBar.SetHealth(maxHealth);
        this.heartBar.SetMaxHealth(maxHealth);
        health = maxHealth;
        invincible = false;
    }

    /* Adds damage to player's health and triggers the invulnerability cycle.
     * If the player's health reaches 0, triggers the death process.
     */
    void AddDamage(int damage) {
        Debug.Log("AddDamage");
        if (!invincible) {
            health = health - damage;

            if (health <= 0) {
                playerController.Die();
            }

            this.heartBar.SetHealth(health);
            this.playerController.StartInvulnerabilityTime();
        }
    }

    public void SetInvincibility(bool invincible) {
        this.invincible = invincible;
    }

    public bool IsInvincible() {
        return this.invincible;
    }
}
