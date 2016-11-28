using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

namespace BattlePark {
	public class LobbyGUI : MonoBehaviour {
		public Image Fade;
	
		public Text UsersPanel;
		public Text ChatTextPanel;
	
		public Button LeaveButton;
		public Button ReadyButton;

		public InputField ChatInputField;

		private GameClient client;
	
		private bool inAnimation;

		public IEnumerator LoadTitleScreen() {
			inAnimation = true;

			//client.Close();

			for (float i = 0; i < 60; i++) {
				Fade.color = Color.Lerp(new Color(1f, 1f, 1f, 0), Color.white, Mathf.SmoothStep(0f, 1f, i / 40f));
				yield return null;
			}

			UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
		}

		private void Awake() {
			//ChatInputField.onEndEdit.AddListener(Chat);
		
			LeaveButton.onClick.AddListener(() => StartCoroutine(LoadTitleScreen()));
			//ReadyButton.onClick.AddListener(BecomeReady);

			client = FindObjectOfType<GameClient>();

			StartCoroutine(FadeGraphic(Fade, 0, 60f, Color.black, Color.clear));
		}
	
		private void Update() {
			Fade.raycastTarget = inAnimation;
		}
	
		private IEnumerator FadeGraphic(Graphic graphic, float delay, float duration, Color fromColor, Color toColor) {
			for (int i = 0; i < duration + delay; i++) {
				inAnimation = true;
				graphic.color = Color.Lerp(fromColor, toColor, Mathf.SmoothStep(0f, 1f, (i - delay) / duration));
				yield return null;
			}
			inAnimation = false;
		}
	}
}