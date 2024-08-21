using System;
using UnityEngine;

/* Class EnemyHealthController is used as one of the player components.
 * Manages the player's health, and is responsible for the player's heartBar to change.
 */
public class PlayerHealthController : MonoBehaviour {

    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float health;
    [SerializeField] private bool invincible;

    [Header("References")]
    [SerializeField] private HeartBar heartBar;

    public event Action Hurt;
    public event Action Die;


    void Start()
    {
        this.heartBar.SetHealth(this.maxHealth);
        this.heartBar.SetMaxHealth(this.maxHealth);
        this.health = this.maxHealth;
        this.invincible = false;
    }

    /* Adds damage to player's health and triggers the invulnerability cycle.
     * If the player's health reaches 0, triggers the death process.
     */
    void AddDamage(int damage)
    {
        if (!this.invincible)
        {
            this.invincible = true;

            this.health = this.health - damage;
            this.heartBar.SetHealth(this.health);
            
            if (this.health <= 0)
            {
                Die?.Invoke();
            }
            else
            {
                Hurt?.Invoke();
            }
        }
    }

    void AddHealth(int health)
    {
        this.health += health;

        if(this.health > this.maxHealth)
        {
            this.health = this.maxHealth;
        }

        this.heartBar.SetHealth(this.health);
    }

    public void SetInvincibility(bool invincible)
    {
        this.invincible = invincible;
    }

    public bool IsInvincible()
    {
        return this.invincible;
    }

    public bool IsHealthFull()
    {
        return this.health == this.maxHealth;
    }
}
