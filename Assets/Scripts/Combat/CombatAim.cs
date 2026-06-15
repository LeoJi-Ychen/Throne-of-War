using UnityEngine;
using UnityEngine.UI;

public class CombatAim : MonoBehaviour
{
    public Button next;
    private void Awake()
    {
        Time.timeScale = 0;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        next.onClick.AddListener(StartBattle);
    }

    // Update is called once per frame
    void StartBattle()
    {
        Time.timeScale = 1;
        CursorController.HideCursor();
        this.gameObject.SetActive(false);
    }
}
