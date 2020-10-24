using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isAttacking) {
            if (collision.collider.CompareTag("Enemy")) {
                collision.collider.SendMessageUpwards("AddDamage", new int[] {damage, 1});
            }
        }
    }
}
