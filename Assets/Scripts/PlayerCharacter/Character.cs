using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject weapon;
    public bool isAttacking;
    public int atkmode;
    public bool isDodge;
    public int maxblood;
    public int blood;
    public int damage;
    public bool hitted;
    public bool nearDeath;
    public bool death;
    public float dead_timer;

    private void Awake()
    {
        maxblood = 1000;
        blood = maxblood;
        damage = 10;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (blood <= 0)
        {
            nearDeath = true;
        }
        if(nearDeath)
        {
            dead_timer += Time.deltaTime;
            if (dead_timer > 3)
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
                weapon.SetActive(false);
                GetComponent<CharacterMove>().enabled = false;
                GetComponent<CharacterAttack>().enabled = false;
                GetComponent<CharacterMouseRotate>().enabled = false;
                death = true;
            }
        }
        weapon.GetComponent<CharacterWeapon>().damage = damage;
        weapon.GetComponent<CharacterWeapon>().isAttack = isAttacking;
        weapon.GetComponent<CharacterWeapon>().atkmode = atkmode;
    }
    public void BeAttacked(int damage)
    {
        if (!isDodge)
        {
            blood -= damage;
            hitted = true;
        }
    }
    public void BeHealed(int heal)
    {
        blood += heal;
    }
    public void EndAttack()
    {
        weapon.GetComponent<CharacterWeapon>().EndAttack();
    }
}
