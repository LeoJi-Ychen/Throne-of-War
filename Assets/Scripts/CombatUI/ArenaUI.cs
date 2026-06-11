using UnityEngine;
using UnityEngine.UI;

public class ArenaUI : MonoBehaviour
{
    public GameObject blood;
    EnemyBoss eb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eb = CombatManager.boss.GetComponent<EnemyBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        blood.GetComponent<Image>().fillAmount = (float)eb.blood / eb.maxblood;
    }
}
