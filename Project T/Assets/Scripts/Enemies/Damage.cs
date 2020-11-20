using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyHealth {

    public enum DamageColor { White, Red, Blue }

    public class Damage {

        int damagePoints;
        DamageColor damageColor;

        public Damage(int damagePoints, DamageColor damageColor) {
            this.damagePoints = damagePoints;
            this.damageColor = damageColor;
        }


        public int CalculateDamage(DamageColor enemyColor) {
            if (enemyColor == DamageColor.White) {
                return this.damagePoints;
            } else {
                if (enemyColor == this.damageColor) {
                    return this.damagePoints;
                } else return 0;
            }
        }


    }
}