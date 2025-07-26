using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    [SerializeField] private float groundLength = 113f;
    private static float lastZSpawn = 0f;
    private bool hasSpawned = false;

    private void OnEnable()
    {
        hasSpawned = false; // Reset mỗi lần object được bật lại
        lastZSpawn = 0f; // Reset vị trí spawn mỗi lần object được bật lại
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            hasSpawned = true;

            Vector3 newSpawnPos = new Vector3(0f, 0f, lastZSpawn + groundLength);
            GameObject newGround = GroundPool.Instance.GetGround();
            newGround.transform.position = newSpawnPos;

            lastZSpawn = newSpawnPos.z;
        }
    }
}
