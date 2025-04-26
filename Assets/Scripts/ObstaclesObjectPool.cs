using UnityEngine;
using System.Collections.Generic;

public class ObstaclesObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 4;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public List<GameObject> activeObjects = new List<GameObject>();

    public bool IsPoolReady { get; private set; } = false;

    void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, Spawner.Instance.parentTransform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        IsPoolReady = true;
    }

    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            Debug.LogWarning($"Pool empty for {prefab.name}");
            return null;
        }

        GameObject obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        activeObjects.Add(obj);
        pool.Enqueue(obj);
        return obj;
    }

    public void DisableOffscreenObjects(float cameraY, float threshold)
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            if (activeObjects[i].activeSelf && activeObjects[i].transform.position.y < cameraY - threshold)
            {
                activeObjects[i].SetActive(false);
                activeObjects.RemoveAt(i);
                break; // Only disable one per frame to avoid heavy load
            }
        }
    }
}
