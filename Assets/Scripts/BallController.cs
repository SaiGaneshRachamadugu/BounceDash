using UnityEngine;

public class BallController : MonoBehaviour
{
    public float bounceForce = 10f;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Bounce();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
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
    }
}
