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
            WorldData data = new WorldData();
            data = WorldData.LoadStructFromJson();
            bool contain = false;
            int index = -1;
            for (int i = 0; i < data.playertroops.Count; i++)
            {
                if (data.playertroops[i].id == ID)
                {
                    contain = true;
                    index = i;
                    break;
                }
            }
            if (contain)
            {
                controller.enabled = false;
                transform.position = data.playertroops[index].pos;
                troopState = data.playertroops[index].state;
                controller.enabled = true;
            }
            else
            {
                this.gameObject.SetActive(false);
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
