using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for melee enemies
public class MeleeController : EnemyController {

    // Update is called once per frame
    void Update() {
        if (!isStunned) {
            Vector3 direction = (playerTransform.position - this.transform.position).normalized;
            float rotation = Vector2.SignedAngle(Vector2.right, direction);

            this.transform.Translate(direction * this.movingSpeed * Time.deltaTime, Space.World);
            this.transform.eulerAngles = new Vector3(0, 0, rotation);
        }
    }
}
