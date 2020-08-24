using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vertex
{
    [SerializeField]
    public int ID;
    [SerializeField]
    public Vector2 Pos;
    [SerializeField]
    public List<int> Conns;    
    [SerializeField]
    public VertexState State;
    [SerializeField]
    public string Floor;
    [SerializeField]
    public int Rot;

    public Vertex(Vector2 pos)
    {
        ID = VertexSystem.Vertices.Count;
        Pos = pos;
        Conns = new List<int>();
        State = VertexState.Available;
        Floor = "None";
        Rot = 0;
    }

    public Vertex(Vector2 pos, VertexState state)
    {
        ID = VertexSystem.Vertices.Count;
        Pos = pos;
        Conns = new List<int>();
        State = state;
        Floor = "None";
        Rot = 0;
    }

    public void UpdateCons()
    {
        Vertex v = VertexSystem.Vertices.Find(u => u.Pos == new Vector2(Pos.x - 0.25f, Pos.y));
        if (v != null)
        {
            if (!Conns.Contains(v.ID))
            {
                Conns.Add(v.ID);
                v.UpdateCons();
            }
        }
        v = VertexSystem.Vertices.Find(u => u.Pos == new Vector2(Pos.x, Pos.y - 0.25f));
        if (v != null)
        {
            if (!Conns.Contains(v.ID))
            {
                Conns.Add(v.ID);
                v.UpdateCons();
            }
        }
        v = VertexSystem.Vertices.Find(u => u.Pos == new Vector2(Pos.x, Pos.y + 0.25f));
        if (v != null)
        {
            if (!Conns.Contains(v.ID))
            {
                Conns.Add(v.ID);
                v.UpdateCons();
            }
        }
        v = VertexSystem.Vertices.Find(u => u.Pos == new Vector2(Pos.x + 0.25f, Pos.y));
        if (v != null)
        {
            if (!Conns.Contains(v.ID))
            {
                Conns.Add(v.ID);
                v.UpdateCons();
            }
        }
    }
}

public enum VertexState
{
    Available,
    Walkable,
    Occuppied
}