using UnityEngine;

public class Node
{
    private bool _walkable;
    private Vector3 _worldPosition;

    public Node(bool walkable, Vector3 worldPosition)
    {
        _walkable = walkable;
        _worldPosition = worldPosition;
    }

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
}
