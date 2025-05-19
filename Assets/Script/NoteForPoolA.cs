using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteForPoolA : MonoBehaviour
{
    [Header("Note")]
    public GameObject noteAPrefabs;
    private Queue<GameObject> poolAnote = new Queue<GameObject>();


    public GameObject GetNoteA()
    {
        while (poolAnote.Count > 0)
        {
            GameObject noteA = poolAnote.Dequeue();

            if (noteA != null)
            {
                noteA.SetActive(true);
                return noteA;
            }
        }

        return Instantiate(noteAPrefabs);
    }

    public void ReturnNoteA(GameObject noteA)
    {
        noteA.SetActive(false);
        poolAnote.Enqueue(noteA);
    }
}
