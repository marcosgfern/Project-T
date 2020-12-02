using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

class TouchManager {
    private Vector2 startingPosition;
    private Vector2 currentPosition;
    private bool swipe;
    private TouchPhase? phase;

    public void Update() {
        if (Input.touches.Length > 0) {
            Touch touch = Input.GetTouch(0);

            this.phase = touch.phase;

            switch (touch.phase) {
                case TouchPhase.Began:
                    this.startingPosition = this.currentPosition = touch.position;
                    this.swipe = false;

                    break;
                case TouchPhase.Moved:
                    this.swipe = true;
                    this.currentPosition = touch.position;

                    break;
                case TouchPhase.Ended:
                    this.currentPosition = touch.position;
                    break;
            }
        } else {
            phase = null;
        }
    }

    public Vector2 GetStartingPosition() {
        return this.startingPosition;
    }

    public Vector2 GetSwipeDirection() {
        return (this.currentPosition - this.startingPosition).normalized;
    }

    public bool IsSwipe() {
        return this.swipe;
    }

    public TouchPhase? GetPhase() {
        return this.phase;
    }
}

public class PlayerController : MonoBehaviour {

    public float movingForce = 10f;
    public float movingTime = 0.2f;
    public float startingLinearDrag = 1f;
    public float finalLinearDrag = 10f;
    public float shotCoolingTime = 0.1f;

    public GameObject projectilePrefab;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private PlayerHealthController healthController;
    private TouchManager touchManager;
    private SpriteManager spriteManager;

    private bool canShoot = true;


    private void Awake(){
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();

        this.healthController = GetComponent<PlayerHealthController>();
        this.touchManager = new TouchManager();
        this.spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
    }

    // Update is called once per frame
    void Update(){
        this.touchManager.Update();

        switch(this.touchManager.GetPhase()) {
            case TouchPhase.Moved:
                this.ChargeAttack();
                break;
            case TouchPhase.Ended:
                if (this.touchManager.IsSwipe()) {
                    this.DoMeleeAttack();
                } else {
                    this.DoRangedAttack();
                }
                break;
        }
    }

    private void ChargeAttack() {
        this.animator.SetBool("IsSwiping", true);
        this.RotatePlayerToTouch();
    }

    private void DoMeleeAttack() {
        this.RotatePlayerToTouch();
        StartCoroutine("ChangeDrag");
    }

    private void DoRangedAttack() {
        if (canShoot) {
            canShoot = false;
            StartCoroutine("Shooting");
        }
    }

    private void RotatePlayerToTouch() {
        this.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, this.touchManager.GetSwipeDirection()));
    }

    IEnumerator ChangeDrag() {
        canShoot = false;
        this.rigidBody.drag = startingLinearDrag;

        this.rigidBody.velocity = Vector2.zero;
        this.rigidBody.AddForce(this.touchManager.GetSwipeDirection() * this.movingForce, ForceMode2D.Impulse);

        this.animator.SetBool("IsSwiping", false);

        yield return new WaitForSeconds(movingTime);

        this.rigidBody.drag = finalLinearDrag;
        this.animator.SetTrigger("EndAttack");
        canShoot = true;       
    }

    IEnumerator Shooting() {
        //Setting player rotation to direction of shot
        float rotation = Vector2.SignedAngle(Vector2.right, Camera.main.ScreenToWorldPoint(this.touchManager.GetStartingPosition()) - this.transform.position);
        this.transform.eulerAngles = new Vector3(0, 0, rotation);

        Shoot(Camera.main.ScreenToWorldPoint(this.touchManager.GetStartingPosition()), this.transform.position);

        yield return new WaitForSeconds(shotCoolingTime);
        canShoot = true;
    }

    void Shoot(Vector2 target, Vector2 shootingPoint) {
        if (projectilePrefab != null) {
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint, Quaternion.identity) as GameObject;

            Projectile projectileComponent = projectile.GetComponent<Projectile>();

            projectileComponent.direction = target - shootingPoint;
        }
    }

    public void StartInvulnerabilityTime() {
        StartCoroutine("InvulnerabilityTime");
    }
    private IEnumerator InvulnerabilityTime() {
        if (!this.healthController.IsInvincible()) {
            this.healthController.SetInvincibility(true);

            yield return StartCoroutine(this.spriteManager.HitFlash());
            yield return StartCoroutine(this.spriteManager.InvulnerabilityFlash(1f));
            this.spriteManager.ResetColor();

            this.healthController.SetInvincibility(false);
        }
    }
}