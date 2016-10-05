    using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public event EventHandler<LobbyPlayerEventArgs> LobbyPlayerAdded;
    public event EventHandler<LobbyPlayerEventArgs> LobbyPlayerRemoved;
    public event EventHandler LobbySettingsChanged;

    [SerializeField]
    private string lobbySceneName = "Lobby";

    private List<LobbyClient> clients = new List<LobbyClient>();

    private Guid myGuid;

    private List<LobbyPlayer> players = new List<LobbyPlayer>();

    private LobbySettings currentSettings;

    //Lobby countdown timer stuff
    private bool lobbyTimerOn = false;
    private const float lobbyTimerMax = 3;
    private float lobbyTimer = lobbyTimerMax;

    //Auto start timer (Only used in online mode)
    private bool autoStartTimerOn = false;
    private float autoStartTimer = 0;

    //Bools for scene initializing and related stuff
    private bool inLobby = false; //Are we in the lobby or in a race?
    private bool loadingLobby = false;
    private bool loadingStage = false;
    private bool joiningRaceInProgress = false; //If true, RaceManager will be created as if a race was already in progress.
    private bool showSettingsOnLobbyLoad = false; //If true, the lobby settings window will pop up when the lobby scene is entered.

    //Lobby messenger used to send and receive state changes.
    //This will be either a LocalLobbyMessenger or OnlineLobbyMessenger, but each are used the same way.
    private LobbyMessenger messenger;

    private Chat activeChat;

    //Timer used for syncing realtime stuff in online
    private float netUpdateTimer = 0;
    private const int NET_UPDATES_PER_SECOND = 40;

    /// <summary>
    /// True if playing online. Used for enabling online-only behaviour, like the client list and the chat
    /// </summary>
    public bool OnlineMode { get { return messenger is OnlineLobbyMessenger; } }

    /// <summary>
    /// Contains all clients connected to the game. In offline lobbyes this will always only contain one client.
    /// </summary>
    public ReadOnlyCollection<LobbyClient> Clients { get { return clients.AsReadOnly(); } }

    /// <summary>
    /// Contains all players in the game, even ones from other clients in online races
    /// </summary>
    public ReadOnlyCollection<LobbyPlayer> Players { get { return players.AsReadOnly(); } }

    /// <summary>
    /// Current settings for this lobby. On remote clients, this is only used for showing settings on the UI.
    /// </summary>
    public LobbySettings CurrentSettings { get { return currentSettings; } }

    public Guid LocalClientGuid { get { return myGuid; } }

    public bool AutoStartTimerOn { get { return autoStartTimerOn; } }
    public float AutoStartTimer { get { return autoStartTimer; } }

    public void RequestSettingsChange(LobbySettings newSettings)
    {
        messenger.SendMessage(new SettingsChangedMessage(newSettings));
    }

    public void RequestPlayerJoin()
    {
        messenger.SendMessage(new PlayerJoinedMessage(myGuid, ctrlType, initialCharacter));
    }

    public void RequestPlayerLeave()
    {
        messenger.SendMessage(new PlayerLeftMessage(myGuid, ctrlType));
    }

    private void SettingsChangedCallback(SettingsChangedMessage msg, float travelTime)
    {
        currentSettings = msg.NewLobbySettings;
        if (LobbySettingsChanged != null)
            LobbySettingsChanged(this, EventArgs.Empty);
    }

    private void ClientJoinedCallback(ClientJoinedMessage msg, float travelTime)
    {
        clients.Add(new LobbyClient(msg.ClientGuid, msg.ClientName));
        Debug.Log("New client " + msg.ClientName);
    }

    private void ClientLeftCallback(ClientLeftMessage msg, float travelTime)
    {
        //Remove all players added by this client
        List<LobbyPlayer> playersToRemove = players.Where(a => a.ClientGuid == msg.ClientGuid).ToList();
        foreach (LobbyPlayer player in playersToRemove)
        {
            PlayerLeftCallback(new PlayerLeftMessage(player.ClientGuid, player.CtrlType), travelTime);
        }
        //Remove the client
        clients.RemoveAll(a => a.Guid == msg.ClientGuid);
    }

    private void ChatCallback(ChatMessage msg, float travelTime)
    {
        if (activeChat)
            activeChat.ShowMessage(msg.Type, msg.From, msg.Text);
    }

    public void InitLocalLobby()
    {
        currentSettings = ActiveData.LobbySettings;

        messenger = new LocalLobbyMessenger();

        showSettingsOnLobbyLoad = true;
        GoToLobby();
    }

    public void InitOnlineLobby(Lidgren.Network.NetClient client, LobbyState lobbyState)
    {
        //Create existing clients
        foreach (var clientInfo in lobbyState.Clients)
        {
            clients.Add(new LobbyClient(clientInfo.Guid, clientInfo.Name));
        }

        //Create existing players
        foreach (var playerInfo in lobbyState.Players)
        {
            LobbyPlayer p = new LobbyPlayer(playerInfo.ClientGuid, playerInfo.CtrlType, playerInfo.CharacterId);
            p.ReadyToRace = playerInfo.ReadyToRace;
            players.Add(p);

            if (inLobby)
            {
                SpawnLobbyBall(p);
            }
        }

        //Set settings
        currentSettings = lobbyState.Settings;

        //Create messenger
        messenger = new OnlineLobbyMessenger(client);
        ((OnlineLobbyMessenger)messenger).Disconnected += (sender, e) =>
        {
            QuitLobby(e.Reason);
        };

        //Create chat
        activeChat = Instantiate(chatPrefab);
        activeChat.MessageSent += LocalChatMessageSent;

        GoToLobby();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //A messenger should be created by now! Time to create some message listeners
        messenger.CreateListener<SettingsChangedMessage>(SettingsChangedCallback);
        messenger.CreateListener<ClientJoinedMessage>(ClientJoinedCallback);
        messenger.CreateListener<ClientLeftMessage>(ClientLeftCallback);
        messenger.CreateListener<ChatMessage>(ChatCallback);

        //Create this client
        myGuid = Guid.NewGuid();
        messenger.SendMessage(new ClientJoinedMessage(myGuid, ActiveData.GameSettings.nickname));
    }

    private void LocalChatMessageSent(object sender, UI.ChatM essageArgs args)
    {
        LobbyClient myClient = clients.FirstOrDefault(a => a.Guid == myGuid);
        messenger.SendMessage(new ChatMessage(myClient.Name, ChatMessageType.User, args.Text));
    }

    private void Update()
    {
        messenger.UpdateListeners();

        if (lobbyTimerOn && inLobby)
        {
            lobbyTimer -= Time.deltaTime;
            LobbyReferences.Active.CountdownField.text = "Lobby starts in " + Mathf.Max(1f, Mathf.Ceil(lobbyTimer));
        }

        if (autoStartTimerOn && inLobby)
        {
            autoStartTimer = Mathf.Max(0, autoStartTimer - Time.deltaTime);
        }

        if (OnlineMode)
        {
            netUpdateTimer -= Time.deltaTime;

            if (netUpdateTimer <= 0)
            {
                netUpdateTimer = 1f / NET_UPDATES_PER_SECOND;

                //Send local player positions to other clients
                foreach (LobbyPlayer player in players)
                {
                    if (player.ClientGuid == myGuid && player.BallObject)
                    {
                        //update logic
                    }
                }
            }
        }
    }

    public void OnDestroy()
    {
        messenger.Close();
        if (activeChat)
            Destroy(activeChat.gameObject);
    }

    private void StartLobbyTimer(float offset = 0)
    {
        lobbyTimerOn = true;
        lobbyTimer -= offset;
        LobbyReferences.Active.CountdownField.enabled = true;
    }

    private void StopLobbyTimer()
    {
        lobbyTimerOn = false;
        lobbyTimer = lobbyTimerMax;
        LobbyReferences.Active.CountdownField.enabled = false;
    }

    private void GoToLobby()
    {
        if (inLobby) return;

        loadingStage = false;
        loadingLobby = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(lobbySceneName);
    }
}
