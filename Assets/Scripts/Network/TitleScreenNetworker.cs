using System;
using System.Linq;
using UnityEngine;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class TitleScreenNetworker : MonoBehaviour {
		public TitleScreenGUI GUI;
		
		private Client client;
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			//GUI.FindIP();
			
			GUI.StartClientButton.onClick.AddListener(() => {
				StartCoroutine(GUI.AnimatePanel(GUI.ClientPanel, 1));
				GUI.GenerateUsername();
			});
			GUI.ExitButton.onClick.AddListener(Application.Quit);
			
			GUI.ClientBackButton.onClick.AddListener(() => StartCoroutine(GUI.AnimatePanel(GUI.MainPanel, -1)));
			GUI.ClientJoinButton.onClick.AddListener(StartClient);
			
			client.CreateListener<ServerApprovalNetMessage>(OnServerApproval);
			client.CreateListener<ServerDenialNetMessage>(OnServerDenial);
		}

		private void StartClient() {
			client.JoinOnlineGame(
				GUI.ClientUsernameInputField.text,
				GUI.ClientIpInputField.text,
				Int32.Parse(GUI.ClientPortInputField.text)
			);
			
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 30f, Color.clear, new Color(0, 0, 0, 0.4f), true));
		}
		
		private void OnServerApproval(ServerApprovalNetMessage message) {
			client.RemoveListener<ServerApprovalNetMessage>(OnServerApproval);
			client.RemoveListener<ServerDenialNetMessage>(OnServerDenial);
			
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, new Color(0, 0, 0, 0.4f), Color.black, false, () => {
				UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
			}));
		}
		
		private void OnServerDenial(ServerDenialNetMessage message) {
			Debug.LogWarning(String.Format(LanguageManager.GetString(message.Reason), message.Username, message.ClientVersion, message.ServerVersion));
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 30f, new Color(0, 0, 0, 0.4f), Color.clear));
		}
	}
}