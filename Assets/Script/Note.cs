using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int move = -5;
    void Start()
    {
        Destroy(gameObject, 15f); 
    }
    private void Update()
    {
        transform.position += new Vector3(0, 0, move) * Time.deltaTime;
    }
}

