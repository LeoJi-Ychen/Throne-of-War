using UnityEngine;

public class Castle : MonoBehaviour
{
    public GameObject flag_red;
    public GameObject flag_yellow;
    public int camp;//1 player
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (camp == 0)
        {
            flag_red.SetActive(false);
            flag_yellow.SetActive(true);
        }
        else
        {
            flag_red.SetActive(true);
            flag_yellow.SetActive(false);
        }
    }
}
