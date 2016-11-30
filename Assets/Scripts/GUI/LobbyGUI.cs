using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class LobbyGUI : MonoBehaviour {
		public Image Fade;
	
		public Text UsersPanel;
		public Text ChatTextPanel;
	
		public Button LeaveButton;
		public Button ReadyButton;

		public InputField ChatInputField;
	
		private bool inAnimation;

		private void Awake() {
			StartCoroutine(FadeGraphic(Fade, 0, 60f, Color.black, Color.clear));
		}
	
		private void Update() {
			Fade.raycastTarget = inAnimation;
		}
	
		public IEnumerator FadeGraphic(Graphic graphic, float delay, float duration, Color fromColor, Color toColor, bool disableRaycast = false, Action callback = null) {
			for (int i = 0; i < duration + delay; i++) {
				inAnimation = true;
				graphic.color = Color.Lerp(fromColor, toColor, Mathf.SmoothStep(0f, 1f, (i - delay) / duration));
				yield return null;
			}
			inAnimation = disableRaycast;
			
			if (callback != null) {
				callback();
			}
		}
	}
}