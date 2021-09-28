using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyHealth {
/* Class LifeBar is used as a component in enemies' health bar.
 * Changes visual size of healthbar depending on an enemy's health points.
 */
    public class LifeBar : MonoBehaviour {

        public float pointWidth = 0.08f;
        public float pointHeight = 0.08f;
        public int lifePoints = 1;

        private SpriteRenderer lifeBarRenderer;

        private void Awake() {
            this.lifeBarRenderer = GetComponentInChildren<SpriteRenderer>();
            this.lifeBarRenderer.enabled = true;
        }

        void Start() {
            this.lifeBarRenderer.size = new Vector2(pointWidth * lifePoints, pointHeight);
        }

        void Update() {
            transform.rotation = Quaternion.identity;
        }

        public void SetHealthPoints(int points) {
            lifePoints = points;
            this.lifeBarRenderer.size = new Vector2(pointWidth * lifePoints, pointHeight);
        }
    }
}
