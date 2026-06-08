using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static readonly List<Unit> AllUnits = new List<Unit>();
    public static List<GameObject> CurrentSelection = new List<GameObject>();
    public GameObject selectionCircle;
    public bool IsSelected { get; private set; }

    private void OnEnable()
    {
        if (!AllUnits.Contains(this))
        {
            AllUnits.Add(this);
        }
        if (selectionCircle != null)
        {
            if (IsSelected)
            {
                selectionCircle.SetActive(true);
            }
            else
            {
                selectionCircle.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        AllUnits.Remove(this);
        if (CurrentSelection.Contains(this.gameObject))
        {
            CurrentSelection.Remove(this.gameObject);
        }      
    }

    public void Select()
    {
        IsSelected = true;
        if (!CurrentSelection.Contains(this.gameObject))
        {
            CurrentSelection.Add(this.gameObject);
        }      
        if (selectionCircle != null)
        {
            selectionCircle.SetActive(true);
        }         
    }
    public void Deselect()
    {
        IsSelected = false;
        if (CurrentSelection.Contains(this.gameObject))
        {
            CurrentSelection.Remove(this.gameObject);
        }      
        if (selectionCircle != null)
        {
            selectionCircle.SetActive(false);
        }         
    }
}
