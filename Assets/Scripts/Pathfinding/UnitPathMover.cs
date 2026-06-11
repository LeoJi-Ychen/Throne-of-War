using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitPathMover : MonoBehaviour
{
    public AreaGraph graph;
    public CharacterController controller;

    public float moveSpeed = 5f;
    public float stopDistance = 0.2f;

    private List<Vector3> movePath;
    private int currentIndex;

    public Camera mainCamera;
    Vector3 worldPos;
    bool isValid;
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    public void SetPath(List<Vector3> path)
    {
        movePath = path;
        currentIndex = 0;
    }

    private void Update()
    {
        AreaNode endNode = null;
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            GetGroundPosition();
            if (isValid)
            {
                isValid = false;
                endNode = graph.FindArea(worldPos);
                if (endNode != null)
                {
                    AreaNode startNode = graph.FindArea(transform.position);
                    List<AreaNode> areaPath = new List<AreaNode>();
                    List<Vector3> path = new List<Vector3>();
                    if (graph.FindPath(startNode, endNode) != null)
                    {
                        areaPath = new List<AreaNode>(graph.FindPath(startNode, endNode));
                    }                   
                    if (areaPath.Count > 0)
                    {
                        path = new List<Vector3>(graph.ConvertAreaPathToWorldPath(areaPath, worldPos));
                        SetPath(path);
                    }
                    else
                    {
                        if (startNode == endNode)
                        {
                            path = new List<Vector3>(graph.ConvertAreaPathToWorldPath(areaPath, worldPos));
                            SetPath(path);
                        }
                    }
                }          
            }
        }      
        MoveAlongPath();
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
            if (aim == hit.collider.gameObject)
            {
                worldPos = hit.point;
                isValid = true;
            }
        }
    }
    private void MoveAlongPath()
    {
        if (movePath == null || currentIndex >= movePath.Count)
            return;

        Vector3 target = movePath[currentIndex];

        Vector3 dir = target - transform.position;
        dir.y = 0;

        if (dir.magnitude <= stopDistance)
        {
            currentIndex++;
            return;
        }

        Vector3 move = dir.normalized * moveSpeed * Time.deltaTime;

        controller.Move(move);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(dir),
            360f * Time.deltaTime);
    }
}