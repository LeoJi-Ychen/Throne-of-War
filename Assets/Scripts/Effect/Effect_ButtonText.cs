using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Effect_ButtonText : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public List<TextMeshProUGUI> buttonText;
    List<Color> initColor;

    void Awake()
    {
        initColor = new List<Color>();
        if (buttonText.Count<=0)
        {
            buttonText.Add(this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
        for(int i = 0; i < buttonText.Count; i++)
        {
            initColor.Add(buttonText[i].color);
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < buttonText.Count; i++)
        {
            buttonText[i].color = initColor[i];
            //buttonText[i].fontStyle = FontStyles.Normal;
        }       
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttonText.Count; i++)
        {
            buttonText[i].color = Color.white;
            //buttonText[i].fontStyle = FontStyles.Normal;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < buttonText.Count; i++)
        {
            buttonText[i].color = initColor[i];
            //buttonText[i].fontStyle = FontStyles.Normal;
        }
    }
}
