﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour{

    public float movingForce = 10f;
    public float movingTime = 0.2f;
    public float startingLinearDrag = 1f;
    public float finalLinearDrag = 10f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private float dashStartingTime;

    private Vector2 touchStartingPosition;
    private bool isSwipe = false;
    private Vector2 swipeDirection = Vector2.zero;

    private float rotation = 0;
    private void Awake(){
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        if(Input.touches.Length > 0) {
            Touch touch = Input.GetTouch(0);

            //Get starting position of the touch
            if(touch.phase == TouchPhase.Began) {
                this.touchStartingPosition = touch.position;
                this.isSwipe = false;
            }

            //Check if the action is a tap or a swipe
            if(touch.phase == TouchPhase.Moved) {
                this.isSwipe = true;
            }

            if (touch.phase == TouchPhase.Ended) {
                //Get the position at the end of the swipe
                if(isSwipe) {
                    this.swipeDirection = (touch.position - this.touchStartingPosition).normalized; //Direction of the swipe
                    this.rotation = Vector2.SignedAngle(Vector2.right, this.swipeDirection); //Setting player rotation to direction of swipe
                    StartCoroutine("ChangeDrag");
                }
            }
        }

        this.transform.eulerAngles = new Vector3(0, 0, this.rotation);
    }

    IEnumerator ChangeDrag() {
        this.rigidBody.drag = startingLinearDrag;
        this.rigidBody.AddForce(this.swipeDirection * this.movingForce, ForceMode2D.Impulse);
        this.animator.SetTrigger("Attack");
        


        yield return new WaitForSeconds(movingTime);

        this.rigidBody.drag = finalLinearDrag;
        
    }
}
