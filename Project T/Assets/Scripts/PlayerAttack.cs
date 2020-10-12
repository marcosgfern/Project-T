using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private bool isAttacking = false;
    private Animator animator;
    void Awake() {
        this.animator = GetComponent<Animator>();
    }

    void LateUpdate() {
        isAttacking = this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (isAttacking) {
            if (collision.CompareTag("Enemy")) {
                collision.SendMessageUpwards("AddDamage");
            }
        }
    }
}
