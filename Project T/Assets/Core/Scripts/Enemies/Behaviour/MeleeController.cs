using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for melee enemies */
public class MeleeController : EnemyController {

    /* Returns the vector from the enemy to the player's current position. */
    override protected Vector2 CalculateDirection() {
        return (playerTransform.position - this.transform.position).normalized;
    }

    protected override void SecondaryActions() { }
}
