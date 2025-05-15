using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePool : MonoBehaviour
{
    [Header("Note")]
    public GameObject notePrefabs;
    private Queue<GameObject> poolnote = new Queue<GameObject>();


    public GameObject GetNote()
    {
        while (poolnote.Count > 0)
        {
            GameObject note = poolnote.Dequeue();

            if (note != null) // ✅ tránh lỗi null (destroyed)
            {
                note.SetActive(true);
                return note;
            }
        }

        return Instantiate(notePrefabs);
    }

    public void ReturnNote(GameObject note)
    {
        note.SetActive(false);
        poolnote.Enqueue(note);
    }
}
