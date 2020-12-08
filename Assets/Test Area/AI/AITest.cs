using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITest : MonoBehaviour
{
    public GameObject Ann;
    public float AnnSpeed;
    public Stack<Vector2> AnnPath;

    // We don't want thrash
    Vector2 mousePos;

    void Start()
    {
        if (VertexSystem.New()) Debug.Log("Vertex system initialized");
        AnnPath = new Stack<Vector2>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            AnnPath = VertexSystem.FindPath(Ann.transform.position, mousePos);
        }
        if (AnnPath.Count > 0)
        {
            if ((Vector2)Ann.transform.position == AnnPath.Peek()) AnnPath.Pop();
            else Ann.transform.position = Vector2.MoveTowards(Ann.transform.position, AnnPath.Peek(), Time.deltaTime * AnnSpeed);
        }
    }
}