using UnityEngine;

public class BallController : MonoBehaviour
{
    public float bounceForce = 10f;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float swipeThreshold = 50f; // minimum swipe distance in pixels
    private float moveDirection = 0f; // -1 for left, 1 for right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Bounce();
    }

    private void Update()
    {
        HandleKeyboardInput();
        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleKeyboardInput()
    {
        float moveInput = Input.GetAxis("Horizontal");

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mathf.Abs(moveInput) > 0.1f)
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
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
                float deltaX = touchEndPos.x - touchStartPos.x;

                if (Mathf.Abs(deltaX) > swipeThreshold)
                {
                    if (deltaX > 0)
                        moveDirection = 1f;
                    else
                        moveDirection = -1f;
                }
                else
                {
                    moveDirection = 0f;
                }
            }
        }
#endif
    }

    private void HandleMovement()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveDirection * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            Bounce();
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
