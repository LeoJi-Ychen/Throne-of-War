using UnityEngine;
using UnityEngine.InputSystem;

public class Commander : MonoBehaviour
{
    public GameObject audio_0;
    public GameObject audio_1;
    public GameObject audio_charge;
    public int OrderID;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OrderID != 1)
        {
            timer += Time.deltaTime;
            if( timer > 15)
            {
                OrderID = 1;
                audio_1.GetComponent<AudioSource>().Play();
                audio_charge.GetComponent<AudioSource>().Play();
            }
            if (Keyboard.current.digit1Key.isPressed)
            {
                OrderID = 1;
                audio_0.GetComponent<AudioSource>().Play();
                audio_1.GetComponent<AudioSource>().Play();
                audio_charge.GetComponent<AudioSource>().Play();
            }
        }
    }
}
