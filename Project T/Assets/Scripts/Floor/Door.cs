using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator animator;

    void Start() {
        this.animator = GetComponent<Animator>();
    }

    public void Open() {
        this.animator.SetBool("Closed", false);
    }

    public void Close() {
        this.animator.SetBool("Closed", true);
    }
}
