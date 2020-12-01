using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

    public float maxHealth = 3f;

    private float health;
    private bool invincible;

    private HeartBar heartBar;

    private PlayerController playerController;

    private void Awake() {
        this.heartBar = GetComponent<HeartBar>();
        this.playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start() {
        health = maxHealth;
        invincible = false;
    }

    void AddDamage(int damage) {
        if (!invincible) {
            health = health - damage;

            if (health <= 0) {
                gameObject.SetActive(false);
                //Game over
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
