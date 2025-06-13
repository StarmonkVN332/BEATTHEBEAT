using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("obstacle va cham collider");
        if (other.CompareTag("ReturnPool"))
        {
            ObstaclePool.Instance.ReturnObstacle(gameObject);
            Debug.Log("obstacle return to pool");
        }
    }
}
