using UnityEngine;
using System.Collections.Generic;

public class ObstaclesObjectPool : MonoBehaviour
{
    // Prefab to be pooled
    public GameObject prefab;
    public int poolSize = 4;

    // Queue to hold available (inactive) pooled objects
    private Queue<GameObject> pool = new Queue<GameObject>();

    // List to track currently active objects
    public List<GameObject> activeObjects = new List<GameObject>();

    // Last object that was spawned
    private GameObject lastSpawnedObject;

    // Flag to check if pool has been initialized
    public bool IsPoolReady { get; private set; } = false;

    void Start()
    {
        InitializePool();
    }

    // Instantiates and prepares the pool of objects
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

    // Retrieves an object from the pool, activates it, and positions it
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

        lastSpawnedObject = obj;
        return obj;
    }

    // Returns the last spawned object
    public GameObject GetLastSpawned()
    {
        return lastSpawnedObject;
    }

    // Deactivates objects that are far below the camera's current Y position
    public void DisableOffscreenObjects(float cameraY, float threshold = 10f, bool fromCollider = false)
    {
        if (fromCollider)
        {
            Debug.Log($"@Check {prefab.name}");
        }

        for (int i = 0; i < activeObjects.Count; i++)
        {
            if (activeObjects[i].activeSelf && activeObjects[i].transform.position.y < cameraY - threshold)
            {
                activeObjects[i].SetActive(false);
                activeObjects.RemoveAt(i);
                break;
            }
        }
    }

    // Deactivates a specific object after collision and removes it from the active list
    public void DisableCollidedObj(GameObject collidedObj)
    {
        Debug.Log("Removing Obj from list...");
        collidedObj.SetActive(false);
        activeObjects.Remove(collidedObj);
    }
}
