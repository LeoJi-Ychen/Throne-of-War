using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime;
    private GUIStyle fpsStyle;
    public bool test;
    void Awake()
    {
        if (test)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1;
        }
        fpsStyle = new GUIStyle();
        fpsStyle.fontSize = 30;
        fpsStyle.normal.textColor = Color.yellow;
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        float fps = 1.0f / deltaTime;

        GUI.Label(
            new Rect(10, 150, 200, 40),
            $"FPS: {fps:F1}",fpsStyle);
    }
}