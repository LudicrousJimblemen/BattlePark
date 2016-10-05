using System.Collections;
using UnityEngine;

public class SettingsChangedMessage : LobbyMessage
{
    public LobbySettings NewLobbySettings { get; private set; }

    public SettingsChangedMessage(LobbySettings newLobbySettings)
    {
        NewLobbySettings = newLobbySettings;
    }
}