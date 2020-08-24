using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MailboxData
{
    [SerializeField]
    public Queue<Letter> Letters;
}