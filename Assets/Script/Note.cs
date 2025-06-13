using UnityEngine;

public class Note : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotePool.Instance.ReturnNote(gameObject);
        }
        if (other.CompareTag("ReturnPool"))
        {
            NotePool.Instance.ReturnNote(gameObject);
        }
        Debug.Log("Note return to pool");
    }
}
