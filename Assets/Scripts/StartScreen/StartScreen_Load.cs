using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartScreen_Load : MonoBehaviour
{
    string keyName = "saved";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Load);
        if (!PlayerPrefs.HasKey(keyName))
        {
            PlayerPrefs.SetInt(keyName, 0);
        }
        int saved = PlayerPrefs.GetInt(keyName);
        if (saved == 0)
        {
            GetComponent<Button>().interactable = false;
            if (GetComponent<Effect_ButtonText>())
            {
                GetComponent<Effect_ButtonText>().enabled = false;
            }
        }
    }

    void Load()
    {
        SceneManager.LoadSceneAsync("MainGame");
    }
}
