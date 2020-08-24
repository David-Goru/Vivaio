using UnityEngine;

[System.Serializable]
public class DeliveryPoint
{
    [SerializeField]
    public Vector2 Pos;
    [SerializeField]
    public bool Available;

    public DeliveryPoint(Vector2 pos)
    {
        Pos = pos;
        Available = true;
    }
}