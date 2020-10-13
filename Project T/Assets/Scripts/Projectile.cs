using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Vector2 direction;
    public float speed;
    // Start is called before the first frame update
    void Start() {
        float rotation = Vector2.SignedAngle(Vector2.right, this.direction);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    // Update is called once per frame
    void Update() {
        this.transform.Translate(this.direction.normalized * this.speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("entro");
        if (collision.CompareTag("Enemy")) {
            collision.SendMessageUpwards("AddDamage");
        }

        Destroy(gameObject);
    }
}
