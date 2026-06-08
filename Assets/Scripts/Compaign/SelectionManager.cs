using UnityEngine;
using UnityEngine.InputSystem;
public class SelectionManager : MonoBehaviour
{
    public Camera mainCamera;
    Vector3 worldPos;
    bool isValid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            GetGroundPosition();
            if (isValid)
            {
                isValid = false;
                SetUnitsAimPos();
            }            
        }
    }
    private void GetGroundPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        int groundLayerMask = LayerMask.GetMask("Ground");
        GameObject aim = null;
        if (Physics.Raycast(ray, out RaycastHit h, 1000f))
        {
            aim = h.collider.gameObject;
        }
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayerMask))
        {
            if(aim == hit.collider.gameObject)
            {
                worldPos = hit.point;
                isValid = true;
            }       
        }
    }
    void SetUnitsAimPos()
    {
        foreach(GameObject g in Unit.CurrentSelection)
        {
            if(g != null)
            {
                g.GetComponent<PlayerUnitMove>().action = true;
                g.GetComponent<PlayerUnitMove>().targetLocation = worldPos;
            }
        }
    }
}
