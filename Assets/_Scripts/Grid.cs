using UnityEngine;

public class Grid : MonoBehaviour
{
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
                _grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    private Node GetNodeFromeWorldPoint(Vector3 worldPosition)
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

        if (_grid != null)
        {
            foreach (Node node in _grid)
            {
                Gizmos.color = node.Walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiametor - .1f));
            }
        }
    }
}
