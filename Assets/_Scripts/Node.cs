using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Node Parent;
    public bool Walkable;

    private Vector3 _worldPosition;
    private int _movmentPenalty, _heapIndex, _gCost, _hCost;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY, int penalty)
    {
        Walkable = walkable;
        GridX = gridX;
        GridY = gridY;

        _worldPosition = worldPosition;
        _movmentPenalty = penalty;
    }

    public int GridX { get; private set; }
    public int GridY { get; private set; }
    public Vector3 WorldPosition
    {
        get { return _worldPosition; }
        set { _worldPosition = value; }
    }
    public int GCost
    {
        get { return _gCost; }
        set
        {
            if (value > 0)
                _gCost = value;
        }
    }
    public int HCost
    {
        get { return _hCost; }
        set
        {
            if (value > 0)
                _hCost = value;
        }
    }
    public int FCost
    {
        get { return _gCost + _hCost; }
    }
    public int MovmentPenalty
    {
        get { return _movmentPenalty; }
        set
        {
            if (value > 0)
                _movmentPenalty = value;
        }
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

        if (compare == 0)
            compare = HCost.CompareTo(nodeToCompare.HCost);

        return -compare;
    }
}
