using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Default Volume")]
    [Range(0f, 1f)] public float defaultMasterVolume = 1f;
    [Range(0f, 1f)] public float defaultMusicVolume = 0.8f;
    [Range(0f, 1f)] public float defaultSFXVolume = 1f;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeVolume();
        LoadVolume();
    }

    private void InitializeVolume()
    {
        if (!PlayerPrefs.HasKey(MasterKey))
        {
            PlayerPrefs.SetFloat(MasterKey, defaultMasterVolume);
        }

        if (!PlayerPrefs.HasKey(MusicKey))
        {
            PlayerPrefs.SetFloat(MusicKey, defaultMusicVolume);
        }

        if (!PlayerPrefs.HasKey(SFXKey))
        {
            PlayerPrefs.SetFloat(SFXKey, defaultSFXVolume);
        }

        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        SetVolume(MasterKey, value);
        PlayerPrefs.SetFloat(MasterKey, value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(MusicKey, value);
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume(SFXKey, value);
        PlayerPrefs.SetFloat(SFXKey, value);
    }

    private void SetVolume(string parameterName, float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        float db = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat(parameterName, db);
    }

    private void LoadVolume()
    {
        SetVolume(MasterKey, PlayerPrefs.GetFloat(MasterKey));
        SetVolume(MusicKey, PlayerPrefs.GetFloat(MusicKey));
        SetVolume(SFXKey, PlayerPrefs.GetFloat(SFXKey));
    }
}