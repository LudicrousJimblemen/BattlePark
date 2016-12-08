using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class GameGUI : MonoBehaviour {
		public Image Fade;
	
		public Button PathsButton;
		
		private bool inAnimation;
	
		private void Awake() {
			Fade.color = Color.black;
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