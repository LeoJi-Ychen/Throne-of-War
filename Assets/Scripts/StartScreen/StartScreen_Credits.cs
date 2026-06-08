using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartScreen_Credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Credits);
    }

    void Credits()
    {
        SceneManager.LoadSceneAsync("Credits");
    }
}
