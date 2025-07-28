using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private AudioMixer audioMixer;

    // Đổi key để không xung đột với âm lượng tổng
    private const string VolumePrefKey = "MusicVolume";
    private const string MixerParameter = "MusicVolume"; // tên exposed trong AudioMixer

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        musicSlider.value = savedVolume;
        SetVolume(savedVolume);

        musicSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(MixerParameter, dB);

        if (musicText != null)
        {
            int percent = Mathf.RoundToInt(value * 100);
            musicText.text = percent + "%";
        }

        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }
}
