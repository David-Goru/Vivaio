using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static bool IsRunning;
    public float Speed;
    public float RunSpeed;
    public static string LastDirection = "Idle down";
    Animator controller;

    void Start()
    {
        PlayerTools.DoingAnim = false;
        //transform.Find("Camera").GetComponent<Camera>().orthographicSize = (float)1080 / 64 / 8;
        transform.Find("Camera").GetComponent<Camera>().orthographicSize = (float)Screen.currentResolution.height / 64 / 8;
        controller = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (PlayerTools.DoingAnim) return;

        if (Input.GetAxis("Horizontal") > 0)
        {
            if (IsRunning)
            {
                transform.Translate(new Vector2(Time.deltaTime * RunSpeed, 0));
                if (LastDirection != "Running right")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkRight");
                    LastDirection = "Running right";
                }
            }
            else
            {                
                transform.Translate(new Vector2(Time.deltaTime * Speed, 0));
                if (LastDirection != "Walking right")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkRight");
                    LastDirection = "Walking right";
                }
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (IsRunning)
            {
                transform.Translate(new Vector2(-Time.deltaTime * RunSpeed, 0));
                if (LastDirection != "Running left")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkLeft");
                    LastDirection = "Running left";                
                }
            }
            else
            {   
                transform.Translate(new Vector2(-Time.deltaTime * Speed, 0));
                if (LastDirection != "Walking left")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkLeft");
                    LastDirection = "Walking left";                
                }
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (IsRunning)
            {
                transform.Translate(new Vector2(0, Time.deltaTime * RunSpeed));
                if (LastDirection != "Running up")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkUp");
                    LastDirection = "Running up";
                }
            }
            else
            {
                transform.Translate(new Vector2(0, Time.deltaTime * Speed));
                if (LastDirection != "Walking up")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkUp");
                    LastDirection = "Walking up";
                }
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (IsRunning)
            {
                transform.Translate(new Vector2(0, -Time.deltaTime * RunSpeed));
                if (LastDirection != "Running down")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkDown");
                    LastDirection = "Running down";
                }
            }
            else
            {
                transform.Translate(new Vector2(0, -Time.deltaTime * Speed));
                if (LastDirection != "Walking down")
                {                    
                    switch(LastDirection)
                    {
                        case "Walking down":
                        case "Running down":
                            controller.ResetTrigger("WalkDown");
                            break;
                        case "Walking up":
                        case "Running up":
                            controller.ResetTrigger("WalkUp");
                            break;
                        case "Walking right":
                        case "Running right":
                            controller.ResetTrigger("WalkRight");
                            break;
                        case "Walking left":
                        case "Running left":
                            controller.ResetTrigger("WalkLeft");
                            break;
                        case "Idle down":
                            controller.ResetTrigger("IdleDown");
                            break;
                        case "Idle up":
                            controller.ResetTrigger("IdleUp");
                            break;
                        case "Idle right":
                            controller.ResetTrigger("IdleRight");
                            break;
                        case "Idle left":
                            controller.ResetTrigger("IdleLeft");
                            break;
                    }

                    controller.SetTrigger("WalkDown");
                    LastDirection = "Walking down";
                }
            }
        }
        else
        {
            switch(LastDirection)
            {
                case "Walking down":
                case "Running down":
                case "Anim down":
                    controller.ResetTrigger("WalkDown");
                    controller.SetTrigger("IdleDown");
                    LastDirection = "Idle down";
                    break;
                case "Walking up":
                case "Running up":
                case "Anim up":
                    controller.ResetTrigger("WalkUp");
                    controller.SetTrigger("IdleUp");
                    LastDirection = "Idle up";
                    break;
                case "Walking right":
                case "Running right":
                case "Anim right":
                    controller.ResetTrigger("WalkRight");
                    controller.SetTrigger("IdleRight");
                    LastDirection = "Idle right";
                    break;
                case "Walking left":
                case "Running left":
                case "Anim left":
                    controller.ResetTrigger("WalkLeft");
                    controller.SetTrigger("IdleLeft");
                    LastDirection = "Idle left";
                    break;
            }
        }
    }
}