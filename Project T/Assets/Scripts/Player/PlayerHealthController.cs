using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void AddDamage(int damage) {
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
