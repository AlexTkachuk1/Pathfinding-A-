using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = _grid.GetNodeFromeWorldPoint(startPosition);
        Node targetNode = _grid.GetNodeFromeWorldPoint(targetPosition);

        List<Node> openList = new List<Node>();
        HashSet<Node> closeList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closeList.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = currentNode.GCost + GetMovementCost(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || !openList.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetMovementCost(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode){
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        _grid.Path = path;
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
