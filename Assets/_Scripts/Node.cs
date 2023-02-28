using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Node Parent;
    private int _movmentPenalty;
    private bool _walkable;
    private Vector3 _worldPosition;
    private int _gCost;
    private int _hCost;
    private int _heapIndex;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY, int penalty)
    {
        _walkable = walkable;
        _worldPosition = worldPosition;
        _movmentPenalty = penalty;
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
    public int MovmentPenalty
    {
        get { return _movmentPenalty; }
        set { _movmentPenalty = value; }
    }

    public int HeapIndex
    {
        get 
        {
            return _heapIndex;
        } 
        set
        {
            _heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if(compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }
        return -compare;
    }
}
