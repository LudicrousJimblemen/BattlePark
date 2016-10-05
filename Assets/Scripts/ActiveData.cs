using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class ActiveData : MonoBehaviour
{
    //Pseudo-singleton pattern - this field accesses the current instance.
    private static ActiveData instance;

    //This data is saved to a json file
    private GameSettings gameSettings = new GameSettings();

    private KeybindCollection keybinds = new KeybindCollection();
    private LobbySettings lobbySettings = LobbySettings.CreateDefault();

    public static GameSettings GameSettings { get { return instance.gameSettings; } }
    public static KeybindCollection Keybinds { get { return instance.keybinds; } }
    public static LobbySettings LobbySettings { get { return instance.lobbySettings; } set { instance.lobbySettings = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        LoadAll();
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    public void LoadAll()
    {
        Load("GameSettings.json", ref gameSettings);
        Load("GameKeybinds.json", ref keybinds);
        Load("LobbySettings.json", ref lobbySettings);
    }

    public void SaveAll()
    {
        Save("GameSettings.json", gameSettings);
        Save("GameKeybinds.json", keybinds);
        Save("LobbySettings.json", lobbySettings);
    }

    private void Load<T>(string filename, ref T output)
    {
        string fullPath = Application.persistentDataPath + "/" + filename;
        if (File.Exists(fullPath))
        {
            //Load file contents
            string dataString;
            using (StreamReader sr = new StreamReader(fullPath))
            {
                dataString = sr.ReadToEnd();
            }
            //Deserialize from JSON into a data object
            try
            {
                var dataObj = JsonConvert.DeserializeObject<T>(dataString);
                //Make sure an object was created, this would't end well with a null value
                if (dataObj != null)
                {
                    output = dataObj;
                    Debug.Log(filename + " loaded successfully.");
                }
                else
                {
                    Debug.LogError("Failed to load " + filename + ": file is empty.");
                }
            }
            catch (JsonException ex)
            {
                Debug.LogError("Failed to parse " + filename + "! JSON converter info: " + ex.Message);
            }
        }
        else
        {
            Debug.Log(filename + " has not been loaded - file not found.");
        }
    }

    private void Save(string filename, object objToSave)
    {
        var data = JsonConvert.SerializeObject(objToSave);
        using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + filename))
        {
            sw.Write(data);
        }
        Debug.Log(filename + " saved successfully.");
    }
}