using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] public bool _DisplayGridGizmous;
    [SerializeField] private LayerMask _obstacles;
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeRadius;
    [SerializeField] TerrainType[] _walkableRegions;

    private LayerMask _walkableMask;
    private Dictionary<int, int> _walkableRegionsDictionary = new Dictionary<int, int>();
    private Node[,] _grid;
    private float _nodeDiametor;
    private int _gridSizeX, _gridSizeY;


    private void Awake()
    {
        _nodeDiametor = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiametor);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiametor);

        foreach (TerrainType region in _walkableRegions)
        {
            _walkableMask.value += region.terrainMask.value;
            _walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPanalty);
        }
         
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

                int movmentPenalty = 10;

                if (walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, _walkableMask))
                    {
                        _walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movmentPenalty);
                    }
                }

                _grid[x, y] = new Node(walkable, worldPoint, x, y, movmentPenalty);
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

        if (_DisplayGridGizmous && _grid != null)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = node.Walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiametor - .1f));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPanalty;
    }
}
