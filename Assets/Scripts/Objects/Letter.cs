using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : IObject
{
    public string Type;
    public string Title;
    public string Body;
    public bool Read;

    public Letter(string type, string title, string body)
    {
        Type = type;
        Title = title;
        Body = body;
        Read = false;
    }
}