using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using System;

[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(PathRequestManager))]
public class Pathfinding : MonoBehaviour
{

    private PathRequestManager _pathRequestManager;
    private Grid _grid;

    private void Awake()
    {
        _pathRequestManager = GetComponent<PathRequestManager>();
        _grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    private IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _grid.GetNodeFromeWorldPoint(startPosition);
        Node targetNode = _grid.GetNodeFromeWorldPoint(targetPosition);

        if (startNode.Walkable && targetNode.Walkable)
        {
            Heap<Node> openList = new Heap<Node>(_grid.maxSize);
            HashSet<Node> closeList = new HashSet<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList.RemoveFirst();
                closeList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print($"Path found: {sw.ElapsedMilliseconds} ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closeList.Contains(neighbour)) continue;

                    int newMovementCostToNeighbour = currentNode.GCost + GetMovementCost(currentNode, neighbour) + neighbour.MovmentPenalty;
                    if (newMovementCostToNeighbour < neighbour.GCost || !openList.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetMovementCost(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openList.Contains(neighbour))
                            openList.Add(neighbour);
                        else openList.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) waypoints = RetracePath(startNode, targetNode);
        _pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    private int GetMovementCost(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        int resalt = distX > distY
            ? 14 * distY + 10 * (distX - distY)
            : 14 * distX + 10 * (distY - distX);

        return resalt;
    }
}
