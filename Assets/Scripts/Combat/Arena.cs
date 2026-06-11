using UnityEngine;

public class Arena : MonoBehaviour
{
    public AudioSource herovoice;
    public GameObject hero;
    public GameObject boss;
    public Transform pt;
    public Transform et;
    Vector3 origin_pt;
    Vector3 origin_et;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        herovoice.Play();
        hero = GameObject.FindGameObjectWithTag("Player");
        boss = CombatManager.boss;
        hero.GetComponent<CharacterController>().enabled = false;
        boss.GetComponent<CharacterController>().enabled = false;
        origin_pt = hero.transform.position;
        origin_et = boss.transform.position;
        hero.transform.position = pt.position;
        hero.transform.rotation = pt.rotation;
        boss.transform.position = et.position;
        boss.transform.rotation = et.rotation;
        boss.GetComponent<EnemyBoss>().blood = boss.GetComponent<EnemyBoss>().maxblood;
        hero.GetComponent<Character>().blood = 
            Mathf.Max(hero.GetComponent<Character>().blood,30);
        hero.GetComponent<CharacterController>().enabled = true;
        boss.GetComponent<CharacterController>().enabled = true;
    }

    // Update is called once per frame
    private void OnDisable()
    {
        hero.GetComponent<CharacterController>().enabled = false;
        boss.GetComponent<CharacterController>().enabled = false;
        hero.transform.position = origin_pt;
        boss.transform.position = origin_et;
        hero.GetComponent<CharacterController>().enabled = true;
        boss.GetComponent<CharacterController>().enabled = true;
    }
}
