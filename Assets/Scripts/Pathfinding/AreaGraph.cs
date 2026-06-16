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
        if (start == null || end == null)
        {
            return null;
        }
        if (start == end)
        {
            return new List<AreaNode> { start };
        }
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
    private float GetFCost(
    AreaNode node,
    Dictionary<AreaNode, float> gCost,
    AreaNode end)
    {
        float g = gCost[node];

        float h = Vector3.Distance(
            node.area.position,
            end.area.position);

        return g + h;
    }
    private AreaNode GetLowestFCostNode(List<AreaNode> openList,Dictionary<AreaNode, float> gCost,AreaNode end)
    {
        AreaNode bestNode = openList[0];
        float bestFCost = GetFCost(bestNode, gCost, end);

        for (int i = 1; i < openList.Count; i++)
        {
            AreaNode node = openList[i];
            float fCost = GetFCost(node, gCost, end);

            if (fCost < bestFCost)
            {
                bestNode = node;
                bestFCost = fCost;
            }
        }

        return bestNode;
    }
    public List<AreaNode> FindPathAStar(AreaNode start, AreaNode end)
    {
        if (start == null || end == null)
        {
            return null;
        }
        if (start == end)
        {
            return new List<AreaNode> { start };
        }
        List<AreaNode> openList = new();
        HashSet<AreaNode> closedSet = new();

        Dictionary<AreaNode, AreaNode> cameFrom = new();
        Dictionary<AreaNode, float> gCost = new();

        openList.Add(start);
        gCost[start] = 0f;

        while (openList.Count > 0)
        {
            AreaNode current = GetLowestFCostNode(
                openList,
                gCost,
                end);

            if (current == end)
            {
                return BuildPath(cameFrom, start, end);
            }

            openList.Remove(current);
            closedSet.Add(current);

            foreach (var next in current.connections)
            {
                if (next == null)
                    continue;

                if (closedSet.Contains(next.targetArea))
                    continue;

                float newGCost =
                    gCost[current] +
                    Vector3.Distance(
                        current.area.position,
                        next.targetArea.area.position);

                if (!gCost.ContainsKey(next.targetArea) ||
                    newGCost < gCost[next.targetArea])
                {
                    cameFrom[next.targetArea] = current;
                    gCost[next.targetArea] = newGCost;

                    if (!openList.Contains(next.targetArea))
                    {
                        openList.Add(next.targetArea);
                    }
                }
            }
        }

        return null;
    }
}
