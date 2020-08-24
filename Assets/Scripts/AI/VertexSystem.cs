using System;
using System.Collections.Generic;
using UnityEngine;

public class VertexSystem : MonoBehaviour
{
    public static List<Vertex> Vertices;

    // When loading a game
    public static bool Load(List<Vertex> vertices)
    {
        try
        {
            Vertices = vertices;

            foreach (Vertex v in vertices)
            {
                if (v.Floor != "None")
                {
                    GameObject floor = Instantiate(Resources.Load<GameObject>("Objects/" + v.Floor), v.Pos, Quaternion.Euler(0, 0, 0));
                    floor.GetComponent<BoxCollider2D>().enabled = true;
                    floor.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<ObjectInfo>("Objects info/" + v.Floor).Sprites[v.Rot];
                }
            }
        }
        catch (Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "VertexSystem", e));
        }
        
        return true;
    }

    // When creating a new game
    public static bool New()
    {
        Transform vertices = GameObject.Find("Initializer").transform.Find("Vertices");
        Vector3 firstV = vertices.Find("First vertex").position;
        Vector3 lastV = vertices.Find("Last vertex").position;

        Vertices = new List<Vertex>();

        // Create vertices net (shop area)
        for (float i = firstV.x; i <= lastV.x; i += 0.25f)
        {
            for (float j = firstV.y; j <= lastV.y; j += 0.25f)
            {
                Vertices.Add(new Vertex(new Vector2(i, j)));
            }
        }

        // Create vertices net (sidewalk)
        foreach (Transform t in vertices.Find("Sidewalk"))
        {
            firstV = t.Find("First vertex").position;
            lastV = t.Find("Last vertex").position;
            for (float i = firstV.x; i <= lastV.x; i += 0.25f)
            {
                for (float j = firstV.y; j <= lastV.y; j += 0.25f)
                {
                    Vertices.Add(new Vertex(new Vector2(i, j), VertexState.Walkable));
                }
            }
        }

        // Update vertices connections
        foreach (Vertex v in Vertices)
        {
            v.UpdateCons();
        }

        // Add farm vertices (that won't be connected)
        Vector3 firstVfarm = vertices.Find("First vertex farm").position;
        Vector3 lastVfarm = vertices.Find("Last vertex farm").position;
        for (float i = firstVfarm.x; i <= lastVfarm.x; i += 0.25f)
        {
            for (float j = firstVfarm.y; j <= lastVfarm.y; j += 0.25f)
            {
                Vertices.Add(new Vertex(new Vector2(i, j)));
            }
        }

        // Create vertices net (sidewalk)
        /*foreach (Transform t in vertices.Find("Sidewalk"))
        {
            Vertices.Add(new Vertex(new Vector2(t.position.x, t.position.y)));
        }

        foreach (Transform t in vertices.Find("Sidewalk"))
        {
            Vertex v = Vertices.Find(x => x.Pos == new Vector2(t.position.x, t.position.y));
            foreach (Transform c in t)
            {
                Vertex vc = Vertices.Find(x => x.Pos == new Vector2(c.position.x, c.position.y));
                v.Conns.Add(vc.ID);
                vc.Conns.Add(v.ID);
            }
        }*/

        Vector3 firstVhouse = vertices.Find("First vertex house").position;
        Vector3 lastVhouse = vertices.Find("Last vertex house").position;
        
        for (float i = firstVhouse.x; i <= lastVhouse.x; i += 0.25f)
        {
            for (float j = firstVhouse.y; j <= lastVhouse.y; j += 0.25f)
            {
                VertexSystem.Vertices.Add(new Vertex(new Vector2(i, j)));
            }
        }

        Vector3 firstVbasement = vertices.Find("First vertex basement").position;
        Vector3 lastVbasement = vertices.Find("Last vertex basement").position;
        
        for (float i = firstVbasement.x; i <= lastVbasement.x; i += 0.25f)
        {
            for (float j = firstVbasement.y; j <= lastVbasement.y; j += 0.25f)
            {
                VertexSystem.Vertices.Add(new Vertex(new Vector2(i, j)));
            }
        }

        return true;
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

            foreach (int v in Vertices[thisIndex].Conns)
            {
                Vertex vx = Vertices.Find(x => x.ID == v);
                int vertex = Vertices.IndexOf(vx);
                if (!visited[vertex])
                {
                    visited[vertex] = true;
                    if (Vertices[vertex].Pos == lastPos) pathFinished = true;
                    if (vx.State == VertexState.Available || vx.State == VertexState.Walkable)
                    {
                        father[vertex] = thisIndex;
                        box.Enqueue(vertex);
                    }
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

    public static Stack<Vector2> Route(Vector2 firstPos, List<Vector2> lastPos)
    {
        Stack<Vector2> bestRoute = new Stack<Vector2>();

        if (lastPos.Contains(firstPos))
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
        int lastPosIndex = -1;

        while (box.Count != 0 && !pathFinished)
        {
            int thisIndex = box.Peek();
            box.Dequeue();

            foreach (int v in Vertices[thisIndex].Conns)
            {
                Vertex vx = Vertices.Find(x => x.ID == v);
                int vertex = Vertices.IndexOf(vx);
                if (!visited[vertex])
                {
                    visited[vertex] = true;
                    
                    if (vx.State == VertexState.Available || vx.State == VertexState.Walkable)
                    {
                        father[vertex] = thisIndex;
                        box.Enqueue(vertex);
                    }

                    if (lastPos.Contains(Vertices[vertex].Pos))
                    {
                        pathFinished = true;
                        lastPosIndex = vertex;
                        break;
                    }
                }
            }
        }

        if (box.Count == 0 && !pathFinished)
        {
            Debug.Log("Route not found: from " + firstPos + " to " + lastPos[0]);
            return bestRoute;
        }

        index = lastPosIndex;

        while (index != -1)
        {
            bestRoute.Push(Vertices[index].Pos);
            index = father[index];
        }

        return bestRoute;
    }
}