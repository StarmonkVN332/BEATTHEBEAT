using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private float groundLength = 113f;

    private static float lastZSpawn = 0f;  // giữ vị trí spawn cuối cùng
    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            hasSpawned = true;

            // Tính vị trí spawn mới nối tiếp ground trước
            Vector3 newSpawnPos = new Vector3(0f, 0f, lastZSpawn + groundLength);
            Instantiate(groundPrefab, newSpawnPos, Quaternion.identity);

            // Cập nhật lại vị trí cuối cùng đã spawn
            lastZSpawn = newSpawnPos.z;

            Debug.Log("Spawn ground tại z = " + lastZSpawn);
        }
    }
}
