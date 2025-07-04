using UnityEngine;
using System.Collections;

public class GameSceneSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnDelay = 1f; // Adjustable in Inspector

    void Start()
    {
        StartCoroutine(SpawnWithDelay());
    }

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        GameObject selected = PlayerManager.Instance.GetSelectedPrefab();
        Instantiate(selected, spawnPoint.position, spawnPoint.rotation);
    }
}
