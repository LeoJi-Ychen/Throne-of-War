using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Button_BackToStartScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Back);
    }

    void Back()
    {
        SceneManager.LoadSceneAsync("StartScreen");
    }
}
