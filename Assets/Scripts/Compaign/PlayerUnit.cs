using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class PlayerUnit : MonoBehaviour
{
    public static int pid;
    public int ID;
    public int troopState;
    private CharacterController controller;
    public static List<GameObject> AllPlayerUnit = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ID = pid;
        pid++;
        controller = GetComponent<CharacterController>();
        if (WarManager.HasData)
        {
            
            if(ID>= WarManager.data.playtroops.Count)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                controller.enabled = false;
                transform.position = WarManager.data.playtroops[ID];
                troopState = WarManager.data.playtroopsState[ID];
                controller.enabled = true;
            }
            
        }
    }
    private void OnEnable()
    {
        if (!AllPlayerUnit.Contains(this.gameObject))
        {
            AllPlayerUnit.Add(this.gameObject);
        }
    }
    private void OnDisable()
    {
        AllPlayerUnit.Remove(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
