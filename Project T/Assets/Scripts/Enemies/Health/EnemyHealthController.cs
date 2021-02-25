using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EnemyHealth {
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

        void AddDamage(Damage damageInfo) {           
            this.health -= damageInfo.CalculateDamage(this.damageColor);
           
            if (this.health <= 0) {
                this.gameObject.SetActive(false);
            } else {
                this.lifeBar.SetHealthPoints(this.health);
                this.enemyController.StartInvulnerabilityTime();
            }
        }

        public DamageColor GetDamageColor() {
            return this.damageColor;
        }

        public void SetDamageColor(DamageColor color) {
            this.damageColor = color;
        }

        public void SetHealth(int health) {
            this.health = health;
            this.lifeBar.SetHealthPoints(this.health);
        }
    }
}     