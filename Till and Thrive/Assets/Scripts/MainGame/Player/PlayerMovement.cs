using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    public Rigidbody2D rb;
    public Animator animator;
    
    Vector2 movement;
    private string lastDirection = "down"; // Default direction

    private bool canMoveHorizontal = true;
    private bool canMoveVertical = true;

    AudioManager audioManager;
    [SerializeField] AudioClip WalkingSound;

    private float walkingSoundDelay = 0.3f;
    private float lastWalkingSoundTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // Get input
        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");
        
        // Apply movement restrictions
        movement.x = canMoveHorizontal ? rawX : 0;
        movement.y = canMoveVertical ? rawY : 0;

        // Update animator parameters
        animator.SetFloat("horizontal", movement.x);
        animator.SetFloat("vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        // Update direction parameters based on movement
        if (movement != Vector2.zero)
        {
            // Reset all direction floats
            animator.SetFloat("up", 0f);
            animator.SetFloat("down", 0f);
            animator.SetFloat("left", 0f);
            animator.SetFloat("right", 0f);

            // Play walking sound with delay
            if (Time.time - lastWalkingSoundTime >= walkingSoundDelay)
            {
                audioManager.PlaySFX(1, WalkingSound);
                lastWalkingSoundTime = Time.time;
            }

            // Set the new direction
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // Horizontal movement is stronger
                if (movement.x > 0)
                {
                    lastDirection = "right";
                    animator.SetFloat("right", 1f);
                }
                else
                {
                    lastDirection = "left";
                    animator.SetFloat("left", 1f);
                }
            }
            else
            {
                // Vertical movement is stronger
                if (movement.y > 0)
                {
                    lastDirection = "up";
                    animator.SetFloat("up", 1f);
                }
                else
                {
                    lastDirection = "down";
                    animator.SetFloat("down", 1f);
                }
            }
        }
        // Keep last direction active when idle
        else
        {
            // Reset all direction floats first
            animator.SetFloat("up", 0f);
            animator.SetFloat("down", 0f);
            animator.SetFloat("left", 0f);
            animator.SetFloat("right", 0f);
            
            // Set last direction to 1
            animator.SetFloat(lastDirection, 1f);
        }
    }

    void FixedUpdate()
    {
        // Use MovePosition instead of directly modifying velocity
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            canMoveHorizontal = true;
            canMoveVertical = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            // Get the collision normal to determine direction of impact
            Vector2 normal = collision.GetContact(0).normal;
            
            // Stop horizontal movement if hitting a vertical surface
            if (Mathf.Abs(normal.x) > 0.5f)
            {
                movement.x = 0;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            
            // Stop vertical movement if hitting a horizontal surface
            if (Mathf.Abs(normal.y) > 0.5f)
            {
                movement.y = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }
}