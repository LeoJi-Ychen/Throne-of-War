using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    private bool cursorLocked = false;

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        cursorLocked = !cursorLocked;

        if (cursorLocked)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
