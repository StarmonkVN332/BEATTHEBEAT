using UnityEngine;

public class FloatingManager : MonoBehaviour
{
    public Transform[] floatingObjects;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float[] baseYs;
    private float[] offsets;
    private float[] freqs;

    void Start()
    {
        int count = floatingObjects.Length;
        baseYs = new float[count];
        offsets = new float[count];
        freqs = new float[count];

        for (int i = 0; i < count; i++)
        {
            baseYs[i] = floatingObjects[i].position.y;
            offsets[i] = Random.Range(0f, 2f * Mathf.PI);
            freqs[i] = frequency * Random.Range(0.8f, 1.2f);
        }
    }

    void Update()
    {
        for (int i = 0; i < floatingObjects.Length; i++)
        {
            float newY = baseYs[i] + Mathf.Sin(Time.time * freqs[i] + offsets[i]) * amplitude;
            Vector3 pos = floatingObjects[i].position;
            pos.y = newY;
            floatingObjects[i].position = pos;
        }
    }
}
