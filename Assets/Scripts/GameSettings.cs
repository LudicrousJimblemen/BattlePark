using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public string nickname = "";

    public GameSettings() { }

    public void CopyValues(GameSettings original)
    {
        nickname = original.nickname;
    }
}