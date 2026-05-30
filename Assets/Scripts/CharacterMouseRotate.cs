using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMouseRotate : MonoBehaviour
{
    [Range(0f, 1f)]
    public float sensitivity = 1f;

    [Header("Perspective")]
    public Transform body;
    public Transform view;

    [Header("Rotate")]
    public float yawSpeed = 360f;
    public float pitchSpeed = 90f;

    [Header("Pitch Limit")]
    public float minPitch = -30f;
    public float maxPitch = 15f;

    private float pitch;
    private const string SensitivityKey = "Sensitivity";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(SensitivityKey))
        {
            PlayerPrefs.SetFloat(SensitivityKey, sensitivity);
        }
        sensitivity = PlayerPrefs.GetFloat(SensitivityKey);

        if (view != null)
        {
            pitch = view.localEulerAngles.x;
            if (pitch > 180f)
            {
                pitch -= 360f;
            }       
        }
    }

    private void Update()
    {
        MouseRotate();
    }

    private void MouseRotate()
    {
        if (Mouse.current == null)
            return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        float mouseX = Mathf.Clamp(delta.x / 500f, -1f, 1f);
        float mouseY = Mathf.Clamp(delta.y / 300f, -1f, 1f);

        body.Rotate(
            0f,
            mouseX * sensitivity * yawSpeed,
            0f,
            Space.Self
        );

        pitch -= mouseY * sensitivity * pitchSpeed;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );

        view.localRotation = Quaternion.Euler(
            pitch,
            0f,
            0f
        );
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat(SensitivityKey, sensitivity);
    }
}