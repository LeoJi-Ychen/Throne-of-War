using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [Header("Fog Texture")]
    public int textureSize = 256;
    public Material fogMaterial;

    [Header("Units")]
    List<Transform> detectUnits = new();
    public float viewRadius = 30f;

    [Header("Update")]
    public float updateInterval = 0.1f;

    private Texture2D fogTexture;
    public bool[,] explored;
    public bool[,] visible;

    private Bounds fogBounds;
    private Vector2 mapSize;

    private static readonly Color VisibleColor =
        new Color(0f, 0f, 0f, 0f);

    private static readonly Color ExploredColor =
        new Color(0f, 0f, 0f, 0.45f);

    private static readonly Color UnexploredColor =
        new Color(1f, 1f, 1f, 0.9f);

    void Start()
    {
        InitFogBounds();
        InitFogTexture();

        InvokeRepeating(
            nameof(UpdateFog),
            0f,
            updateInterval
        );
    }

    void InitFogBounds()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (mr == null)
        {
            Debug.LogError("MeshRenderer");
            return;
        }

        fogBounds = mr.bounds;

        mapSize = new Vector2(
            fogBounds.size.x,
            fogBounds.size.z
        );

        Debug.Log("Fog map size: " + mapSize);
    }

    void InitFogTexture()
    {
        fogTexture = new Texture2D(
            textureSize,
            textureSize,
            TextureFormat.RGBA32,
            false
        );

        fogTexture.wrapMode = TextureWrapMode.Clamp;
        fogTexture.filterMode = FilterMode.Bilinear;

        if (!WarManager.HasData)
        {
            explored = new bool[textureSize, textureSize];
            visible = new bool[textureSize, textureSize];
        }
        if (fogMaterial != null)
        {
            fogMaterial.mainTexture = fogTexture;
        }
        else
        {
            Debug.LogError("fogMaterial");
        }
    }

    void UpdateFog()
    {
        ClearVisible();
        detectUnits.Clear();
        foreach(GameObject obj in Castle.AllCastle)
        {
            detectUnits.Add(obj.transform);
        }
        foreach (GameObject obj in PlayerUnit.AllPlayerUnit)
        {
            detectUnits.Add(obj.transform);
        }
        foreach (Transform unit in detectUnits)
        {
            if (unit == null)
                continue;

            Vector3 centerPoint =
                GetUnitProjectPointOnFogPlane(unit);

            Vector2 texPos =
                WorldToTex(centerPoint);

            RevealCircle(texPos);
        }

        ApplyFogTexture();
    }

    void ClearVisible()
    {
        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                visible[x, y] = false;
            }
        }
    }

    void RevealCircle(Vector2 texPos)
    {
        int radius =
            Mathf.RoundToInt(
                viewRadius / mapSize.x * textureSize
            );

        int centerX = Mathf.RoundToInt(texPos.x);
        int centerY = Mathf.RoundToInt(texPos.y);

        int radiusSqr = radius * radius;

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y > radiusSqr)
                    continue;

                int px = centerX + x;
                int py = centerY + y;

                if (px < 0 || px >= textureSize ||
                    py < 0 || py >= textureSize)
                    continue;

                visible[px, py] = true;
                explored[px, py] = true;
            }
        }
    }

    void ApplyFogTexture()
    {
        for (int x = 0; x < textureSize; x++)
        {
            for (int y = 0; y < textureSize; y++)
            {
                if (visible[x, y])
                {
                    fogTexture.SetPixel(x, y, VisibleColor);
                }
                else if (explored[x, y])
                {
                    fogTexture.SetPixel(x, y, ExploredColor);
                }
                else
                {
                    fogTexture.SetPixel(x, y, UnexploredColor);
                }
            }
        }

        fogTexture.Apply();
    }

    Vector2 WorldToTex(Vector3 worldPos)
    {
        float u =
            (worldPos.x - fogBounds.min.x)
            / fogBounds.size.x;

        float v =
            (worldPos.z - fogBounds.min.z)
            / fogBounds.size.z;

        u = 1f - u;
        v = 1f - v;

        return new Vector2(
            u * textureSize,
            v * textureSize
        );
    }

    Vector3 GetUnitProjectPointOnFogPlane(Transform unit)
    {
        Vector3 pos = unit.position;
        //pos = new Vector3(pos.x,pos.y, pos.z-transform.position.y * Mathf.Tan(30 * Mathf.Deg2Rad));

        return pos;
    }

    void OnDisable()
    {
        CancelInvoke(nameof(UpdateFog));
    }
    public bool IsWorldVisible(Vector3 worldPos)
    {
        Vector2 texPos = WorldToTex(worldPos);

        int x = Mathf.FloorToInt(texPos.x);
        int y = Mathf.FloorToInt(texPos.y);

        if (x < 0 || x >= textureSize ||
            y < 0 || y >= textureSize)
        {
            return false;
        }

        return visible[x, y];
    }
}