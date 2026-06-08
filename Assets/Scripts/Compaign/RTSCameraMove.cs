using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCameraMove : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float borderSize = 40f;

    public float zoomSpeed = 10f;
    public float minHeight = 10f;
    public float maxHeight = 60f;

    public float lowMinX = -80f;
    public float lowMaxX = 80f;
    public float lowMinZ = -80f;
    public float lowMaxZ = 80f;

    public float highMinX = -50f;
    public float highMaxX = 50f;
    public float highMinZ = -50f;
    public float highMaxZ = 50f;

    private void Update()
    {
        HandleEdgeMove();
        HandleZoom();
        ClampCameraPosition();
    }

    private void HandleEdgeMove()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        float xMove = 0f;
        float zMove = 0f;

        if (mousePos.x <= borderSize)
        {
            xMove = -GetEdgeStrength(mousePos.x);
        }
        else if (mousePos.x >= Screen.width - borderSize)
        {
            xMove = GetEdgeStrength(Screen.width - mousePos.x);
        }

        if (mousePos.y <= borderSize)
        {
            zMove = -GetEdgeStrength(mousePos.y);
        }
        else if (mousePos.y >= Screen.height - borderSize)
        {
            zMove = GetEdgeStrength(Screen.height - mousePos.y);
        }

        Vector3 moveDir = new Vector3(xMove, 0f, zMove);

        transform.position +=
            moveDir *
            moveSpeed *
            Time.deltaTime;
    }

    private float GetEdgeStrength(float distanceToEdge)
    {
        return 1f - Mathf.Clamp01(distanceToEdge / borderSize);
    }

    private void HandleZoom()
    {
        if (Mouse.current == null)
            return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        Vector3 pos = transform.position;

        pos.y -= scroll * zoomSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);

        transform.position = pos;
    }

    private void ClampCameraPosition()
    {
        Vector3 pos = transform.position;

        float t = Mathf.InverseLerp(minHeight, maxHeight, pos.y);

        float currentMinX = Mathf.Lerp(lowMinX, highMinX, t);
        float currentMaxX = Mathf.Lerp(lowMaxX, highMaxX, t);
        float currentMinZ = Mathf.Lerp(lowMinZ, highMinZ, t);
        float currentMaxZ = Mathf.Lerp(lowMaxZ, highMaxZ, t);

        pos.x = Mathf.Clamp(pos.x, currentMinX, currentMaxX);
        pos.z = Mathf.Clamp(pos.z, currentMinZ, currentMaxZ);
        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);

        transform.position = pos;
    }
}