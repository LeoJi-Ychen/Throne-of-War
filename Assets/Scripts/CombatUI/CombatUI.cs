using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class CombatUI : MonoBehaviour
{
    GameObject player;
    public TextMeshProUGUI playerForce;
    public TextMeshProUGUI enemyForce;
    public GameObject Win;
    public GameObject Lose;
    public GameObject bloodLine;
    public GameObject playerMoraleLine;
    public GameObject enemyMoraleLine;
    public GameObject playerForceLine;
    public GameObject enemyForceLine;
    public GameObject tips;
    public GameObject Morale_Bravery;
    public GameObject Morale_Broken;
    public GameObject skill;
    public GameObject battledisplay;
    float length_bloodLine;
    float length_playerForceLine;
    float length_enemyForceLine;
    int morale;
    bool init;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        length_bloodLine = bloodLine.GetComponent<RectTransform>().rect.width;
        length_playerForceLine = playerForceLine.GetComponent<RectTransform>().rect.width;
        length_enemyForceLine = enemyForceLine.GetComponent<RectTransform>().rect.width;
        Morale_Broken.SetActive(false);
        Morale_Bravery.SetActive(false);
        init = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!init)
        {
            return;
        }
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
            return;
        }
        if (player.GetComponent<Character>().single)
        {
            skill.SetActive(true);
        }
        else
        {
            skill.SetActive(false);
        }
        if (player.GetComponent<Commander>().OrderID != 1)
        {
            tips.SetActive(true);
        }
        else
        {
            tips.SetActive(false);
        }
        morale = player.GetComponent<CombatManager>().battleSituation;
        if (morale < -50)
        {
            Morale_Broken.SetActive(true);
            Morale_Bravery.SetActive(false);
        }
        else if (morale > 50)
        {
            Morale_Broken.SetActive(false);
            Morale_Bravery.SetActive(true);
        }
        int maxpf = Mathf.Max(0, player.GetComponent<CombatManager>().maxplayerforces);
        int maxef = Mathf.Max(0, player.GetComponent<CombatManager>().maxemyforces);
        int pf = Mathf.Max(0, player.GetComponent<CombatManager>().playerforces);
        int ef = Mathf.Max(0, player.GetComponent<CombatManager>().emyforces);
        int blood = player.GetComponent<Character>().blood;
        int maxblood = player.GetComponent<Character>().maxblood;
        if (maxblood > 0)
        {
            RectTransform rt_blood = bloodLine.GetComponent<RectTransform>();
            rt_blood.sizeDelta = new Vector2(((float)(blood)) / (maxblood) * length_bloodLine, rt_blood.sizeDelta.y);
        }      
        //player
        playerForce.text = "Player Force: " + pf + "/" + maxpf;
        RectTransform rt_pf = playerMoraleLine.GetComponent<RectTransform>();
        rt_pf.sizeDelta = new Vector2(((float)(pf + 1)) / (pf + ef + 2) * length_playerForceLine, rt_pf.sizeDelta.y);
        playerForceLine.GetComponent<Image>().fillAmount = (float)(pf) / Mathf.Max(maxpf, 1);
        //enemy
        enemyForce.text = "Enemy Force: " + ef + "/" + maxef;
        RectTransform rt_ef = enemyMoraleLine.GetComponent<RectTransform>();
        rt_ef.sizeDelta = new Vector2(((float)(ef + 1)) / (pf + ef + 2) * length_enemyForceLine, rt_ef.sizeDelta.y);
        enemyForceLine.GetComponent<Image>().fillAmount = (float)(ef) / Mathf.Max(maxef, 1);
        if (player.GetComponent<CombatManager>().gameRes == 1)
        {
            Win.SetActive(true);
            battledisplay.SetActive(false);
        }
        else if (player.GetComponent<CombatManager>().gameRes == 2)
        {
            Lose.SetActive(true);
            battledisplay.SetActive(false);
        }
    }
}
