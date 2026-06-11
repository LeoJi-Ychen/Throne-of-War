using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class EndCombat : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "MainGame";
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnEnable()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1.5f)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame ||
           Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadSceneAsync(sceneName);
            }
        }      
    }
}
