using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;

    // Update is called once per frame
    void AddDamage() {
        health += -1;
        Debug.Log("entro");

        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
