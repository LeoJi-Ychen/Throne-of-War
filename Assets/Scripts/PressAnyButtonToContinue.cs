using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PressAnyButtonToContinue : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "StartScreen";
    float alpha = 0.6f;
    int state;

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame ||
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }
        if (state == 0)
        {
            alpha += Time.deltaTime;
            if (alpha >= 1)
            {
                state = 1;
                alpha = 1;
            }
        }
        else
        {
            alpha -= Time.deltaTime;
            if (alpha <= 0.1f)
            {
                state = 0;
                alpha = 0.1f;
            }
        }
        Color cl = GetComponent<Image>().color;
        cl.a = alpha;
        GetComponent<Image>().color = cl;
    }
}
