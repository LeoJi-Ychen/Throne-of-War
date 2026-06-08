using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    float t;
    private void Awake()
    {
        t = 1;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        t = 1;
    }
    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime;
        t = Mathf.Max(0, t);
        Color c = GetComponent<Image>().color;
        c.a = t;
        GetComponent<Image>().color = c;
    }
}
