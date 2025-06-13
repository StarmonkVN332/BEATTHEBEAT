using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePool : MonoBehaviour
{
    public static NotePool Instance;

    [Header("Note Settings")]
    public GameObject notePrefab;
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
            GameObject obj = Instantiate(notePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetNote()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(notePrefab);
    }

    public void ReturnNote(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

