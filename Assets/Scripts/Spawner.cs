using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public ObstaclesObjectPool platformPool;
    public ObstaclesObjectPool spikePool;
    public ObstaclesObjectPool bladePool;
    public ObstaclesObjectPool coinPool;

    public Transform parentTransform;

    public float spawnIntervalY = 3f;
    public float minX = -2.5f, maxX = 2.5f;
    private float nextSpawnY = -2f;

    private bool poolsReady = false;
    public float disableThreshold = 10f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(CheckPoolsReady());
    }

    private IEnumerator CheckPoolsReady()
    {
        Debug.Log("Waiting for all pools to be ready...");

        while (!platformPool.IsPoolReady ||
               !spikePool.IsPoolReady ||
               !bladePool.IsPoolReady ||
               !coinPool.IsPoolReady)
        {
            Debug.Log($"Status => Platform: {platformPool.IsPoolReady}, Spike: {spikePool.IsPoolReady}, Blade: {bladePool.IsPoolReady}, Coin: {coinPool.IsPoolReady}");
            yield return null;
        }

        poolsReady = true;
        Debug.Log("All pools ready");
    }


    void Update()
    {
        if (!poolsReady) return;

        float cameraY = Camera.main.transform.position.y;
        //Debug.Log("CamY Spawner : " + cameraY);

        // Spawn new platforms
        if (cameraY + 10f > nextSpawnY)
        {
            SpawnLevelChunk();
            nextSpawnY += spawnIntervalY;
        }

        // Disable off-screen objects (one per frame)

        platformPool.DisableOffscreenObjects(cameraY, disableThreshold);
        spikePool.DisableOffscreenObjects(cameraY, disableThreshold);
        bladePool.DisableOffscreenObjects(cameraY, disableThreshold);
        coinPool.DisableOffscreenObjects(cameraY, disableThreshold);
    }


    private void SpawnLevelChunk()
    {
        Debug.Log("@ Spawning level chunk at Y: " + nextSpawnY);

        // 1. Spawn platform
        Vector3 platformPos = new Vector3(Random.Range(minX, maxX), nextSpawnY, 0f);
        platformPool.GetFromPool(platformPos, Quaternion.identity);

        // 2. Maybe spawn spike (only 50% chance)
        Vector3? spikePos = null;
        if (Random.value < 0.5f)
        {
            spikePos = platformPos + new Vector3(0f, 0.5f, 0f);
            spikePool.GetFromPool(spikePos.Value, Quaternion.identity);
        }

        // 3. Maybe spawn blade (30% chance)
        Vector3? bladePos = null;
        if (Random.value < 0.3f)
        {
            float bladeOffsetX = Random.Range(1.5f, 2.5f); // Blades away from platform
            float bladeX = platformPos.x + (platformPos.x > 0 ? bladeOffsetX : -bladeOffsetX);
            bladeX = Mathf.Clamp(bladeX, minX, maxX);

            bladePos = new Vector3(bladeX, nextSpawnY + Random.Range(2f, 3f), 0f);
            bladePool.GetFromPool(bladePos.Value, Quaternion.identity);
        }

        // 4. Spawn coins
        Vector3 coinPos;
        int coinPositionType = Random.Range(0, 2); // 0 or 1
        if (coinPositionType == 0)
        {
            coinPos = platformPos + new Vector3(Random.Range(-0.5f, 0.5f), 2f, 0f);
        }
        else
        {
            coinPos = platformPos + new Vector3(Random.Range(-1f, 1f), 2f, 0f);
        }

        coinPool.GetFromPool(coinPos, Quaternion.identity);

        // 5. Adjust positions if too close (blade vs others)

        float minSafeDistance = 1.5f; // You can tweak this

        if (bladePos.HasValue)
        {
            // Check against platform
            if (Vector3.Distance(bladePos.Value, platformPos) < minSafeDistance)
            {
                Vector3 dir = (bladePos.Value - platformPos).normalized;
                bladePos = platformPos + dir * minSafeDistance;
            }

            // Check against spike
            if (spikePos.HasValue && Vector3.Distance(bladePos.Value, spikePos.Value) < minSafeDistance)
            {
                Vector3 dir = (bladePos.Value - spikePos.Value).normalized;
                bladePos = spikePos.Value + dir * minSafeDistance;
            }

            // Check against coin
            if (Vector3.Distance(bladePos.Value, coinPos) < minSafeDistance)
            {
                Vector3 dir = (bladePos.Value - coinPos).normalized;
                bladePos = coinPos + dir * minSafeDistance;
            }

            // Update blade position
            // You will need to move the blade object in the scene manually (since Pool returns object instance)
            GameObject bladeObject = bladePool.GetLastSpawned(); // <- You need to implement GetLastSpawned() inside your pooling system
            if (bladeObject != null)
            {
                bladeObject.transform.position = bladePos.Value;
            }
        }
    }

}
