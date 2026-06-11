using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchPerspective : MonoBehaviour
{
    public static bool isFP;
    public GameObject cam;    
    public GameObject cam_first;
    public GameObject cam_third;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFP = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Switch();
        }
        if (isFP)
        {
            cam.transform.position = cam_first.transform.position;
        }
    }
    void Switch()
    {
        isFP = !isFP;

        if (isFP)
        {            
            cam.transform.position = cam_first.transform.position;
            cam.GetComponent<Camera>().nearClipPlane = 0.03f;
        }
        else
        {
            cam.transform.position = cam_third.transform.position;
            cam.GetComponent<Camera>().nearClipPlane = 0.2f;
        }
    }
}
