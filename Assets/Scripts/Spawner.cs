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
    private float nextSpawnY = 0f;

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

        Vector3 platformPos = new Vector3(Random.Range(minX, maxX), nextSpawnY, 0f);
        platformPool.GetFromPool(platformPos, Quaternion.identity);

        if (Random.value < 0.5f)
        {
            //return;

            Vector3 spikePos = platformPos + new Vector3(0f, 0.5f, 0f);
            spikePool.GetFromPool(spikePos, Quaternion.identity);
        }

        if (Random.value < 0.3f)
        {
            //return;
            Vector3 bladePos = new Vector3(Random.Range(minX, maxX), nextSpawnY + Random.Range(1f, 2f), 0f);
            bladePool.GetFromPool(bladePos, Quaternion.identity);
        }

        int coinCount = Random.Range(1, 2);
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 coinPos = platformPos + new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), 0f);
            coinPool.GetFromPool(coinPos, Quaternion.identity);
        }
    }
}
