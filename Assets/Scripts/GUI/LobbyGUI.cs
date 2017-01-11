using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyGUI : GUI {
	[Header("Panels")]
	public Graphic MainPanel;
	public Graphic CreateGamePanel;
	public Graphic FindGamePanel;
	public Graphic LobbyPanel;
	
	[Header("MainPanel")]
	public Button CreateGameButton;
	public Button FindGameButton;
	public Button ExitButton;
	
	[Header("CreateGamePanel")]
	public Button CreateGameBackButton;
	public InputField CreateGameRoomNameInputField;
	public InputField CreateGameUsernameInputField;
	public Button CreateGameCreateRoomButton;
	
	[Header("FindGamePanel")]
	public Button FindGameBackButton;
	
	[Header("LobbyPanel")]
	public Button LobbyReadyButton;
	public Button LobbyLeaveButton;
	public InputField LobbyChatInputField;
	public Text LobbyChatPanelText;
	public Text LobbyUsersPanelText;
	
	[Header("Data")]
	public List<LobbyPlayer> lobbyPlayers = new List<LobbyPlayer>();
	
	protected override void Awake() {
		base.Awake();
		
		currentPanel = MainPanel;
		
		CreateGameButton.onClick.AddListener(() => {
			CreateGameUsernameInputField.text = GenerateUsername();
			AnimatePanel(CreateGamePanel, 1);
		});
		
		FindGameButton.onClick.AddListener(() => {
			LobbyManager.Instance.StartMatchMaker();
			AnimatePanel(FindGamePanel, 1);
		});
		ExitButton.onClick.AddListener(Application.Quit);
		
		CreateGameBackButton.onClick.AddListener(() => AnimatePanel(MainPanel, -1));
		CreateGameRoomNameInputField.onEndEdit.AddListener(input => {
			if (Input.GetKeyDown(KeyCode.Return)) {
				LobbyManager.Instance.CreateRoom(input);
			}
		});
		CreateGameCreateRoomButton.onClick.AddListener(() => LobbyManager.Instance.CreateRoom(CreateGameRoomNameInputField.text));
		
		FindGameBackButton.onClick.AddListener(() => {
			CreateGameUsernameInputField.text = GenerateUsername();
			AnimatePanel(MainPanel, -1);
		});
		
		LobbyReadyButton.onClick.AddListener(() => {
			lobbyPlayers.First(player => player.isLocalPlayer).SendReadyToBeginMessage();
			LobbyReadyButton.interactable = false;
		});
		LobbyLeaveButton.onClick.AddListener(() => {
		    if (NetworkServer.active) {
		    	LobbyManager.Instance.matchMaker.DestroyMatch((NetworkID)CurrentMatchID, 0, OnDestroyMatch);
		    }
		    lobbyPlayers.First(player => player.isLocalPlayer).connectionToServer.Disconnect();
		    AnimatePanel(MainPanel, -1);
		});
		LobbyChatInputField;
		LobbyChatPanelText;
		LobbyUsersPanelText;
		
		//stop hosting
		//	
		
		//stop client
		//	StopClient();
		// if matchmaking StopMatchMaker();
		
		//stop server
		//	StopServer();
	}
	
	public void UpdateText() {
		string output = String.Empty;
		foreach (var player in lobbyPlayers) {
			output += String.Format("<color=#{0}>{1}</color>\n", player.readyToBegin? "ffffffff" : "ccccccff", player.Username);
		}
		
		LobbyUsersPanelText.text = output;
	}
	
	public static string GenerateUsername() {
		const string consonants = "bbbbbbbbbbbbbbbbbbbbbbbcdffgghjklmnppppppppppppppppppppppqrsssstvwxzzzz";
		const string vowels = "aaeeiiiiiooooooooooouuuuuuuuuuuuuy";
		int type = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));

		string returnedName = String.Empty;
		for (int i = 0; i < 14; i++) {
			if (i != 7) {
				float chance = UnityEngine.Random.value;
				if (type == 0) {
					returnedName += consonants.ElementAt(UnityEngine.Random.Range(0, consonants.Length));
					if (chance <= 0.5) {
						type = 1;
					}
				} else {
					returnedName += vowels.ElementAt(UnityEngine.Random.Range(0, vowels.Length));
					if (chance <= 0.6) {
						type = 0;
					}
				}
			} else {
				returnedName += " ";
			}
		}
		
		return returnedName;
	}
}