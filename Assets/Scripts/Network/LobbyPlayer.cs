using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class LobbyPlayerEventArgs : EventArgs
{
    public LobbyPlayer Player { get; private set; }
    public bool IsLocal { get; private set; }

    public LobbyPlayerEventArgs(LobbyPlayer player, bool isLocal)
    {
        Player = player;
        IsLocal = isLocal;
    }
}

[Serializable]
public class LobbyPlayer
{
    private Guid clientGuid;
    private DateTime latestMessageTime = DateTime.Now;

    public LobbyPlayer(Guid clientGuid)
    {
        this.clientGuid = clientGuid;
    }

    public Guid ClientGuid { get { return clientGuid; } }
    public bool Ready { get; set; }

    //TODO process gameplay messages
}