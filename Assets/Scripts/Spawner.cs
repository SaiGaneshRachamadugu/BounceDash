using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject spikePrefab;
    public GameObject bladePrefab;
    public GameObject coinPrefab;

    public float spawnIntervalY = 3f;
    public float minX = -2.5f, maxX = 2.5f;
    private float nextSpawnY = 0f;

    void Update()
    {
        if (Camera.main.transform.position.y + 10f > nextSpawnY)
        {
            SpawnLevelChunk();
            nextSpawnY += spawnIntervalY;
        }
    }

    private void SpawnLevelChunk()
    {
        Vector3 platformPos = new Vector3(Random.Range(minX, maxX), nextSpawnY, 0f);
        GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);

        if (Random.value < 0.5f)
        {
            return;
            Vector3 spikePos = platformPos + new Vector3(0f, 0.5f, 0f);
            Instantiate(spikePrefab, spikePos, Quaternion.identity);
        }
        if (Random.value < 0.3f)
        {
            return;
            Vector3 bladePos = new Vector3(Random.Range(minX, maxX), nextSpawnY + Random.Range(1f, 2f), 0f);
            Instantiate(bladePrefab, bladePos, Quaternion.identity);
        }

        int coinCount = Random.Range(1, 2);
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 coinPos = platformPos + new Vector3(Random.Range(-1f, 1f), Random.Range(1f, 2f), 0f);
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }
    }
}
