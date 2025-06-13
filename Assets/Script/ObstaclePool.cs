using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    public static ObstaclePool Instance;

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public int initialPoolSize = 100;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObstacle()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(obstaclePrefab);
    }

    public void ReturnObstacle(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
