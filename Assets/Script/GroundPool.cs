using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPool : MonoBehaviour
{
    [Header("Ground")]
    public GameObject groundPrefab;
    private Queue<GameObject> poolground = new Queue<GameObject>();

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
