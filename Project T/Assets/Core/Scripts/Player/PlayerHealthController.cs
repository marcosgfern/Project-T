using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class EnemyHealthController is used as one of the player components.
 * Manages the player's health, and is responsible for the player's heartBar to change.
 */
public class PlayerHealthController : MonoBehaviour {

    public float maxHealth = 3f;
    public float health;
    public bool invincible;

    public GameObject heartBarGameObject;
    private HeartBar heartBar;

    private PlayerController playerController;

    private void Awake() {
        this.heartBar = heartBarGameObject.GetComponent<HeartBar>();
        this.playerController = GetComponent<PlayerController>();
    }

    void Start() {
        this.heartBar.SetHealth(this.maxHealth);
        this.heartBar.SetMaxHealth(this.maxHealth);
        this.health = this.maxHealth;
        this.invincible = false;
    }

    /* Adds damage to player's health and triggers the invulnerability cycle.
     * If the player's health reaches 0, triggers the death process.
     */
    void AddDamage(int damage) {
        if (!this.invincible) {
            this.invincible = true;

            this.health = this.health - damage;
            this.heartBar.SetHealth(this.health);
            
            if (this.health <= 0) {
                playerController.Die();
                return;
            }

            this.playerController.StartInvulnerabilityTime();
        }
    }

    void AddHealth(int health) {
        this.health += health;

        if(this.health > this.maxHealth) {
            this.health = this.maxHealth;
        }

        this.heartBar.SetHealth(this.health);
    }

    public void SetInvincibility(bool invincible) {
        this.invincible = invincible;
    }

    public bool IsInvincible() {
        return this.invincible;
    }

    public bool IsHealthFull() {
        return this.health == this.maxHealth;
    }
}
