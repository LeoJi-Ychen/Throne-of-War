using UnityEngine;
using System.Collections.Generic;
public class AreaNode : MonoBehaviour
{
    public Transform area;
    public Vector2 areaSize;
    public GameObject points;
    public List<AreaConnection> connections = new();

    [System.Serializable]
    public class AreaConnection
    {
        public Transform linkPoint;
        public AreaNode targetArea;     
    }
    private void Awake()
    {
        int count = points.transform.childCount;
        for(int i = 0; i < count; i++)
        {
            AreaConnection connection = new();
            connection.linkPoint = points.transform.GetChild(i);
            connection.targetArea = connection.linkPoint.GetComponent<AreaLinkPoint>().nextNode;
            connections.Add(connection);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            area.position,
            new Vector3(areaSize.x, 1f, areaSize.y));

        foreach (var c in connections)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(
                c.linkPoint.position,
                0.5f);
        }    
    }
    public bool IsPointInArea(Vector3 point)
    {
        float minX = area.position.x - areaSize.x * 0.5f;
        float maxX = area.position.x + areaSize.x * 0.5f;

        float minZ = area.position.z - areaSize.y * 0.5f;
        float maxZ = area.position.z + areaSize.y * 0.5f;

        return point.x >= minX &&
               point.x <= maxX &&
               point.z >= minZ &&
               point.z <= maxZ;
    }
}
