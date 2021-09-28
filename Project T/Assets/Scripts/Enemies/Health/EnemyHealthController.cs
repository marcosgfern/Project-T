using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace EnemyHealth {
    /* Class EnemyHealthController is used as one of the enemy components.
     * Manages an enemy's health, and is responsible for the enemy's lifeBar to change.
     */
    public class EnemyHealthController : MonoBehaviour {

        public int health = 1;
        public DamageColor damageColor = DamageColor.White;

        private LifeBar lifeBar;
        private EnemyController enemyController;

        void Awake() {
            this.lifeBar = GetComponentInChildren<LifeBar>();
            this.enemyController = GetComponent<EnemyController>();
        }

        void Start() {
            this.lifeBar.SetHealthPoints(this.health);
        }

        /* Calculates the damage the enemy receives.
         * If enemies health is > 0 after the damage, starts invulnerability state.
         * If not, kills the enemy.
         */
        void AddDamage(Damage damageInfo) {           
            SetHealth(this.health - damageInfo.CalculateDamage(this.damageColor));
           
            if (this.health <= 0) {
                this.enemyController.Die();
            } else {
                this.enemyController.StartInvulnerabilityTime();
            }
        }

        public DamageColor GetDamageColor() {
            return this.damageColor;
        }

        public void SetDamageColor(DamageColor color) {
            this.damageColor = color;
        }

        /* Sets health points and updates lifeBar */
        public void SetHealth(int health) {
            this.health = health;
            this.lifeBar.SetHealthPoints(this.health);
        }
    }
}