using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public GameObject coinPrefab;
    public Transform player;
    public float spawnDistance = 10f;
    public float spacing = 2f;
    private float lastY;

    private void Start()
    {
        lastY = player.position.y;
    }

    private void Update()
    {
        while (player.position.y + spawnDistance > lastY)
        {
            SpawnObjects();
            lastY += spacing;
        }
    }

    void SpawnObjects()
    {
        Vector3 pos = new Vector3(Random.Range(-2f, 2f), lastY, 0f);

        if (Random.value > 0.5f)
            Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], pos, Quaternion.identity);
        else
            Instantiate(coinPrefab, pos, Quaternion.identity);
    }
}