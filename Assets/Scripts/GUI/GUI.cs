using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {
	public Image Fade;
	public Color FadeFrom;
	
	private bool inAnimation;
	
	[HideInInspector]
	public Graphic CurrentPanel;

	private void Awake() {
		FadeGraphic(Fade, 0, 60f, FadeFrom, new Color(FadeFrom.r, FadeFrom.g, FadeFrom.b, 0));
	}

	private void Update() {
		Fade.raycastTarget = inAnimation;
	}

	public void FadeGraphic(Graphic graphic, float delay, float duration, Color fromColor, Color toColor, bool disableRaycast = false, Action callback = null) {
		StartCoroutine(FadeGraphicCoroutine(graphic, delay, duration, fromColor, toColor, disableRaycast, callback));
	}
	private IEnumerator FadeGraphicCoroutine(Graphic graphic, float delay, float duration, Color fromColor, Color toColor, bool disableRaycast, Action callback) {
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

	public void AnimatePanel(Graphic to, int fromDirection) {
		StartCoroutine(AnimatePanelCoroutine(to, fromDirection));
	}
	private IEnumerator AnimatePanelCoroutine(Graphic to, int fromDirection) {
		inAnimation = true;
		to.gameObject.SetActive(true);
	
		for (int i = 0; i < 70; i++) {
			to.rectTransform.localPosition = Vector3.Lerp(
				new Vector3(to.rectTransform.rect.width * fromDirection, 0, 0),
				Vector3.zero,
				Mathf.SmoothStep(0, 1f, Mathf.SmoothStep(0, 1f, i / 70f))
			);
			CurrentPanel.rectTransform.localPosition = Vector3.Lerp(
				Vector3.zero,
				new Vector3(CurrentPanel.rectTransform.rect.width * -fromDirection, 0, 0),
				Mathf.SmoothStep(0, 1f, Mathf.SmoothStep(0, 1f, i / 70f))
			);
			yield return null;
		}
	
		CurrentPanel.gameObject.SetActive(false);
		CurrentPanel = to;
		inAnimation = false;
	}
}