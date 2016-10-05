using System.Collections;
using UnityEngine;

public class ClientLeftMessage : LobbyMessage
{
    public System.Guid ClientGuid { get; private set; }

    public ClientLeftMessage(System.Guid clientGuid)
    {
        ClientGuid = clientGuid;
    }
}