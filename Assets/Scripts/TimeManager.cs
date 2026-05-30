using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float gameSpeed = 1;
    private const string GameSpeedrKey = "GameSpeed";
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
        Load();
    }
    
    void Initialize()
    {
        if (!PlayerPrefs.HasKey(GameSpeedrKey))
        {
            PlayerPrefs.SetFloat(GameSpeedrKey, gameSpeed);
        }
    }
    void Load()
    {
        gameSpeed = PlayerPrefs.GetFloat(GameSpeedrKey);
    }
}
