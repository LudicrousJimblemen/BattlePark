using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

public struct LobbySettings
{
    public bool IsGood { get; set; }

    public static LobbySettings CreateDefault()
    {
        return new LobbySettings()
        {
            IsGood = true
        };
    }
}