using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tile;

public class Node: IHeapItem<Node> 
{
    public Vector2 coordinates;
    public int index;
    public List<Vector2> neighborsVector;
    public List<int> neighborsIndex;

    public bool isWalkable = true;
    public List<Node> neighborNodes;

    public int gCost;
    public int hCost;
    int heapIndex;

    public Node(Vector2 _coordinates, int _index)
    {
        coordinates = _coordinates;
        index = _index;
    }

    public int fCost { get { return gCost + hCost; } }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
