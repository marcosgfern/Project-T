using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour {

    public float movingForce = 10f;
    public float movingTime = 0.2f;
    public float startingLinearDrag = 1f;
    public float finalLinearDrag = 10f;
    public float shotCoolingTime = 0.1f;

    public GameObject projectilePrefab;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerHealthController healthController;

    private Vector2 touchStartingPosition;
    private bool isSwipe = false;
    private Vector2 swipeDirection = Vector2.zero;
    
    private float rotation = 0;

    private bool canShoot = true;


    private void Awake(){
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.healthController = GetComponent<PlayerHealthController>();
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
                this.animator.SetBool("IsSwiping", true);
                this.swipeDirection = (touch.position - this.touchStartingPosition).normalized; //Direction of the swipe
                this.rotation = Vector2.SignedAngle(Vector2.right, this.swipeDirection); //Setting player rotation to direction of current touch position
            }

            if (touch.phase == TouchPhase.Ended) {
                //Get the position at the end of the swipe
                if(isSwipe) {
                    this.swipeDirection = (touch.position - this.touchStartingPosition).normalized; //Direction of the swipe
                    this.rotation = Vector2.SignedAngle(Vector2.right, this.swipeDirection); //Setting player rotation to direction of swipe
                    StartCoroutine("ChangeDrag");
                } else {
                    if (canShoot) {
                        canShoot = false;
                        StartCoroutine("Shooting");
                    }
                }
            }
        }

        this.transform.eulerAngles = new Vector3(0, 0, this.rotation);
    }

    IEnumerator ChangeDrag() {
        canShoot = false;
        this.rigidBody.drag = startingLinearDrag;
        this.rigidBody.AddForce(this.swipeDirection * this.movingForce, ForceMode2D.Impulse);
        this.animator.SetBool("IsSwiping", false);



        yield return new WaitForSeconds(movingTime);

        this.rigidBody.drag = finalLinearDrag;
        this.animator.SetTrigger("EndAttack");
        canShoot = true;
        
    }

    IEnumerator Shooting() {
        this.rotation = Vector2.SignedAngle(Vector2.right, Camera.main.ScreenToWorldPoint(this.touchStartingPosition) - this.transform.position); //Setting player rotation to direction of shot
        Shoot(Camera.main.ScreenToWorldPoint(this.touchStartingPosition), this.transform.position);

        yield return new WaitForSeconds(shotCoolingTime);
        canShoot = true;
    }

    public void Shoot(Vector2 target, Vector2 shootingPoint) {
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
        this.healthController.SetInvincibility(true);

        //Visual feedback
        this.spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.05f);
        this.spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        this.spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < 5; i++) {
            this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(0.1f);
            this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.6f);
            yield return new WaitForSeconds(0.1f);
        }

        this.spriteRenderer.color = Color.white;
        this.healthController.SetInvincibility(false);
    }
}
