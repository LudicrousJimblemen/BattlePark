using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
	
	private void Awake() {
		CreateGameButton.onClick.AddListener(() => AnimatePanel(CreateGamePanel, 1));
		FindGameButton.onClick.AddListener(() => AnimatePanel(FindGamePanel, 1));
		ExitButton.onClick.AddListener(Application.Quit);
		
		CreateGameBackButton.onClick.AddListener(() => {
			CreateGameUsernameInputField.text = GenerateUsername();
			AnimatePanel(MainPanel, -1);
		});
		CreateGameRoomNameInputField.onEndEdit.AddListener(input => {
			if (Input.GetKeyDown(KeyCode.Return)) {
                LobbyManager.s_Singleton.CreateRoom(input);
            }
		});
		CreateGameCreateRoomButton.onClick.AddListener(() => LobbyManager.s_Singleton.CreateRoom(CreateGameRoomNameInputField.text));
		
		FindGameBackButton.onClick.AddListener(() => {
			CreateGameUsernameInputField.text = GenerateUsername();
			AnimatePanel(MainPanel, -1);
		});
		
		//stop hosting
		//	matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
		
		//stop client
		//	StopClient();
		// if matchmaking StopMatchMaker();
		
		//stop server
		//	StopServer();
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