using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public ObjectPooler leftWallPool;
    public ObjectPooler rightWallPool;
    public Transform player;

    public float spacing = 2f;
    public float spawnDistance = 10f;

    private float lastSpawnY;

    private void Start()
    {
        lastSpawnY = player.position.y;
    }

    private void Update()
    {
        while (player.position.y + spawnDistance > lastSpawnY)
        {
            Vector3 leftPos = new Vector3(-2.5f, lastSpawnY, 0f);
            Vector3 rightPos = new Vector3(2.5f, lastSpawnY, 0f);

            leftWallPool.GetObject(leftPos);
            rightWallPool.GetObject(rightPos);

            lastSpawnY += spacing;
        }
    }
}
