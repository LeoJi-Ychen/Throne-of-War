using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartScreen_NewGame : MonoBehaviour
{
    string keyName = "saved";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(NewGame);
    }

    void NewGame()
    {
        PlayerPrefs.SetInt(keyName, 0);
        SceneManager.LoadSceneAsync("MainGame");
    }
}
