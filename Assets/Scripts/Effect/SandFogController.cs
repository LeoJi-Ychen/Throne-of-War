using UnityEngine;

public class SandFogController : MonoBehaviour
{
    public Color fogColor = new Color(0.75f, 0.65f, 0.45f);
    public float fogStart = 40f;
    public float fogEnd = 60f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogStartDistance = fogStart;
        RenderSettings.fogEndDistance = fogEnd;
    }
}