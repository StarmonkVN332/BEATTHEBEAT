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
    }

    public void TogglePreview()
    {
        if (isPlaying) StopPreview();
        else PlayPreview();
    }

    public void PlayPreview()
    {
        if (audioSource != null && previewClip != null)
        {
            audioSource.clip = previewClip;
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void StopPreview()
    {
        if (isPlaying && audioSource != null)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    void Update()
    {
        if (isPlaying && recordDisk != null)
        {
            recordDisk.transform.Rotate(0f, 0f, -180f * Time.deltaTime);
        }
    }

    // 🚨 Tự động gọi khi GameObject này hoặc parent bị SetActive(false)
    void OnDisable()
    {
        StopPreview(); // đảm bảo dừng nhạc khi panel bị tắt
    }
}