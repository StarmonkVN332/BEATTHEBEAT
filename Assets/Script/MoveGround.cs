using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    [SerializeField] private int groundMove = 5;
    
    void Update()
    {
        GroundMove();
    }

    private void GroundMove()
    {
        transform.position += new Vector3(0, 0, groundMove) * Time.deltaTime;
        Destroy(gameObject, 15f);
    }
}
