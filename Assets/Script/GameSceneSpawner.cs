using UnityEngine;
using System.Collections;

public class GameSceneSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnDelay = 0f; // Adjustable in Inspector

    private void Start()
    {
        StartCoroutine(SpawnWithDelay());

    }
    private void OnEnable()
    {
        // Start the coroutine to spawn the player after a delay
        StartCoroutine(SpawnWithDelay());
    }
    

    IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(0f);

        GameObject selected = PlayerManager.Instance.GetSelectedPrefab();
        Instantiate(selected, spawnPoint.position, spawnPoint.rotation);
    }
}
