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

    [SerializeField]
    private PauseMenu pauseMenuPrefab;

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

#endregion Lobby state

    #region Scenes and scene initializing

    //Bools for scene initializing and related stuff
    private bool inLobby = false; //Are we in the lobby or in a race?
    private bool loadingLobby = false;
    private bool loadingStage = false;
    private bool joiningRaceInProgress = false; //If true, RaceManager will be created as if a race was already in progress.
    private bool showSettingsOnLobbyLoad = false; //If true, the lobby settings window will pop up when the lobby scene is entered.

    #endregion Scenes and scene initializing

    //Lobby messenger used to send and receive state changes.
    //This will be either a LocalLobbyMessenger or OnlineLobbyMessenger, but each are used the same way.
    private LobbyMessenger messenger;

    private UI.Chat activeChat;

    //Timer used for syncing realtime stuff in online
    private float netUpdateTimer = 0;
    private const int NET_UPDATES_PER_SECOND = 40;

    #region Properties

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

    #endregion Properties

    #region State changing methods

    public void RequestSettingsChange(LobbySettings newSettings)
    {
        messenger.SendMessage(new SettingsChangedMessage(newSettings));
    }

    public void RequestPlayerJoin(ControlType ctrlType, int initialCharacter)
    {
        messenger.SendMessage(new PlayerJoinedMessage(myGuid, ctrlType, initialCharacter));
    }

    public void RequestPlayerLeave(ControlType ctrlType)
    {
        messenger.SendMessage(new PlayerLeftMessage(myGuid, ctrlType));
    }

    public void RequestCharacterChange(ControlType ctrlType, int newCharacter)
    {
        messenger.SendMessage(new CharacterChangedMessage(myGuid, ctrlType, newCharacter));
    }

    public void RequestReadyChange(ControlType ctrlType, bool ready)
    {
        messenger.SendMessage(new ChangedReadyMessage(myGuid, ctrlType, ready));
    }

    public void RequestLoadLobby()
    {
        messenger.SendMessage(new LoadLobbyMessage());
    }

    #endregion State changing methods

    #region Lobby message callbacks

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

    private void PlayerJoinedCallback(PlayerJoinedMessage msg, float travelTime)
    {
        var p = new LobbyPlayer(msg.ClientGuid, msg.CtrlType, msg.InitialCharacter);
        players.Add(p);

        if (inLobby)
        {
            SpawnLobbyBall(p);
        }

        StopLobbyTimer();

        if (LobbyPlayerAdded != null)
            LobbyPlayerAdded(this, new LobbyPlayerEventArgs(p, msg.ClientGuid == myGuid));
    }

    private void PlayerLeftCallback(PlayerLeftMessage msg, float travelTime)
    {
        var player = players.FirstOrDefault(a => a.ClientGuid == msg.ClientGuid && a.CtrlType == msg.CtrlType);
        if (player != null)
        {
            players.Remove(player);

            if (player.BallObject)
            {
                player.BallObject.CreateRemovalParticles();
                Destroy(player.BallObject.gameObject);
            }

            if (LobbyPlayerRemoved != null)
                LobbyPlayerRemoved(this, new LobbyPlayerEventArgs(player, msg.ClientGuid == myGuid)); //TODO: determine if removed player was local
        }
    }

    private void CharacterChangedCallback(CharacterChangedMessage msg, float travelTime)
    {
        if (!inLobby)
        {
            Debug.LogError("Cannot set character outside of lobby!");
        }

        var player = players.FirstOrDefault(a => a.ClientGuid == msg.ClientGuid && a.CtrlType == msg.CtrlType);
        if (player != null)
        {
            player.CharacterId = msg.NewCharacter;
            SpawnLobbyBall(player);
        }
    }

    private void ChangedReadyCallback(ChangedReadyMessage msg, float travelTime)
    {
        var player = players.FirstOrDefault(a => a.ClientGuid == msg.ClientGuid && a.CtrlType == msg.CtrlType);
        if (player != null)
        {
            player.ReadyToRace = !player.ReadyToRace;

            //Check if all players are ready and start/stop lobby timer accordingly
            var allReady = players.TrueForAll(a => a.ReadyToRace);
            if (allReady && !lobbyTimerOn)
            {
                StartLobbyTimer(travelTime);
            }
            if (!allReady && lobbyTimerOn)
            {
                StopLobbyTimer();
            }
        }
    }

    private void LoadRaceCallback(LoadRaceMessage msg, float travelTime)
    {
        StopLobbyTimer();
        CameraFade.StartAlphaFade(Color.black, false, 0.3f, 0.05f, () =>
        {
            GoToStage();
        });
    }

    private void ChatCallback(ChatMessage msg, float travelTime)
    {
        if (activeChat)
            activeChat.ShowMessage(msg.Type, msg.From, msg.Text);
    }

    private void LoadLobbyCallback(LoadLobbyMessage msg, float travelTime)
    {
        GoToLobby();
    }

    private void AutoStartTimerCallback(AutoStartTimerMessage msg, float travelTime)
    {
        autoStartTimerOn = msg.Enabled;
        autoStartTimer = currentSettings.AutoStartTime - travelTime;
    }

    private void PlayerMovementCallback(PlayerMovementMessage msg, float travelTime)
    {
        if (msg.ClientGuid == myGuid) return;

        LobbyPlayer player = players.FirstOrDefault(a => a.ClientGuid == msg.ClientGuid && a.CtrlType == msg.CtrlType);
        if (player != null && player.BallObject != null)
        {
            player.ProcessMovementMessage(msg);
        }
    }

    #endregion Lobby message callbacks

    #region Lobby initializing

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

        //Set auto start timer
        //TODO Get and apply travel time
        autoStartTimerOn = lobbyState.CurAutoStartTime != 0;
        autoStartTimer = lobbyState.CurAutoStartTime;

        //Create messenger
        messenger = new OnlineLobbyMessenger(client);
        ((OnlineLobbyMessenger)messenger).Disconnected += (sender, e) =>
        {
            QuitLobby(e.Reason);
        };

        //Create chat
        activeChat = Instantiate(chatPrefab);
        activeChat.MessageSent += LocalChatMessageSent;

        //Enter the lobby or stage
        if (lobbyState.InRace)
        {
            joiningRaceInProgress = true;
            GoToStage();
        }
        else
        {
            //showSettingsOnLobbyLoad = true;
            GoToLobby();
        }
    }

    #endregion Lobby initializing

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //A messenger should be created by now! Time to create some message listeners
        messenger.CreateListener<SettingsChangedMessage>(SettingsChangedCallback);
        messenger.CreateListener<ClientJoinedMessage>(ClientJoinedCallback);
        messenger.CreateListener<ClientLeftMessage>(ClientLeftCallback);
        messenger.CreateListener<PlayerJoinedMessage>(PlayerJoinedCallback);
        messenger.CreateListener<PlayerLeftMessage>(PlayerLeftCallback);
        messenger.CreateListener<CharacterChangedMessage>(CharacterChangedCallback);
        messenger.CreateListener<ChangedReadyMessage>(ChangedReadyCallback);
        messenger.CreateListener<LoadRaceMessage>(LoadRaceCallback);
        messenger.CreateListener<ChatMessage>(ChatCallback);
        messenger.CreateListener<LoadLobbyMessage>(LoadLobbyCallback);
        messenger.CreateListener<AutoStartTimerMessage>(AutoStartTimerCallback);
        messenger.CreateListener<PlayerMovementMessage>(PlayerMovementCallback);

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

        //Pausing/unpausing
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (!UI.PauseMenu.GamePaused)
            {
                UI.PauseMenu menu = Instantiate(pauseMenuPrefab);
                menu.OnlineMode = OnlineMode;
            }
            else
            {
                var menu = FindObjectOfType<UI.PauseMenu>();
                if (menu)
                    Destroy(menu.gameObject);
            }
        }

        if (lobbyTimerOn && inLobby)
        {
            lobbyTimer -= Time.deltaTime;
            LobbyReferences.Active.CountdownField.text = "Lobby starts in " + Mathf.Max(1f, Mathf.Ceil(lobbyTimer));

            //LoadRaceMessages don't need to be sent in online mode - the server will ignore it anyway.
            if (lobbyTimer <= 0 && !OnlineMode)
            {
                messenger.SendMessage(new LoadRaceMessage());
            }
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
                        Rigidbody ballRb = player.BallObject.GetComponent<Rigidbody>();
                        messenger.SendMessage(new PlayerMovementMessage(
                            DateTime.Now,
                            myGuid,
                            player.CtrlType,
                            player.BallObject.transform.position.ToSimpleVector3(),
                            player.BallObject.transform.rotation.eulerAngles.ToSimpleVector3(),
                            ballRb.velocity.ToSimpleVector3(),
                            ballRb.angularVelocity.ToSimpleVector3(),
                            player.BallObject.DirectionVector.ToSimpleVector3()
                            ));
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

    #region Players ready and lobby timer

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

    #endregion Players ready and lobby timer

    #region Scene changing / race loading

    private void GoToLobby()
    {
        if (inLobby) return;

        loadingStage = false;
        loadingLobby = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(lobbySceneName);
    }

    private void GoToStage()
    {
        var targetStage = ActiveData.Stages[currentSettings.StageId];

        loadingStage = true;
        loadingLobby = false;

        foreach (var p in Players)
        {
            p.ReadyToRace = false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(targetStage.sceneName);
        ;
    }

    //Check if we were loading the lobby or the race
    private void OnLevelWasLoaded(int level)
    {
        if (loadingLobby)
        {
            InitLobby();
            loadingLobby = false;
        }
        if (loadingStage)
        {
            InitRace();
            loadingStage = false;
        }
    }

    //Initiate the lobby after loading lobby scene
    private void InitLobby()
    {
        inLobby = true;

        foreach (var p in Players)
        {
            SpawnLobbyBall(p);
        }

        if (showSettingsOnLobbyLoad)
        {
            //Let the player pick settings first time entering the lobby
            LobbyReferences.Active.LobbySettingsPanel.Show();
            showSettingsOnLobbyLoad = false;
        }
    }

    //Initiate a race after loading the stage scene
    private void InitRace()
    {
        inLobby = false;

        var raceManager = Instantiate(raceManagerPrefab);
        raceManager.Init(currentSettings, this, messenger, joiningRaceInProgress);
        joiningRaceInProgress = false;
    }

    public void QuitLobby(string reason = null)
    {
        StartCoroutine(QuitLobbyInternal(reason));
    }

    private IEnumerator QuitLobbyInternal(string reason)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

        if (reason != null)
        {
            yield return null;

            FindObjectOfType<UI.PopupHandler>().OpenPopup(disconnectedPopupPrefab);
            FindObjectOfType<UI.PopupDisconnected>().Reason = reason;
        }

        Destroy(gameObject);
    }

    #endregion Scene changing / race loading

    private void SpawnLobbyBall(LobbyPlayer player)
    {
        var spawner = LobbyReferences.Active.BallSpawner;
        if (player.BallObject != null)
        {
            player.BallObject.CreateRemovalParticles();
            Destroy(player.BallObject.gameObject);
        }

        string name = clients.First(a => a.Guid == player.ClientGuid).Name + " (" + GameInput.GetControlTypeName(player.CtrlType) + ")";

        player.BallObject = spawner.SpawnBall(PlayerType.Normal, (player.ClientGuid == myGuid) ? player.CtrlType : ControlType.None, player.CharacterId, name);

        if (player.ClientGuid != myGuid)
        {
            Marker marker = Instantiate(markerPrefab);
            marker.transform.SetParent(LobbyReferences.Active.MarkerContainer, false);
            marker.Color = Color.clear;
            marker.Text = name;
            marker.Target = player.BallObject.transform;
        }
    }
}
