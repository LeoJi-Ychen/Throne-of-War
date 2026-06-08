using UnityEngine;
using UnityEngine.UI;

public class StartScreen_Exit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Exit);
    }

    void Exit()
    {
        Application.Quit();
    }
}
