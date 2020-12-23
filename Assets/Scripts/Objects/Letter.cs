using UnityEngine;

[System.Serializable]
public class Letter : IObject
{
    [SerializeField]
    public string Type;
    [SerializeField]
    public string Title;
    [SerializeField]
    public string Body;
    [SerializeField]
    public string Signature;
    [SerializeField]
    public bool Read;

    public Letter(string type, string title, string body, string signature) : base("Letter", "", 1, 1, "LetterClosed")
    {
        Type = type;
        Title = title;
        Body = body;
        Signature = signature;
        Read = false;
    }

    public override Sprite GetUISprite()
    {
        return UI.Sprites[Name + " " + (Read ? "open" : "closed")];
    }
}