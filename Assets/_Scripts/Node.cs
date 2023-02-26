using UnityEngine;

public class Node
{
    public Node Parent;
    private bool _walkable;
    private Vector3 _worldPosition;
    private int _gCost;
    private int _hCost;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        _walkable = walkable;
        _worldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    public int GridX { get; private set; }
    public int GridY { get; private set; }
    public bool Walkable
    {
        get { return _walkable; }
        set { _walkable = value; }
    }
    public Vector3 WorldPosition
    {
        get { return _worldPosition; }
        set { _worldPosition = value; }
    }
    public int GCost
    {
        get { return _gCost; }
        set { _gCost = value; }
    }
    public int HCost
    {
        get { return _hCost; }
        set { _hCost = value; }
    }
    public int FCost
    {
        get { return _gCost + _hCost; }
    }
}
