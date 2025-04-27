using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float bounceForce = 10f;
    private Rigidbody2D rb;

    [SerializeField] private ObstaclesObjectPool obstaclesOp;
    [SerializeField] private CoinCollector2D coinCollector;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, bounceForce);
    }

    private void Update()
    {
        float input = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);
            Vector2.Lerp(this.transform.position, new Vector2(0, bounceForce), Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            ScoreManager.Instance.AddCoin();

            // Fly animation
            coinCollector.CollectCoin(other.transform.position);

            // Disable collected coin
            obstaclesOp.DisableCollidedObj(other.gameObject);
        }
    }
}
