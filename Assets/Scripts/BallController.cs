using UnityEngine;

public class BallController : MonoBehaviour
{
    // Force applied when bouncing
    public float bounceForce = 10f;
    // Speed at which the ball moves left or right
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    // Variables for touch swipe input
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    // Minimum swipe distance to detect a swipe
    private float swipeThreshold = 50f;
    // Direction of movement: -1 (left), 1 (right), 0 (no movement)
    private float moveDirection = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Apply an initial bounce at the start
        Bounce();
    }

    private void Update()
    {
        // Handle movement input from keyboard (PC)
        HandleKeyboardInput();
        // Handle movement input from mobile touch
        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        // Apply the movement based on input in physics update
        HandleMovement();
    }

    private void HandleKeyboardInput()
    {
        // Get horizontal input (-1 for left, +1 for right)
        float moveInput = Input.GetAxis("Horizontal");

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mathf.Abs(moveInput) > 0.1f)
            // Set the movement direction
            moveDirection = moveInput;
#endif
    }

    private void HandleTouchInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Record start position of touch
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Record end position of touch
                touchEndPos = touch.position;
                // Difference in x direction
                float deltaX = touchEndPos.x - touchStartPos.x;

                // Check if swipe is big enough to count
                if (Mathf.Abs(deltaX) > swipeThreshold)
                {
                    if (deltaX > 0)
                        // Swipe right
                        moveDirection = 1f;
                    else
                        // Swipe left
                        moveDirection = -1f;
                }
                else
                {
                    // No valid swipe detected
                    moveDirection = 0f;
                }
            }
        }
#endif
    }

    private void HandleMovement()
    {
        // Apply movement based on input
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveDirection * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void Bounce()
    {
        // Apply a vertical bounce force
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is from below (ground)
        if (collision.contacts[0].normal.y > 0.5f)
        {
            Bounce(); // Bounce again when hitting the ground
        }

        // Check if collided with an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver(); 
        }
    }
}
