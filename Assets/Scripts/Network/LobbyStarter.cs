using Lidgren.Network;
using UnityEngine;

public class LobbyStarter : MonoBehaviour
{
    [SerializeField]
    private MatchManager matchManagerPrefab = null;

    private NetClient joiningClient;

    void Update()
    {
        if (joiningClient != null)
        {
            NetIncomingMessage incomingMessage;
            while ((incomingMessage = joiningClient.ReadMessage()) != null)
            {
                switch (incomingMessage.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Debug.Log(incomingMessage.ReadString());
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Debug.LogWarning(incomingMessage.ReadString());
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        Debug.LogError(incomingMessage.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)incomingMessage.ReadByte();
                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                Debug.Log("Connected!");
                                break;

                            case NetConnectionStatus.Disconnected:
                                Debug.Log("Disconnected!");
                                break;

                            default:
                                string statusMsg = incomingMessage.ReadString();
                                Debug.Log("Status change received: " + status + " - Message: " + statusMsg);
                                break;
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        byte type = incomingMessage.ReadByte();
                        if (type == MessageType.InitMessage)
                        {
                            string lobbyStateMessage = "";
                            try
                            {
                                lobbyStateMessage = incomingMessage.ReadString();
                                LobbyState lobbyInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<LobbyState>(lobbyStateMessage);
                                BeginOnlineGame(lobbyInfo);
                            }
                            catch (Newtonsoft.Json.JsonException ex)
                            {
                                joiningClient.Disconnect("Failed to read lobby state");
                                Debug.LogError("Could not read lobby state, error: " + ex.Message);
                                Debug.LogError("Full message: " + lobbyStateMessage);
                            }
                        }
                        break;

                    default:
                        Debug.Log("unhandled NetIncomingMessage: " + incomingMessage.MessageType);
                        break;
                }
            }
        }
    }

    private void BeginOnlineGame(LobbyState lobbyState)
    {
        LobbyManager manager = Instantiate(lobbyManagerPrefab);
        manager.InitOnlineMatch(joiningClient, lobbyState);
    }
}