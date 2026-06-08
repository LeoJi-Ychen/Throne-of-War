using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class TextFlash : MonoBehaviour
{
    float alpha = 0.6f;
    int state;

    void Update()
    {
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
        Color cl = GetComponent<TextMeshProUGUI>().color;
        cl.a = alpha;
        GetComponent<TextMeshProUGUI>().color = cl;
    }
}
