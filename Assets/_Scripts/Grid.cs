using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<Node> Path;
    [SerializeField] public bool _onlyDisplayPathGizmous;
    [SerializeField] private LayerMask _obstacles;
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeRadius;

    private Node[,] _grid;
    private float _nodeDiametor;
    private int _gridSizeX, _gridSizeY;


    private void Start()
    {
        _nodeDiametor = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiametor);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiametor);
        CreateGrid();
    }

    public int maxSize
    {
        get
        {
            return _gridSizeX * _gridSizeY;
        }
    }

    private void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.forward * _gridWorldSize.y / 2;


        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiametor + _nodeRadius) + Vector3.forward * (y * _nodeDiametor + _nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, _nodeRadius, _obstacles));
                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    internal List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    internal Node GetNodeFromeWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
        float percentY = (worldPosition.z + _gridWorldSize.y / 2) / _gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return _grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Vector3 size = new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y);
        Gizmos.DrawWireCube(transform.position, size);

        if (_onlyDisplayPathGizmous)
        {
            if (Path != null)
            {
                foreach (var node in Path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiametor - .1f));
                }
            }
        }
        else
        {
            if (_grid != null)
            {
                foreach (Node node in _grid)
                {
                    Gizmos.color = node.Walkable ? Color.white : Color.red;
                    if (Path != null)
                        if (Path.Contains(node))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiametor - .1f));
                }
            }
        }
    }
}
