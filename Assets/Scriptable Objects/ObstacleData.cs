using UnityEngine;

[CreateAssetMenu(menuName = "Game/ObstacleConfig")]
public class ObstacleData : ScriptableObject
{
    public GameObject obstaclePrefab;
    public float spawnRate;
}
