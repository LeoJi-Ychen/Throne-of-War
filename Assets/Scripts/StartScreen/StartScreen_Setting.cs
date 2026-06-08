using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartScreen_Setting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Setting);
    }

    void Setting()
    {
        SceneManager.LoadSceneAsync("Setting");
    }
}
