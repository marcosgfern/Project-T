﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasing : MonoBehaviour {

    public Transform player;
    public float moveSpeed = 1f;
    private Vector2 movement;

    // Update is called once per frame
    void Update() {
        Vector3 direction = (player.position - this.transform.position).normalized;
        float rotation = Vector2.SignedAngle(Vector2.right, direction);

        this.transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);

    }
}