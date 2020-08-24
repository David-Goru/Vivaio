using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static bool IsRunning;
    public float Speed;
    public float RunSpeed;
    public static string LastDirection = "IdleDown";
    Animator controller;

    void Start()
    {
        PlayerControls.DoingAnim = false;
        controller = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (PlayerControls.DoingAnim) return;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            if (LastDirection.StartsWith("Idle")) return;

            if (LastDirection.EndsWith("Down")) resetAnimation(LastDirection, "IdleDown");
            else if (LastDirection.EndsWith("Up")) resetAnimation(LastDirection, "IdleUp");
            else if (LastDirection.EndsWith("Right")) resetAnimation(LastDirection, "IdleRight");
            else if (LastDirection.EndsWith("Left")) resetAnimation(LastDirection, "IdleLeft");

            return;
        }

        string mode = IsRunning ? "Run" : "Walk";
        string direction = Input.GetAxis("Horizontal") == 0 ? (Input.GetAxis("Vertical") > 0 ? "Up" : "Down") : (Input.GetAxis("Horizontal") > 0 ? "Right" : "Left");
        string newDirection = mode + direction;

        if (LastDirection != newDirection) resetAnimation(LastDirection, newDirection);
    }

    void FixedUpdate()
    {
        if (PlayerControls.DoingAnim) return;
        else if (Input.GetAxis("Horizontal") != 0)
        {
            if (IsRunning) transform.Translate(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * RunSpeed, 0));
            else transform.Translate(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * Speed, 0));
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            if (IsRunning) transform.Translate(new Vector2(0, Input.GetAxis("Vertical") * Time.deltaTime * RunSpeed));
            else transform.Translate(new Vector2(0, Input.GetAxis("Vertical") * Time.deltaTime * Speed));
        }
    }

    void resetAnimation(string lastDirection, string newDirection)
    {
        if (!lastDirection.StartsWith("Anim")) controller.ResetTrigger(lastDirection);
        controller.SetTrigger(newDirection);
        LastDirection = newDirection;
    }
}