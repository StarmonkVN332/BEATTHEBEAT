using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteForPoolB : MonoBehaviour
{
    [Header("Note")]
    public GameObject noteBPrefabs;
    private Queue<GameObject> poolBnote = new Queue<GameObject>();


    public GameObject GetNoteB()
    {
        while (poolBnote.Count > 0)
        {
            GameObject noteB = poolBnote.Dequeue();

            if (noteB != null)
            {
                noteB.SetActive(true);
                return noteB;
            }
        }

        return Instantiate(noteBPrefabs);
    }

    public void ReturnNoteB(GameObject noteB)
    {
        noteB.SetActive(false);
        poolBnote.Enqueue(noteB);
    }
}
