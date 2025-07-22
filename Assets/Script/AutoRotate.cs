using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationSpeed = 30f; // độ xoay mỗi giây

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
