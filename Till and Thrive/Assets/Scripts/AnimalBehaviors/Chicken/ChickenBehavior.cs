using UnityEngine;

public class ChickenBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float minWanderTime = 2f;
    public float maxWanderTime = 5f;

    [Header("State Settings")]
    public float minStateTime = 3f;
    public float maxStateTime = 8f;
    public float stateChangeChance = 0.3f;

    [Header("Boundary Settings")]
    public Vector2 spawnPoint;
    public float maxWanderRadius = 5f;

    [Header("Egg Laying Settings")]
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private float layInterval = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private float wanderTimer;
    private float stateTimer;
    private float layTimer;
    private ChickenState currentState;
    private bool facingRight = true;

    private enum ChickenState
    {
        Idle,
        Moving,
        Eating,
        Sleeping
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        SetNewWanderTimer();
        SetNewStateTimer();
        layTimer = layInterval;
        currentState = ChickenState.Idle;

        // Ignore collision with player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void Update()
    {
        UpdateTimers();
        UpdateAnimator();
        UpdateSpriteDirection();
        CheckBoundary();
        HandleEggLaying();
    }

    void FixedUpdate()
    {
        if (currentState == ChickenState.Moving)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void UpdateTimers()
    {
        if (wanderTimer > 0)
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0)
            {
                ChangeMovementState();
            }
        }

        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0)
            {
                ChangeActivityState();
            }
        }

        layTimer -= Time.deltaTime;
    }

    void HandleEggLaying()
    {
        if (layTimer <= 0f && eggPrefab != null)
        {
            SpawnEgg();
            layTimer = layInterval;
        }
    }

    void SpawnEgg()
    {
        GameObject egg = Instantiate(eggPrefab, transform.position, Quaternion.identity);

        Collider2D eggCollider = egg.GetComponent<Collider2D>();
        if (eggCollider != null)
        {
            Physics2D.IgnoreCollision(eggCollider, GetComponent<Collider2D>());
        }
    }

    void ChangeMovementState()
    {
        if (currentState == ChickenState.Eating || currentState == ChickenState.Sleeping)
        {
            movement = Vector2.zero;
        }
        else
        {
            if (Random.value > 0.3f) // 70% chance to move
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector2 targetPosition = spawnPoint + randomDirection * maxWanderRadius;
                movement = (targetPosition - rb.position).normalized;
                currentState = ChickenState.Moving;
            }
            else
            {
                movement = Vector2.zero;
                currentState = ChickenState.Idle;
            }
        }

        SetNewWanderTimer();
    }

    void ChangeActivityState()
    {
        float randomValue = Random.value;
        if (randomValue < stateChangeChance)
        {
            currentState = ChickenState.Idle; // Reset to idle first

            randomValue = Random.value;
            if (randomValue < 0.3f)
            {
                currentState = ChickenState.Eating;
            }
            else if (randomValue < 0.5f)
            {
                currentState = ChickenState.Sleeping;
            }
        }

        movement = Vector2.zero; // Stop movement during activity
        SetNewStateTimer();
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
        animator.SetBool("IsEating", currentState == ChickenState.Eating);
        animator.SetBool("IsSleeping", currentState == ChickenState.Sleeping);
    }

    void CheckBoundary()
    {
        float distanceFromSpawn = Vector2.Distance(rb.position, spawnPoint);
        if (distanceFromSpawn > maxWanderRadius)
        {
            Vector2 directionToSpawn = (spawnPoint - rb.position).normalized;
            movement = directionToSpawn;
            currentState = ChickenState.Moving;
        }
    }

    void UpdateSpriteDirection()
    {
        if (movement.magnitude > 0.1f)
        {
            if (movement.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (movement.x < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    void SetNewWanderTimer()
    {
        wanderTimer = Random.Range(minWanderTime, maxWanderTime);
    }

    void SetNewStateTimer()
    {
        stateTimer = Random.Range(minStateTime, maxStateTime);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            movement = -movement.normalized; // Reverse direction
            currentState = ChickenState.Moving; // Continue moving in the new direction
            SetNewWanderTimer();
        }
    }
}
