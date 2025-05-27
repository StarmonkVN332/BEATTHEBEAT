using UnityEngine;
using UnityEngine.UI;

public class MusicPreviewController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip previewClip;
    public Button previewButton;
    public GameObject recordDisk;

    private bool isPlaying = false;

    void Start()
    {
        previewButton.onClick.AddListener(TogglePreview);
        // KHÔNG ẩn đĩa nữa
    }

    void TogglePreview()
    {
        if (isPlaying)
            StopPreview();
        else
            PlayPreview();
    }

    void PlayPreview()
    {
        audioSource.clip = previewClip;
        audioSource.Play();
        isPlaying = true;
    }

    void StopPreview()
    {
        audioSource.Stop();
        isPlaying = false;
    }

    void Update()
    {
        if (isPlaying)
        {
            recordDisk.transform.Rotate(0f, 0f, -180f * Time.deltaTime);
        }
    }
}
