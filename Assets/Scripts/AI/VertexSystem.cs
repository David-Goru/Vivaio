using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexSystem : MonoBehaviour
{
    // Vertices and vertices information
    public static List<Vertex> Vertices;

    public class Vertex
    {
        public Vector2 Pos;
        public List<Vertex> Conns;

        public Vertex(Vector2 pos)
        {
            Pos = pos;
            Conns = new List<Vertex>();
        }

        public void UpdateCons()
        {
            Vertex v = Vertices.Find(u => u.Pos == new Vector2(Pos.x - 0.5f, Pos.y));
            if (v != null)
            {
                if (!Conns.Contains(v))
                {
                    Conns.Add(v);
                    Vertices[Vertices.IndexOf(v)].UpdateCons();
                }
            }
            v = Vertices.Find(u => u.Pos == new Vector2(Pos.x, Pos.y - 0.5f));
            if (v != null)
            {
                if (!Conns.Contains(v))
                {
                    Conns.Add(v);
                    Vertices[Vertices.IndexOf(v)].UpdateCons();
                }
            }
            v = Vertices.Find(u => u.Pos == new Vector2(Pos.x, Pos.y + 0.5f));
            if (v != null)
            {
                if (!Conns.Contains(v))
                {
                    Conns.Add(v);
                    Vertices[Vertices.IndexOf(v)].UpdateCons();
                }
            }
            v = Vertices.Find(u => u.Pos == new Vector2(Pos.x + 0.5f, Pos.y));
            if (v != null)
            {
                if (!Conns.Contains(v))
                {
                    Conns.Add(v);
                    Vertices[Vertices.IndexOf(v)].UpdateCons();
                }
            }
        }
    }

    void Start()
    {
        Vertices = new List<Vertex>();

        // Create vertices net (sidewalk)
        foreach (Transform v in transform.Find("Road Vertexs"))
        {
            Vertices.Add(new Vertex(new Vector2(v.position.x, v.position.y)));
        }

        // Update vertices connections
        foreach (Vertex v in Vertices)
        {
            v.UpdateCons();
        }
    }

    // BFS algorithm
    public static Stack<Vector2> Route(Vector2 firstPos, Vector2 lastPos)
    {
        Stack<Vector2> bestRoute = new Stack<Vector2>();

        if (firstPos == lastPos)
        {
            bestRoute.Push(firstPos);
            return bestRoute;
        }

        List<bool> visited = new List<bool>();
        List<int> father = new List<int>();

        for (int i = 0; i < Vertices.Count; i++)
        {
            // Set all vertices to unvisited
            visited.Add(false);

            // Set father to null (-1)
            father.Add(-1);
        }
        
        int index = Vertices.IndexOf(Vertices.Find(v => v.Pos == firstPos));
        visited[index] = true;

        Queue<int> box = new Queue<int>();
        box.Enqueue(index);

        bool pathFinished = false;
        while (box.Count != 0 && !pathFinished)
        {
            int thisIndex = box.Peek();
            box.Dequeue();

            foreach (Vertex v in Vertices[thisIndex].Conns)
            {
                int vertex = Vertices.IndexOf(v);
                if (!visited[vertex])
                {
                    if (Vertices[vertex].Pos == lastPos) pathFinished = true;
                    visited[vertex] = true;
                    father[vertex] = thisIndex;
                    box.Enqueue(vertex);
                }
            }
        }

        if (box.Count == 0 && !pathFinished)
        {
            Debug.Log("Route not found: from " + firstPos + " to " + lastPos);
            return bestRoute;
        }

        index = Vertices.IndexOf(Vertices.Find(v => v.Pos == lastPos));

        while (index != -1)
        {
            bestRoute.Push(Vertices[index].Pos);
            index = father[index];
        }

        return bestRoute;
    }
}