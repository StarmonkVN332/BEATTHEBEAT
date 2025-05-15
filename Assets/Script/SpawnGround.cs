using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    [SerializeField] private GroundPool groundPool;
    [SerializeField] private float vectorSpawn = 113f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Check"))
        {
            GameObject ground = groundPool.GetGround();
            ground.transform.position = new Vector3(0, 0, vectorSpawn);
            ground.transform.rotation = Quaternion.identity;

            StartCoroutine(DeactivateGround(ground));
        }
    }

    IEnumerator DeactivateGround(GameObject ground)
    {
        yield return new WaitForSeconds(30f);
        groundPool.ReturnGround(ground);
    }
}

