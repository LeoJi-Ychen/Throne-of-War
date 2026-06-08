using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectUnit : MonoBehaviour
{
    public List<GameObject> SelectedUnits = new();
    public RectTransform selectionBox;
    public float borderThickness = 2f;
    public Color borderColor = Color.green;

    public Camera mainCamera;

    private RectTransform topLine;
    private RectTransform bottomLine;
    private RectTransform leftLine;
    private RectTransform rightLine;

    private Vector2 startMousePos;
    private Vector2 currentMousePos;
    private bool isDragging;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        CreateBorderLines();

        if (selectionBox != null)
        {
            selectionBox.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelection();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionBox();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            EndSelection();
            SelectedUnits = new List<GameObject>(Unit.CurrentSelection);
        }
    }

    private void CreateBorderLines()
    {
        if (selectionBox == null)
            return;

        Image bgImage = selectionBox.GetComponent<Image>();

        if (bgImage != null)
        {
            bgImage.color = new Color(0, 0, 0, 0);
            bgImage.raycastTarget = false;
        }

        topLine = CreateLine("TopLine");
        bottomLine = CreateLine("BottomLine");
        leftLine = CreateLine("LeftLine");
        rightLine = CreateLine("RightLine");
    }

    private RectTransform CreateLine(string name)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.transform.SetParent(selectionBox, false);

        Image img = lineObj.AddComponent<Image>();
        img.color = borderColor;
        img.raycastTarget = false;

        RectTransform rect = lineObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        return rect;
    }

    private void StartSelection()
    {
        isDragging = true;

        startMousePos = Mouse.current.position.ReadValue();
        currentMousePos = startMousePos;

        if (selectionBox != null)
        {
            selectionBox.gameObject.SetActive(true);
            selectionBox.position = startMousePos;
            selectionBox.sizeDelta = Vector2.zero;
            UpdateBorderLines(0, 0);
        }
    }

    private void UpdateSelectionBox()
    {
        if (!isDragging || selectionBox == null)
        {
            return;
        }

        currentMousePos = Mouse.current.position.ReadValue();

        Vector2 center = (startMousePos + currentMousePos) * 0.5f;

        float width = Mathf.Abs(currentMousePos.x - startMousePos.x);
        float height = Mathf.Abs(currentMousePos.y - startMousePos.y);

        selectionBox.position = center;
        selectionBox.sizeDelta = new Vector2(width, height);

        UpdateBorderLines(width, height);

        SelectUnitsInBox();
    }

    private void UpdateBorderLines(float width, float height)
    {
        if (topLine == null || bottomLine == null || leftLine == null || rightLine == null)
            return;

        topLine.anchoredPosition = new Vector2(0, height * 0.5f);
        topLine.sizeDelta = new Vector2(width, borderThickness);

        bottomLine.anchoredPosition = new Vector2(0, -height * 0.5f);
        bottomLine.sizeDelta = new Vector2(width, borderThickness);

        leftLine.anchoredPosition = new Vector2(-width * 0.5f, 0);
        leftLine.sizeDelta = new Vector2(borderThickness, height);

        rightLine.anchoredPosition = new Vector2(width * 0.5f, 0);
        rightLine.sizeDelta = new Vector2(borderThickness, height);
    }

    private void EndSelection()
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;

        if (selectionBox != null)
        {
            selectionBox.gameObject.SetActive(false);
        }

        SelectUnitsInBox();
    }

    private void SelectUnitsInBox()
    {
        Rect selectionRect = GetScreenRect(startMousePos, Mouse.current.position.ReadValue());

        foreach (Unit unit in Unit.AllUnits)
        {
            if (unit == null)
                continue;

            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPos.z < 0)
            {
                unit.Deselect();
                continue;
            }

            if (selectionRect.Contains(screenPos))
            {
                unit.Select();
            }
            else
            {
                unit.Deselect();
            }
        }
    }

    private Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        float xMin = Mathf.Min(start.x, end.x);
        float yMin = Mathf.Min(start.y, end.y);

        float width = Mathf.Abs(start.x - end.x);
        float height = Mathf.Abs(start.y - end.y);

        return new Rect(xMin, yMin, width, height);
    }
}