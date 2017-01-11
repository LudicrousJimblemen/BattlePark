using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

public class LobbyServerEntry : MonoBehaviour {
	public void NoneFound() {
		GetComponentInChildren<Text>().text = LanguageManager.GetString("titleScreen.noMatchesFound");
		GetComponent<Button>().interactable = false;
	}
	
	public void Populate(MatchInfoSnapshot match) {
		GetComponentInChildren<Text>().text = String.Format("{0} - <i>({1}/{2})</i>", match.name, match.currentSize, match.maxSize);
        //joinhandler
    }

    private void JoinMatch(NetworkID networkID) {
		LobbyManager.Instance.matchMaker.JoinMatch(networkID, "", "", "", 0, 0, LobbyManager.Instance.OnMatchJoined);
		//lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        LobbyManager.Instance.IsMatchmaking = true;
        //lobbyManager.DisplayIsConnecting();
    }
}