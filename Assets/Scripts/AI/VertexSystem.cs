using System;
using System.Collections.Generic;
using UnityEngine;

public class VertexSystem : MonoBehaviour
{
    public static Vertex[,] GridInfo;
    public LayerMask Buildable;
    public LayerMask OnlyWalkable;

    public static LayerMask BuildableMask;
    public static LayerMask OnlyWalkableMask;
    public static int GridSizeX, GridSizeY;
    public static Vector2 GridWorldSize;
    public static float VertexRadius;
    public static float VertexDiameter;

    void Awake()
    {
        BuildableMask = Buildable;
        OnlyWalkableMask = OnlyWalkable;
        GridWorldSize = transform.GetComponent<BoxCollider2D>().size;
        VertexRadius = 0.125f;
        VertexDiameter = VertexRadius * 2;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / VertexDiameter);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / VertexDiameter);
    }

    // When loading a game
    public static bool Load(Vertex[,] gridInfo)
    {
        try
        {
            GridInfo = gridInfo;

            for (int i = 0; i < GridSizeX; i++)
            {
                for (int j = 0; j < GridSizeY; j++)
                {
                    if (GridInfo[i,j].Floor != "None" && GridInfo[i,j].Floor != "House ground" && GridInfo[i,j].Floor != "Plowed soil")
                    {
                        GameObject floor = Instantiate(Resources.Load<GameObject>("Objects/" + GridInfo[i,j].Floor), GridInfo[i,j].Pos, Quaternion.Euler(0, 0, 0));
                        floor.GetComponent<BoxCollider2D>().enabled = true;

                        if (GridInfo[i,j].Floor == "Composite tile" || GridInfo[i,j].Floor == "Dirt tile")
                            floor.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Objects/" + GridInfo[i,j].Floor + "/" + GridInfo[i,j].FloorType);
                    }
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
        CreateGrid();

        return true;
    }

    public static int MaxGridSize
    {
        get
        {
            return GridSizeX * GridSizeY;
        }
    }

    public static Stack<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        List<Vector2> targetToList = new List<Vector2>();
        targetToList.Add(targetPos);
        return FindPath(startPos, targetToList);
    }

    public static Stack<Vector2> FindPath(Vector2 startPos, List<Vector2> targetPos)
    {
        Vertex startVertex = VertexFromPosition(startPos);
        List<Vertex> targetVertices = new List<Vertex>();

        foreach (Vector2 vPos in targetPos)
        {
            targetVertices.Add(VertexFromPosition(vPos));
        }

        Heap<Vertex> openSet = new Heap<Vertex>(MaxGridSize);
        HashSet<Vertex> closedSet = new HashSet<Vertex>();
        openSet.Add(startVertex);
        startVertex.GCost = 0;

        Vertex currentVertex = null;
        while (openSet.Count > 0)
        {
            currentVertex = openSet.RemoveFirst();
            closedSet.Add(currentVertex);

            if (targetVertices.Contains(currentVertex)) return RetracePath(startVertex, currentVertex);

            foreach (Vertex neighbour in GetNeighbours(currentVertex))
            {
                if (neighbour.State == VertexState.NotUsable || neighbour.State == VertexState.Occuppied || closedSet.Contains(neighbour)) continue;

                int newCostToNeighbour = currentVertex.GCost + GetDistance(currentVertex, neighbour) + neighbour.GetPenalty();
                if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetVertices[0]);
                    neighbour.Parent = currentVertex;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
            }
        }

        // No path found, return empty list
        return new Stack<Vector2>();
    }

    public static Stack<Vector2> RetracePath(Vertex startVertex, Vertex endVertex)
    {
        Stack<Vector2> path = new Stack<Vector2>();
        Vertex currentVertex = endVertex;

        while (currentVertex != startVertex)
        {
            path.Push(currentVertex.Pos);
            currentVertex = currentVertex.Parent;
        }
        
        return path;
    }

    public static int GetDistance(Vertex vertexA, Vertex vertexB)
    {
        int distX = Mathf.Abs(vertexA.GridX - vertexB.GridX);
        int distY = Mathf.Abs(vertexA.GridY - vertexB.GridY);
        
        return 10 * distX + 10 * distY;
    }

    public static void CreateGrid()
    {
        GridInfo = new Vertex[GridSizeX, GridSizeY];
        Vector2 bottomLeft = Vector2.zero - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2;

        Vector2 vertexPosition = Vector2.one;
        VertexState state = VertexState.Available;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                vertexPosition = bottomLeft + Vector2.right * (x * VertexDiameter + VertexRadius) + Vector2.up * (y * VertexDiameter + VertexRadius);
                if (Physics2D.OverlapCircle(vertexPosition, VertexRadius, OnlyWalkableMask)) state = VertexState.Walkable;
                else if (Physics2D.OverlapCircle(vertexPosition, VertexRadius, BuildableMask)) state = VertexState.Available;
                else state = VertexState.NotUsable;
                GridInfo[x, y] = new Vertex(vertexPosition, state, x, y);
            }
        }
    }

    // Expand grid used when upgrading farm on management menu
    public static void ExpandGrid()
    {
        // Create a new grid with 5 more rows
        Vertex[,] newGrid = new Vertex[GridSizeX, GridInfo.GetLength(0) + 5];

        // Set new grid vertex as old grid vertex (excluding new rows)
        for (int i = 0; i < GridSizeX; i++)
        {
            for (int j = 0; j < GridInfo.GetLength(0); j++)
            {
                newGrid[i,j] = GridInfo[i,j];
            }
        }

        // Get bottom left position
        Vector2 bottomLeft = Vector2.zero - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2;

        // Add new vertex for the last 5 rows
        Vector2 vertexPosition = Vector2.one;
        VertexState state = VertexState.Available;
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = GridInfo.GetLength(0); y < GridInfo.GetLength(0) + 5; y++)
            {
                vertexPosition = bottomLeft + Vector2.right * (x * VertexDiameter + VertexRadius) + Vector2.up * (y * VertexDiameter + VertexRadius);
                if (Physics2D.OverlapCircle(vertexPosition, VertexRadius, OnlyWalkableMask)) state = VertexState.Walkable;
                else if (Physics2D.OverlapCircle(vertexPosition, VertexRadius, BuildableMask)) state = VertexState.Available;
                else state = VertexState.NotUsable;
                newGrid[x, y] = new Vertex(vertexPosition, state, x, y);
            }
        }

        // Set old grid to new grid and update size
        GridInfo = newGrid;
        GridWorldSize = new Vector2(GridWorldSize.x, GridWorldSize.y + 5);
    }

    public static List<Vertex> GetNeighbours(Vertex vertex)
    {
        List<Vertex> neighbours = new List<Vertex>();

        // Get top, bottom, right and left neightbours
        if (vertex.GridX >= 0 && vertex.GridX < GridSizeX && (vertex.GridY + 1) >= 0 && (vertex.GridY + 1) < GridSizeY) neighbours.Add(GridInfo[vertex.GridX, vertex.GridY + 1]);
        if (vertex.GridX >= 0 && vertex.GridX < GridSizeX && (vertex.GridY - 1) >= 0 && (vertex.GridY - 1) < GridSizeY) neighbours.Add(GridInfo[vertex.GridX, vertex.GridY - 1]);
        if ((vertex.GridX + 1) >= 0 && (vertex.GridX + 1) < GridSizeX && vertex.GridY >= 0 && vertex.GridY < GridSizeY) neighbours.Add(GridInfo[vertex.GridX + 1, vertex.GridY]);
        if ((vertex.GridX - 1) >= 0 && (vertex.GridX - 1) < GridSizeX && vertex.GridY >= 0 && vertex.GridY < GridSizeY) neighbours.Add(GridInfo[vertex.GridX - 1, vertex.GridY]);

        return neighbours;
    }

    public static Vertex VertexFromPosition(Vector2 pos)
    {
        int x = Mathf.RoundToInt((GridSizeX - 1) * (pos.x + GridWorldSize.x / 2) / GridWorldSize.x);
        int y = Mathf.RoundToInt((GridSizeY - 1) * (pos.y + GridWorldSize.y / 2) / GridWorldSize.y);

        //Debug.Log(GridInfo[x, y].Pos + " should be " + pos);

        return GridInfo[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(GridWorldSize.x, GridWorldSize.y, 1));

        if (GridInfo != null)
        {
            foreach (Vertex n in GridInfo)
            {
                Gizmos.color = (n.State == VertexState.Available ? Color.green : (n.State == VertexState.Walkable ? Color.gray : Color.red));
                Gizmos.DrawCube(n.Pos, Vector3.one * VertexDiameter * 0.4f);
            }
        }
    }
}