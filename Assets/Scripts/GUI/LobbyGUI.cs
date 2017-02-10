using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyGUI : GUI {
	public static LobbyGUI Instance;
	
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
	public InputField FindGameUsernameInputField;
	public Button FindGameBackButton;
	
	[Header("LobbyPanel")]
	public Button LobbyReadyButton;
	public Button LobbyLeaveButton;
	public InputField LobbyChatInputField;
	public Text LobbyChatPanelText;
	public Text LobbyUsersPanelText;
	
	[Header("Prefabs")]
	public GameObject ServerButtonPrefab;
	
	[HideInInspector]
	public LobbyPlayer LocalPlayer;
	[HideInInspector]
	public string Username;
	
	public void LoadMain() {
		SwitchPanel(MainPanel);
		FadeGraphic(Fade, 0, 60, FadeFrom, 0);
		
		// currentPanel = MainPanel;
		
		CreateGameButton.onClick.AddListener(() => {
			Client.Instance.StartMatchMaker();
			AnimatePanel(CreateGamePanel, 1);
			Username = GenerateUsername();
			CreateGameUsernameInputField.text = Username;
			CreateGameRoomNameInputField.text = String.Format(LanguageManager.GetString("lobby.defaultRoomName"), Username);
		});
		FindGameButton.onClick.AddListener(() => {
			Client.Instance.StartMatchMaker();
			PopulateServerList();
			Username = GenerateUsername();
			FindGameUsernameInputField.text = Username;
		});
		ExitButton.onClick.AddListener(Application.Quit);
		
		CreateGameBackButton.onClick.AddListener(() => AnimatePanel(MainPanel, -1));
		CreateGameCreateRoomButton.onClick.AddListener(CreateMatch);
		
		FindGameBackButton.onClick.AddListener(() => AnimatePanel(MainPanel, -1));
		
		LobbyLeaveButton.onClick.AddListener(() => 
			NetworkManager.singleton.matchMaker.DropConnection(
			Client.Instance.matchInfo.networkId,
			Client.Instance.matchInfo.nodeId,
			0,
			Client.Instance.OnDropConnection
		)
		);
		LobbyReadyButton.interactable = false;
		LobbyReadyButton.onClick.AddListener(() => {
			if (Client.Instance.localPlayer == null)
				return;
			bool ready = Client.Instance.localPlayer.ToggleReady();
			LobbyReadyButton.GetComponentInChildren<Text>().text = LanguageManager.GetString(ready ? "lobby.ready" : "lobby.unready");
		});
	}
	
	protected void Awake() {
		if (FindObjectsOfType<LobbyGUI>().Count() > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
		
		currentPanel = MainPanel;
		LoadMain();
	}
	
	private void PopulateServerList() {
		Client.Instance.ListMatches(6, (listSuccess, listExtendedInfo, matches) => {
			if (listSuccess) {
				for (int i = 2; i < FindGamePanel.transform.childCount; i++) {
					Destroy(FindGamePanel.transform.GetChild(i).gameObject);
				}
					
				if (matches.Count == 0) {
					GameObject notFound = (GameObject)Instantiate(ServerButtonPrefab);
					notFound.GetComponentInChildren<Text>().text = LanguageManager.GetString("titleScreen.noMatchesFound");
					notFound.transform.SetParent(FindGamePanel.transform, false);
					notFound.GetComponent<Button>().interactable = false;
				} else {
					foreach (var match in matches) {
						GameObject newButton = (GameObject)Instantiate(ServerButtonPrefab);
						newButton.GetComponentInChildren<Text>().text = String.Format("{0} - <i>({1}/{2})</i>", match.name, match.currentSize, match.maxSize);
						newButton.transform.SetParent(FindGamePanel.transform, false);
						newButton.GetComponent<Button>().onClick.AddListener(() => {
							FadeGraphic(Fade, 0, 20, Color.clear, 0.3f, true);
							Client.Instance.JoinMatch(match.networkId, (joinSuccess, joinExtendedInfo, matchInfo) => {
								FadeGraphic(Fade, 0, 20, Fade.color, 0, true);
								if (joinSuccess) {
									AnimatePanel(LobbyPanel, 1);
								} else {
									AnimatePanel(MainPanel, -1);
								}
							});
						});
					}
				}
	     		    
				for (int i = 0; i < FindGamePanel.transform.childCount; i++) {
					((RectTransform)FindGamePanel.transform.GetChild(i).transform).localPosition = new Vector3(
						0,
						(-50 * i) + (25 * (FindGamePanel.transform.childCount - 1)),
						0);
				}
			                            	
				AnimatePanel(FindGamePanel, 1);
			}
		});
	}
	
	private void CreateMatch() {
		Client.Instance.CreateMatch(CreateGameRoomNameInputField.text, 2, (success, extendedInfo, matchInfo) => {
			if (success) {
				AnimatePanel(LobbyPanel, 1);
			}
		});
	}
	
	public void UpdatefghsdfText() {
		string output = String.Empty;
		/*
		foreach (var player in lobbyPlayers) {
			output += String.Format("<color=#{0}>{1}</color>\n", player.readyToBegin? "ffffffff" : "ccccccff", player.Username);
		}
		*/
		
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
				bool upper = (i == 0 || returnedName[i - 1].Equals(' '));
				string source;
				if (type == 0) {
					source = consonants;
					if (upper)
						source = source.ToUpper();
					returnedName += source.ElementAt(UnityEngine.Random.Range(0, consonants.Length));
					if (chance <= 0.5) {
						type = 1;
					}
				} else {
					source = vowels;
					if (upper)
						source = source.ToUpper();
					returnedName += source.ElementAt(UnityEngine.Random.Range(0, vowels.Length));
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
	
	// this is trash - please remove it and replace with the regular FadeGraphic function
	// FadeGraphic(Fade, 0 60, new Color(1f, 1f, 1f, 0f), 1f, false, callback)
	public void FadeToWhite(Action callback = null) {
		Color transparentFadeFrom = FadeFrom;
		transparentFadeFrom.a = 0;
		FadeGraphic(Fade, 0, 60, transparentFadeFrom, 1f, false, callback);
	}
}