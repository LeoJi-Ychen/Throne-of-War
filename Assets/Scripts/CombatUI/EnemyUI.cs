using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject bloodLine;
    GameObject player;
    float length_bloodLine;
    bool init;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        length_bloodLine = bloodLine.GetComponent<RectTransform>().rect.width;
        init = true;
    }
    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player");
        canvas.SetActive(true);
        if (sqdistance(player) > 144)
        {
            canvas.SetActive(false);
        }
    }
    private void OnDisable()
    {
         canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (sqdistance(player) > 144)
        {
            canvas.SetActive(false);
        }
        else
        {
            if (GetComponent<Enemy>().nearDeath)
            {
                canvas.SetActive(false);
            }
            else
            {
                canvas.SetActive(true);
            }
        }
        if (init)
        {
            int blood = GetComponent<Enemy>().blood;
            int maxblood = GetComponent<Enemy>().maxblood;
            if (maxblood > 0)
            {
                RectTransform rt_blood = bloodLine.GetComponent<RectTransform>();
                rt_blood.sizeDelta = new Vector2(((float)(blood)) / (maxblood) * length_bloodLine, rt_blood.sizeDelta.y);
            }
        }
    }
    float distance(GameObject obj)
    {
        float res = (obj.transform.position - transform.position).magnitude;
        return res;
    }
    float sqdistance(GameObject obj)
    {
        float res = (obj.transform.position - transform.position).sqrMagnitude;
        return res;
    }
}
