using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // Customers
    public static List<Customer> Customers;

    public class Customer
    {
        public GameObject Body;
        public GameObject Objective;
        public Stack<Vector2> Path;

        public Customer(GameObject body)
        {
            Body = body;
            Objective = GameObject.Find("End");
            Path = Route(body.transform.position, Objective.transform.position);
        }

        public void RemoveCustomer()
        {
            Destroy(Body);
            Customers.Remove(this);
        }
    }

    // Vertices and vertices information
    public static List<Vertex> Vertices;

    public class Vertex
    {
        public Vector2 Pos;
        public List<int> Conns;

        public Vertex(Vector2 pos)
        {
            Pos = pos;
            Conns = new List<int>();
        }
    }

    void Start()
    {
        Vertices = new List<Vertex>();

        // Create vertices net
        for (int i = -16; i < 10; i++)
        {
            for (int j = -5; j < 14; j++)
            {
                Vertices.Add(new Vertex(new Vector2(i, j)));
            }
        }

        // Connect the 4 possible vertices
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vector2 pos = Vertices[i].Pos;

            int vertexIndex;
            
            vertexIndex = Vertices.IndexOf(Vertices.Find(v => v.Pos == new Vector2(pos.x - 1, pos.y)));
            if (vertexIndex != -1) Vertices[i].Conns.Add(vertexIndex);
            vertexIndex = Vertices.IndexOf(Vertices.Find(v => v.Pos == new Vector2(pos.x, pos.y - 1)));
            if (vertexIndex != -1) Vertices[i].Conns.Add(vertexIndex);
            vertexIndex = Vertices.IndexOf(Vertices.Find(v => v.Pos == new Vector2(pos.x, pos.y + 1)));
            if (vertexIndex != -1) Vertices[i].Conns.Add(vertexIndex);
            vertexIndex = Vertices.IndexOf(Vertices.Find(v => v.Pos == new Vector2(pos.x + 1, pos.y)));
            if (vertexIndex != -1) Vertices[i].Conns.Add(vertexIndex);
        }


        int lol = Vertices.IndexOf(Vertices.Find(v => v.Pos == new Vector2(-15, -4)));
        if (lol != -1) Debug.Log(Vertices[lol].Conns[0]);
        lol = Vertices.IndexOf(Vertices.Find(v => v.Pos == new Vector2(9, 12)));
        if (lol != -1) Debug.Log(Vertices[lol].Conns[0]);

        Customers = new List<Customer>();
        Customers.Add(new Customer((GameObject)Instantiate(Resources.Load("Farm land"), new Vector2(-15, -4), Quaternion.Euler(0, 0, 0))));
        Customers.Add(new Customer((GameObject)Instantiate(Resources.Load("Farm land"), new Vector2(-15, -2), Quaternion.Euler(0, 0, 0))));
    }

    void Update()
    {
        foreach (Customer customer in Customers.ToArray())
        {
            if (customer.Path.Count == 0) customer.RemoveCustomer();
            else
            {
                if ((Vector2)customer.Body.transform.position == customer.Path.Peek())
                    customer.Path.Pop();
                else customer.Body.transform.position = Vector2.MoveTowards(customer.Body.transform.position, customer.Path.Peek(), Time.deltaTime * 2);
            }
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

            foreach (int vertex in Vertices[thisIndex].Conns)
            {
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