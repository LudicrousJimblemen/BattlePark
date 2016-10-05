using System.Collections.Generic;
using UnityEngine;

public enum Keybind
{
    Chat
}

[System.Serializable]
public class KeybindCollection
{
    public Dictionary<Keybind, KeyCode> keybinds = new Dictionary<Keybind, KeyCode>();

    public KeybindCollection()
    {
        keybinds.Add(Keybind.Chat, KeyCode.Return);
    }

    public KeyCode this[Keybind b]
    {
        get
        {
            return keybinds[b];
        }
        set
        {
            keybinds[b] = value;
        }
    }

    public void CopyValues(KeybindCollection original)
    {
        keybinds = new Dictionary<Keybind, KeyCode>(original.keybinds);
    }
}