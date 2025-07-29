using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloEffect : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 30f;

    [Header("Pulsing")]
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.05f;
    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        RotateHalo();
        PulseHalo();
    }

    void RotateHalo()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    void PulseHalo()
    {
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * scale;
    }
}
