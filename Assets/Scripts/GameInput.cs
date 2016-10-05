using UnityEngine;
using UnityEngine.EventSystems;

public static class GameInput
{

    public static string GetKeyCodeName(KeyCode kc)
    {
        string n = kc.ToString();
        //Insert spaces before capital letters
        for (var i = 0; i < n.Length; i++)
        {
            if (i != 0 && !char.IsLower(n[i]) && char.IsLower(n[i - 1]))
            {
                n = n.Substring(0, i) + " " + n.Substring(i);
                //Increment i to account for the newly inserted space
                i++;
            }
        }

        return n;
    }

    public static bool IsOpeningChat()
    {
        return Input.GetKeyDown(ActiveData.Keybinds[Keybind.Chat]);
    }
}