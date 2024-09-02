using System.Collections;
using UnityEngine;

/* PlayerController is used as the main component for player character.
 * In charge of moving the player with the tactile input, starting its animations,
 * controlling other components...
 */
public class PlayerController : MonoBehaviour 
{
    [Header("Parameters")]
    [SerializeField] private float movingForce = 10f;
    [SerializeField] private float movingTime = 0.2f;
    [SerializeField] private float startingLinearDrag = 1f;
    [SerializeField] private float finalLinearDrag = 10f;
    [SerializeField] private float shotCoolingTime = 0.1f;

    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CameraController cameraController;

    private Rigidbody2D rigidBody;
    private Animator animator;

    protected TouchManager touchManager;
    private PlayerHealthController healthController;
    private SpriteManager spriteManager;
    private PlayerSFXController sfxController;

    private bool canShoot = true;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        healthController = GetComponent<PlayerHealthController>();
        touchManager = new TouchManager();
        spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
        sfxController = GetComponent<PlayerSFXController>();
    }

    private void Start()
    {
        healthController.Hurt += OnHurt;
        healthController.Die += OnDie;
    }

    /* Checks the current state of the input and controls the character based on it. */
    void Update()
    {
        touchManager.Update();

        switch(touchManager.GetPhase()) {
            case TouchPhase.Moved:
                ChargeAttack();
                break;
            case TouchPhase.Ended:
                if (touchManager.IsSwipe()) {
                    DoMeleeAttack();
                } else {
                    DoRangedAttack();
                }
                break;
        }
    }

    /* Starts the animation of charging a melee attack. */
    protected void ChargeAttack()
    {
        animator.SetBool("IsSwiping", true);
        RotatePlayerToSwipeDirection();
    }

    protected void DoMeleeAttack()
    {
        RotatePlayerToSwipeDirection();
        StartCoroutine(Dashing());
    }

    protected void DoRangedAttack()
    {
        if (canShoot)
        {
            RotatePlayerToTouch();
            StartCoroutine(Shooting());
        }
    }

    private void RotatePlayerToSwipeDirection()
    {
        transform.eulerAngles = new Vector3(
            0,
            0,
            Vector2.SignedAngle(
                Vector2.right,
                touchManager.GetSwipeDirection()));
    }

    private void RotatePlayerToTouch()
    {
        float rotationAngle = Vector2.SignedAngle(
            Vector2.right, 
            Camera.main.ScreenToWorldPoint(
                touchManager.GetStartingPosition()) - transform.position);

        transform.eulerAngles = new Vector3(0, 0, rotationAngle);
    }

    /* Applies a force to player's game object in the direction of a swipe.
     * After PlayerController.movingTime seconds,
     * slows down the game object by increasing object's linear drag. */
    private IEnumerator Dashing()
    {
        canShoot = false;

        rigidBody.drag = startingLinearDrag;

        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce(
            touchManager.GetSwipeDirection() * movingForce,
            ForceMode2D.Impulse);

        animator.SetBool("IsSwiping", false);

        yield return new WaitForSeconds(movingTime);

        rigidBody.drag = finalLinearDrag;
        animator.SetTrigger("EndAttack");

        canShoot = true;       
    }

    /* If shooting cooldown is over, shoots a proyectile
     * in the direction of the touch from the player. */
    private IEnumerator Shooting()
    {
        canShoot = false;

        Shoot(Camera.main.ScreenToWorldPoint(
            touchManager.GetStartingPosition()), 
            transform.position);

        yield return new WaitForSeconds(shotCoolingTime);

        canShoot = true;
    }

    /* Shoots a proyectile from @shootingPoint to @target. */
    private void Shoot(Vector2 target, Vector2 shootingPoint)
    {
        if (projectilePrefab != null) {
            sfxController.PlayShot();

            GameObject projectile = Instantiate(
                projectilePrefab, 
                shootingPoint, 
                Quaternion.identity) as GameObject;

            Projectile projectileComponent = projectile.GetComponent<Projectile>();

            projectileComponent.direction = target - shootingPoint;

        }
    }

    private void OnHurt()
    {
        sfxController.PlayHit();
        cameraController.DoPlayerHitShake();
        StartCoroutine(InvulnerabilityTime());
    }

    /* Start cycle of invulnerability:
     * Makes player invincible -> Makes hit flash -> 
     * -> starts invulnerability flickering -> removes player's invincibility.*/
    private IEnumerator InvulnerabilityTime() 
    {
        yield return StartCoroutine(spriteManager.HitFlash());
        yield return StartCoroutine(spriteManager.InvulnerabilityFlash(1f));
        spriteManager.ResetColor();

        healthController.SetInvincibility(false);
    }

    /* Makes death screen show up (activated when player health reaches 0). */
    public void OnDie() 
    {
        sfxController.PlayHit();
        cameraController.DoPlayerHitShake();

        healthController.SetInvincibility(true);
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector2.zero;
        animator.SetTrigger("Die");       
    }

    /* Starts death screen animation. 
     * This function is triggered by an animator event. */
    public void ShowDeathScreen() 
    {
        deathScreen.SetActive(true);
    }

    /* Manages collisions with doors and hearts */
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        switch (collision.tag) 
        {
            case "Door":
                collision.SendMessage("PlayerEnter");
                break;

            case "Heart":
                if(!healthController.IsHealthFull())
                {
                    sfxController.PlayHeartPickUp();
                    collision.SendMessage(
                        "Heal", 
                        gameObject.GetComponent<Collider2D>());
                }
                break;
        }
    }

    /* Notifies door if player leaves its collision area. */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Door") {
            collision.SendMessage("PlayerExit");
        }
    }
}