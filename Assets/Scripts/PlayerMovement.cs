using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;

    public int maxJumps = 2;         // Max jumps allowed (e.g., 2 for double jump)
    private int jumpCount = 0;       // Tracks how many jumps we've used

    public GameObject attackPoint;
    public float radius;
    public LayerMask enemies;

    public float damage;

    public bool hasKey = false;


    // first
    AudioManager audioManager;
    // Walking SFX cooldown
    private float walkSFXCooldown = 0.3f;
    private float walkSFXTimer = 0f;
    //second
    private void Awake()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found!");
        }
    }
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        // Walking sound
        if (Mathf.Abs(horizontalMove) > 0.1f && walkSFXTimer <= 0f && controller.IsGrounded())
        {
            if (audioManager != null)
                audioManager.playSFX(audioManager.walking, .3f);
                
            walkSFXTimer = walkSFXCooldown;
        }

        if (walkSFXTimer > 0f)
        {
            walkSFXTimer -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            jump = true;
            jumpCount++; // Use a jump
            animator.SetBool("isJumping", true);


            //third
            if (audioManager != null)
                audioManager.playSFX(audioManager.jump, 1f);

        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isAttacking", true);
        }
    }

    public void onLanding()
    {
        animator.SetBool("isJumping", false);
        jumpCount = 0; // Reset jump count on landing

        
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void attack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);

        foreach (Collider2D enemyGameObject in enemiesHit)
        {
            Debug.Log("Hit enemy");
            //third
            if (audioManager != null)
                audioManager.playSFX(audioManager.playerAttack, .3f);

            // Try to get the Health component and deal damage
            Health enemyHealth = enemyGameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }


    public void endAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("key"))
        {
            hasKey = true;

            if (audioManager != null)
                audioManager.playSFX(audioManager.key, 1);

            Destroy(collision.gameObject);
            Debug.Log("player has key");
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("lava"))
        {
            Debug.Log("lavaaaa");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.transform.position, radius);
    }

}
