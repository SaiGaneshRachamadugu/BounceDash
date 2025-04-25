using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform leftWallPoint;
    public Transform rightWallPoint;
    public float moveDuration = 0.3f;
    public float jumpForce = 10f;
    public Animator animator;

    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool canJump = true;
    private Vector2 targetPosition;
    private float moveTimer = 0f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float swipeThreshold = 50f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            HandleKeyboardInput();
            HandleTouchInput();
        }

        HandleJumpInput();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartMoveToWall(leftWallPoint.position);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StartMoveToWall(rightWallPoint.position - new Vector3(0.5f, 0, 0));
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;

                    // Check if touch was on the ball
                    Vector3 worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
                    Collider2D hit = Physics2D.OverlapPoint(worldTouch);
                    if (hit != null && hit.transform == transform)
                    {
                        Jump();
                    }
                    break;

                case TouchPhase.Ended:
                    touchEndPos = touch.position;
                    float deltaX = touchEndPos.x - touchStartPos.x;

                    if (Mathf.Abs(deltaX) > swipeThreshold)
                    {
                        if (deltaX > 0)
                        {
                            StartMoveToWall(rightWallPoint.position - new Vector3(0.5f, 0, 0));
                        }
                        else
                        {
                            StartMoveToWall(leftWallPoint.position);
                        }
                    }
                    break;
            }
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (!canJump) return;

        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void StartMoveToWall(Vector2 wallPos)
    {
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        targetPosition = new Vector2(wallPos.x, transform.position.y);
        moveTimer = 0f;
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            moveTimer += Time.fixedDeltaTime;
            float moveTime = moveTimer / moveDuration;
            moveTime = Mathf.Clamp01(moveTime);

            transform.position = Vector2.Lerp(transform.position, targetPosition, moveTime);

            if (moveTime >= 1f)
            {
                isMoving = false;
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
