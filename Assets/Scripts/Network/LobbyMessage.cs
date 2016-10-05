using System.Collections;
using UnityEngine;

public delegate void LobbyMessageHandler<T>(T message, float travelTime) where T : LobbyMessage;

public abstract class LobbyMessage
{
    protected bool reliable = true;

    public bool Reliable { get { return reliable; } }
}