using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyState
{
    public List<LobbyClientState> Clients { get; private set; }
    public List<LobbyPlayerState> Players { get; private set; }
    public LobbySettings Settings { get; private set; }

    public LobbyState(List<LobbyClientState> clients, List<LobbyPlayerState> players, LobbySettings settings, bool inRace, float curAutoStartTime)
    {
        Clients = clients;
        Players = players;
        Settings = settings;
    }
}

public class LobbyClientState
{
    public System.Guid Guid { get; private set; }
    public string Name { get; private set; }

    public LobbyClientState(System.Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }
}

public class LobbyPlayerState
{
    public System.Guid ClientGuid { get; private set; }
    public bool Ready { get; private set; }

    public LobbyPlayerState(System.Guid clientGuid, bool ready)
    {
        ClientGuid = clientGuid;
        Ready = ready;
    }
}