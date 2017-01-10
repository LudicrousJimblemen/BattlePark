using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

public class LobbyServerEntry : MonoBehaviour {
    public Text serverInfoText;
    public Text slotInfo;
    public Button joinButton;

	public void Populate(MatchInfoSnapshot match, LobbyManager lobbyManager) {
        NetworkID networkID = match.networkId;

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(() => JoinMatch(networkID, lobbyManager));
    }

    void JoinMatch(NetworkID networkID, LobbyManager lobbyManager) {
		lobbyManager.matchMaker.JoinMatch(networkID, "", "", "", 0, 0, lobbyManager.OnMatchJoined);
		//lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        lobbyManager._isMatchmaking = true;
        //lobbyManager.DisplayIsConnecting();
    }
}