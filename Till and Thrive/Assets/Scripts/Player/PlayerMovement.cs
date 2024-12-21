using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    public Rigidbody2D rb;
    public Animator animator;
    
    Vector2 movement;
    private string lastDirection = "down"; // Default direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}