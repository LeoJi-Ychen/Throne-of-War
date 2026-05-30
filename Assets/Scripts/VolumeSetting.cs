using UnityEngine;
using UnityEngine.UI;
public class VolumeSetting : MonoBehaviour
{
    public enum VolumeType
    {
        Master,
        Music,
        SFX
    }

    [Header("Volume Type")]
    public VolumeType volumeType;

    [Header("UI")]
    public Slider slider;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    private void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;

        slider.value = PlayerPrefs.GetFloat(GetKey(), GetDefaultValue());

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (AudioManager.Instance == null)
            return;

        switch (volumeType)
        {
            case VolumeType.Master:
                AudioManager.Instance.SetMasterVolume(value);
                break;

            case VolumeType.Music:
                AudioManager.Instance.SetMusicVolume(value);
                break;

            case VolumeType.SFX:
                AudioManager.Instance.SetSFXVolume(value);
                break;
        }

        PlayerPrefs.Save();
    }

    private string GetKey()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                return MasterKey;

            case VolumeType.Music:
                return MusicKey;

            case VolumeType.SFX:
                return SFXKey;
        }

        return MasterKey;
    }

    private float GetDefaultValue()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                return 1f;

            case VolumeType.Music:
                return 0.8f;

            case VolumeType.SFX:
                return 1f;
        }

        return 1f;
    }
}
