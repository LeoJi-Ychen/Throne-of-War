using UnityEngine;

public class Disappear : MonoBehaviour
{
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 10)
        {
            this.gameObject.SetActive(false);
        }
    }
}
