using UnityEngine;
using UnityEngine.UI;

public class FirstPersonUI : MonoBehaviour
{
    public Image aiming;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SwitchPerspective.isFP)
        {
            aiming.gameObject.SetActive(true);
        }
        else
        {
            aiming.gameObject.SetActive(false);
        }
        if (CharacterWeapon.hit)
        {
            aiming.color = Color.red;
            timer += Time.deltaTime;
            if(timer > 0.5f)
            {
                timer = 0;
                CharacterWeapon.hit = false;
            }
        }
        else
        {
            aiming.color = Color.white;
        }
    }
}
