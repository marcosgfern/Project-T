using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for melee enemies
public class MeleeController : EnemyController {

    override protected Vector2 CalculateDirection() {
        return (playerTransform.position - this.transform.position).normalized;
    }

    protected override void SecondaryActions() { }
}
