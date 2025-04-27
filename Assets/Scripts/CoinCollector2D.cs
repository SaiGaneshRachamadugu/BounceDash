using UnityEngine;
using System.Collections;

public class CoinCollector2D : MonoBehaviour
{
    [Header("Flying Coin Setup")]
    public GameObject flyingCoinPrefab;    // 2D coin prefab (sprite)
    public Transform coinTarget;           // UI target (your coin counter)
    public float flySpeed = 5f;             // Movement speed
    public float rotationSpeed = 360f;      // Rotation speed in degrees/second

    private GameObject flyingCoinInstance; // Single reusable instance

    public void CollectCoin(Vector3 startWorldPosition)
    {
        if (flyingCoinInstance == null)
        {
            flyingCoinInstance = Instantiate(flyingCoinPrefab, startWorldPosition, Quaternion.identity);
        }

        flyingCoinInstance.SetActive(true);
        flyingCoinInstance.transform.position = startWorldPosition;
        flyingCoinInstance.transform.rotation = Quaternion.identity; // Reset rotation
        StartCoroutine(FlyAndRotateToTarget(flyingCoinInstance.transform));
    }

    private IEnumerator FlyAndRotateToTarget(Transform flyingCoin)
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            coinTarget.position.x,
            coinTarget.position.y + 20f,
            10f)); // Make sure it's ahead of camera
        targetPosition.z = 0;

        while (Vector3.Distance(flyingCoin.position, targetPosition) > 0.1f)
        {
            // Move towards target
            flyingCoin.position = Vector3.MoveTowards(
                flyingCoin.position,
                targetPosition,
                flySpeed * Time.deltaTime);

            // Rotate around Z axis (2D rotation)
            flyingCoin.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        // After reaching, deactivate
        flyingCoin.gameObject.SetActive(false);
    }
}
