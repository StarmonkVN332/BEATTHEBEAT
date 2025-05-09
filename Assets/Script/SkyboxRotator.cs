using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    void Update()
    {
        if (RenderSettings.skybox != null)
        {
            float rotation = Time.time * rotationSpeed;
            RenderSettings.skybox.SetFloat("_Rotation", rotation);
        }
    }
}

