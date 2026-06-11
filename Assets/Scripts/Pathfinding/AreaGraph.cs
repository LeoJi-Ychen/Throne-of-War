using System.Collections.Generic;
using UnityEngine;

public class AreaGraph : MonoBehaviour
{
    public List<AreaNode> allAreas = new();

    public AreaNode FindArea(Vector3 point)
    {
        foreach (AreaNode area in allAreas)
        {
            if (area != null && area.IsPointInArea(point))
            {
                return area;
            }
        }

        return null;
    }
    public List<AreaNode> FindPath(AreaNode start, AreaNode end)
    {
        Queue<AreaNode> queue = new();
        Dictionary<AreaNode, AreaNode> cameFrom = new();
        HashSet<AreaNode> visited = new();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            AreaNode current = queue.Dequeue();

            if (current == end)
            {
                return BuildPath(cameFrom, start, end);
            }
            if (current == null)
            {
                return null;
            }
            foreach (var next in current.connections)
            {
                if (next == null)
                    continue;

                if (visited.Contains(next.targetArea))
                    continue;

                visited.Add(next.targetArea);
                cameFrom[next.targetArea] = current;
                queue.Enqueue(next.targetArea);
            }
        }

        return null;
    }

    private List<AreaNode> BuildPath(
        Dictionary<AreaNode, AreaNode> cameFrom,
        AreaNode start,
        AreaNode end)
    {
        List<AreaNode> path = new();

        AreaNode current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }
    public List<Vector3> ConvertAreaPathToWorldPath(List<AreaNode> areaPath,Vector3 targetPos)
    {
        List<Vector3> worldPath = new();

        for (int i = 0; i < areaPath.Count - 1; i++)
        {
            AreaNode current = areaPath[i];
            AreaNode next = areaPath[i + 1];

            Vector3 link = GetNearestLinkPoint(current, next.area.position);
            worldPath.Add(link);
        }

        worldPath.Add(targetPos);

        return worldPath;
    }

    private Vector3 GetNearestLinkPoint(AreaNode area, Vector3 target)
    {
        Vector3 bestPoint = area.area.position;
        float bestDist = float.MaxValue;

        foreach (var p in area.connections)
        {
            float dist = Vector3.SqrMagnitude(p.linkPoint.position - target);

            if (dist < bestDist)
            {
                bestDist = dist;
                bestPoint = p.linkPoint.position;
            }
        }
        return bestPoint;
    }
}
