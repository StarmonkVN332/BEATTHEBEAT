using UnityEngine;

public class GameSceneSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        GameObject selected = PlayerManager.Instance.GetSelectedPrefab();
        Instantiate(selected, spawnPoint.position, spawnPoint.rotation);
    }
}