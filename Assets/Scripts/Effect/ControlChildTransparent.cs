using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ControlChildTransparent : MonoBehaviour
{
    public GameObject benchmark;
    List<GameObject> childs = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (benchmark == null)
        {
            benchmark = this.gameObject;
        }
        GetChilds(this.gameObject.transform);
        childs.Remove(benchmark);
        SetTransparent();
    }

    // Update is called once per frame
    void Update()
    {
        SetTransparent();
    }
    void SetTransparent()
    {
        float a = 0;
        if (benchmark.GetComponent<Image>())
        {
            a = benchmark.GetComponent<Image>().color.a;
        }
        else if (benchmark.GetComponent<SpriteRenderer>())
        {
            a = benchmark.GetComponent<SpriteRenderer>().color.a;
        }
        else if (benchmark.GetComponent<TextMeshProUGUI>())
        {
            a = benchmark.GetComponent<TextMeshProUGUI>().color.a;
        }

        foreach (GameObject child in childs)
        {
            if (child.GetComponent<Image>())
            {
                Color c = child.GetComponent<Image>().color;
                c.a = a;
                child.GetComponent<Image>().color = c;
            }
            else if (child.GetComponent<SpriteRenderer>())
            {
                Color c = child.GetComponent<SpriteRenderer>().color;
                c.a = a;
                child.GetComponent<SpriteRenderer>().color = c;
            }
            else if (child.GetComponent<TextMeshProUGUI>())
            {
                Color c = child.GetComponent<TextMeshProUGUI>().color;
                c.a = a;
                child.GetComponent<TextMeshProUGUI>().color = c;
            }
        }
    }
    void GetChilds(Transform p)
    {
        if (p.childCount <= 0)
        {
            return;
        }
        for (int i = 0; i < p.childCount; i++)
        {
            childs.Add(p.GetChild(i).gameObject);
            GetChilds(p.GetChild(i));
        }
    }
}
