using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vertex : IHeapItem<Vertex>
{
    [SerializeField]
    public Vector2 Pos; 
    [SerializeField]
    public VertexState State;
    [SerializeField]
    public int GridX;
    [SerializeField]
    public int GridY;
    [SerializeField]
    public string Floor;
    [SerializeField]
    public string FloorType;

    // Pathfinding
    public Vertex Parent;
    public int GCost;
    public int HCost;
    int heapIndex;

    public Vertex(Vector2 pos, VertexState state, int gridX, int gridY)
    {
        Pos = pos;
        State = state;
        GridX = gridX;
        GridY = gridY;
        Floor = "None";
        FloorType = "None";
    }

	public int FCost
    {
		get
        {
			return GCost + HCost;
		}
	}

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

    public int CompareTo(Vertex vertexToCompare)
    {
        int compare = FCost.CompareTo(vertexToCompare.FCost);
        if (compare == 0) compare = HCost.CompareTo(vertexToCompare.HCost);
        return -compare;
    }

    public int GetPenalty()
    {
        if (State == VertexState.Walkable) return 0;
        else if (FloorType != "None") return 3;
        else return 6;
    }
}

public enum VertexState
{
    Available,
    Walkable,
    Occuppied
}