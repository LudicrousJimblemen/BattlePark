using System;

public class LobbyClient
{
    public Guid Guid { get; private set; }
    public string Name { get; private set; }

    public LobbyClient(Guid guid, string name)
    {
        Guid = guid;
        Name = name;
    }
}