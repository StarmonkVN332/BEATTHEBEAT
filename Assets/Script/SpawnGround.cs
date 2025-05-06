using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private float vectorSpawn = 37f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Check"))
        {
            Instantiate(ground, new Vector3(0, 0, vectorSpawn), Quaternion.identity);
        }
    }
}
