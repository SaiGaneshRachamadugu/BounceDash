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
        if (Random.value < 0.5f)
        {
            // Spawn spike ON the platform
            Vector3 spikePos = platformPos + new Vector3(0f, 0.5f, 0f);
            spikePool.GetFromPool(spikePos, Quaternion.identity);
        }

        // 3. Maybe spawn blade (30% chance)
        if (Random.value < 0.3f)
        {
            float bladeOffsetX = Random.Range(1.5f, 2.5f); // Blades away from platform
            float bladeX = platformPos.x + (platformPos.x > 0 ? bladeOffsetX : -bladeOffsetX);
            bladeX = Mathf.Clamp(bladeX, minX, maxX);

            Vector3 bladePos = new Vector3(bladeX, nextSpawnY + Random.Range(2f, 3f), 0f);
            bladePool.GetFromPool(bladePos, Quaternion.identity);
        }

        // 4. Spawn coins
        //int coinCount = Random.Range(1, 2); // 1 or 2 coins
        //for (int i = 0; i < coinCount; i++)
        //{
            
        //}

        Vector3 coinPos;
        int coinPositionType = Random.Range(0, 2); // 0 or 1

        if (coinPositionType == 0)
        {
            // Coin directly above platform
            coinPos = platformPos + new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f);
        }
        else
        {
            // Coin floating higher above platform
            coinPos = platformPos + new Vector3(Random.Range(-1f, 1f), 2f, 0f);
        }

        coinPool.GetFromPool(coinPos, Quaternion.identity);
    }

}
