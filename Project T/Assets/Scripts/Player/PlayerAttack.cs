using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;

/* Class PlayerAttack is used as one of the components for player.
 * Responsible for the player's melee attack, both for animations
 * and applying damage.
 */
public class PlayerAttack : MonoBehaviour {

    public int damage = 1;

    private bool isAttacking = false;
    private Animator animator;
    void Awake() {
        this.animator = GetComponent<Animator>();
    }

    void LateUpdate() {
        isAttacking = (this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));
    }

    /* Sends damage to collision if it is in the animation attack. */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (isAttacking) {
            SendDamage(collision);
        }
    }

    /* If @collision is an enemy, adds red damage to it. */
    private void SendDamage(Collision2D collision) {
        if (collision.collider.CompareTag("Enemy")) {
            collision.collider.SendMessageUpwards("AddDamage", new Damage(damage, DamageColor.Red));
        }
    }
}
