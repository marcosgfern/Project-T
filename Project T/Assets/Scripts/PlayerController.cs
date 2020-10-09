using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    public float movingForce = 5f;
    public float movingTime = 0.5f;
    public float startingLinearDrag = 1f;
    public float endLinearDrag = 20f;

    private Rigidbody2D rigidBody;
    private bool force = false;
    private float startingTime;

    private void Awake(){
        this.rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start(){
        StartCoroutine("AddForce");
    }

    // Update is called once per frame
    void Update(){
        if (!force) {
            this.rigidBody.drag = this.startingLinearDrag;           
        } else {
            if(Time.time - this.startingTime >= movingTime) {
                this.rigidBody.drag = this.endLinearDrag;
            }
        }
    }

    /*IEnumerator ChangeDrag() {

    }*/

    IEnumerator AddForce() {
        yield return new WaitForSeconds(0.2f);

        this.rigidBody.AddForce(Vector2.right * this.movingForce, ForceMode2D.Impulse);
        this.startingTime = Time.time;
        force = true;

    }


}
