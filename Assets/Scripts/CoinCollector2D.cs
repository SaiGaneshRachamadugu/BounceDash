using UnityEngine;
using System.Collections;

public class CoinCollector2D : MonoBehaviour
{
    [Header("Flying Coin Setup")]
    public GameObject flyingCoinPrefab;
    public Transform coinTarget;
    public float flySpeed = 5f;
    public float rotationSpeed = 360f;

    private GameObject flyingCoinInstance;

    public void CollectCoin(Vector3 startWorldPosition)
    {
        if (flyingCoinInstance == null)
        {
            flyingCoinInstance = Instantiate(flyingCoinPrefab, startWorldPosition, Quaternion.identity);
        }

        flyingCoinInstance.SetActive(true);
        flyingCoinInstance.transform.position = startWorldPosition;
        flyingCoinInstance.transform.rotation = Quaternion.identity;
        StartCoroutine(FlyAndRotateToTarget(flyingCoinInstance.transform));
    }

    private IEnumerator FlyAndRotateToTarget(Transform flyingCoin)
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            coinTarget.position.x,
            coinTarget.position.y + 20f,
            10f));
        targetPosition.z = 0;

        while (Vector3.Distance(flyingCoin.position, targetPosition) > 0.1f)
        {
            flyingCoin.position = Vector3.MoveTowards(
                flyingCoin.position,
                targetPosition,
                flySpeed * Time.deltaTime);
            flyingCoin.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);

            yield return null;
        }
        
        flyingCoin.gameObject.SetActive(false);
    }
}
