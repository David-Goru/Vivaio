using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 2.5f;

    void Update()
    {
        transform.Translate(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * Speed, Input.GetAxis("Vertical") * Time.deltaTime * Speed));
    }
}