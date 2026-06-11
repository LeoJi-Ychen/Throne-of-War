using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public float delay;
    float timer;
    float t;
    private void Awake()
    {
        t = 1;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        t = 1;
        timer = 0;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > delay)
        {
            t -= Time.deltaTime;
            t = Mathf.Max(0, t);
            Color c = GetComponent<Image>().color;
            c.a = t;
            GetComponent<Image>().color = c;
        }
    }
}
