using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyHealth {

    public class LifeBar : MonoBehaviour {

        public float pointWidth = 0.08f;
        public float pointHeight = 0.08f;
        public int lifePoints = 1;

        private SpriteRenderer lifeBarRenderer;

        private void Awake() {
            this.lifeBarRenderer = GetComponentInChildren<SpriteRenderer>();
            this.lifeBarRenderer.enabled = true;
        }

        // Start is called before the first frame update
        void Start() {
            this.lifeBarRenderer.size = new Vector2(pointWidth * lifePoints, pointHeight);
        }

        // Update is called once per frame
        void Update() {
            transform.rotation = Quaternion.identity;
        }

        public void SetLifePoints(int points) {
            lifePoints = points;
            this.lifeBarRenderer.size = new Vector2(pointWidth * lifePoints, pointHeight);
        }
    }
}
