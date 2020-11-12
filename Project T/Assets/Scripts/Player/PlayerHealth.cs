using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth = 3f;
    public RectTransform currentHealthUI;
    public float heartSize = 64f;

    private float health;
    private bool invincible;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
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

            StartCoroutine("InvulnerabilityTime");
            this.currentHealthUI.sizeDelta = new Vector2(health * heartSize, heartSize);
        }
    }

    private IEnumerator InvulnerabilityTime() {
        this.invincible = true;

        //Visual feedback
        this.spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.05f);
        this.spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        this.spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < 5; i++) {
            this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(0.1f);
            this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);
            yield return new WaitForSeconds(0.1f);
        }

        this.spriteRenderer.color = Color.white;
        this.invincible = false;
    }
}
