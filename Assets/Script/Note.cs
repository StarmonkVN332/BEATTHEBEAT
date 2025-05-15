using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int move = -5;  
    void Update()
    {
        transform.position += new Vector3(0, 0, move) * Time.deltaTime;
    }
}
