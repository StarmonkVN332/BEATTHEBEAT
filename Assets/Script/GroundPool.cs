using System.Collections.Generic;
using UnityEngine;

public class GroundPool : MonoBehaviour
{
    public static GroundPool Instance;

    [Header("Ground Settings")]
    public GameObject groundPrefab;
    public int initialPoolSize = 5;

    private Queue<GameObject> poolground = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Khởi tạo pool sẵn
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(groundPrefab);
            obj.SetActive(false);
            poolground.Enqueue(obj);
        }
    }

    public GameObject GetGround()
    {
        if (poolground.Count > 0)
        {
            GameObject obj = poolground.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(groundPrefab);
    }

    public void ReturnGround(GameObject obj)
    {
        obj.SetActive(false);
        poolground.Enqueue(obj);
    }
}
