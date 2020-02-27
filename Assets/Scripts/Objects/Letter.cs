using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : IObject
{
    public string Type;
    public string Title;
    public string Body;
    public string Signature;
    public bool Read;

    public Letter(string type, string title, string body, string signature)
    {
        Type = type;
        Title = title;
        Body = body;
        Signature = signature;
        Read = false;
    }
}