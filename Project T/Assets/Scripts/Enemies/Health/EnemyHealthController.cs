using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EnemyHealth {
    public class EnemyHealthController : MonoBehaviour {

        public int health = 1;
        public DamageColor color = DamageColor.White;

        private LifeBar lifeBar;
        private EnemyController enemyController;

        void Awake() {
            this.lifeBar = GetComponentInChildren<LifeBar>();
            this.enemyController = GetComponent<EnemyController>();
        }

        void Start() {
            this.lifeBar.SetLifePoints(this.health);
        }

        void AddDamage(Damage damageInfo) {           
            this.health = this.health - damageInfo.CalculateDamage(this.color);
            this.lifeBar.SetLifePoints(this.health);
           

            if (health <= 0) {
                Destroy(gameObject);
            } else {
                enemyController.InvulnerabilityTime();
            }
        }

        public DamageColor GetColor() {
            return this.color;
        }
    }
}