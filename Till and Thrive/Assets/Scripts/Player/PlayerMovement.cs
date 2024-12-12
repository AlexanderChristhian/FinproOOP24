using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float pixelsPerUnit = 16f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 targetPosition;
    private bool isMoving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        targetPosition = rb.position;
    }

    private void Update()
    {
        // Get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (!isMoving)
        {
            // Allow both horizontal and vertical movement simultaneously
            movement = new Vector2(horizontalInput, verticalInput);

            // Normalize diagonal movement to maintain consistent speed
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            // If there's movement input, set new target position
            if (movement != Vector2.zero)
            {
                // Calculate the next position in grid units
                Vector2 nextPosition = rb.position + movement * (1f / pixelsPerUnit);
                // Round to nearest pixel grid position
                targetPosition = new Vector2(
                    Mathf.Round(nextPosition.x * pixelsPerUnit) / pixelsPerUnit,
                    Mathf.Round(nextPosition.y * pixelsPerUnit) / pixelsPerUnit
                );
                isMoving = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 currentPosition = rb.position;
            Vector2 newPosition = Vector2.MoveTowards(
                currentPosition,
                targetPosition,
                moveSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(newPosition);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                isMoving = false;
            }
        }
    }
}