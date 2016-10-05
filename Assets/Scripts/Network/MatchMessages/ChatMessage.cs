using System.Collections;
using UnityEngine;

public enum ChatMessageType
{
    System,
    User
}

public class ChatMessage : LobbyMessage
{
    public string From { get; private set; }
    public ChatMessageType Type { get; private set; }
    public string Text { get; private set; }

    public ChatMessage(string from, ChatMessageType type, string text)
    {
        From = from;
        Type = type;
        Text = text;
    }
}