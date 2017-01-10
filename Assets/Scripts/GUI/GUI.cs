using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {
	public Image Fade;
	public Color FadeFrom;
	
	private bool inAnimation;
	
	protected Graphic currentPanel;

	protected virtual void Awake() {
		FadeGraphic(Fade, 0, 60f, FadeFrom, new Color(FadeFrom.r, FadeFrom.g, FadeFrom.b, 0));
	}

	protected virtual void Update() {
		Fade.raycastTarget = inAnimation;
	}

	public void FadeGraphic(Graphic graphic, float delay, float duration, Color fromColor, Color toColor, bool disableRaycast = false, Action callback = null) {
		StartCoroutine(FadeGraphicCoroutine(graphic, delay, duration, fromColor, toColor, disableRaycast, callback));
	}
	private IEnumerator FadeGraphicCoroutine(Graphic graphic, float delay, float duration, Color fromColor, Color toColor, bool disableRaycast, Action callback) {
		inAnimation = true;
		for (int i = 0; i < duration + delay; i++) {
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
			currentPanel.rectTransform.localPosition = Vector3.Lerp(
				Vector3.zero,
				new Vector3(currentPanel.rectTransform.rect.width * -fromDirection, 0, 0),
				Mathf.SmoothStep(0, 1f, Mathf.SmoothStep(0, 1f, i / 70f))
			);
			yield return null;
		}
	
		currentPanel.gameObject.SetActive(false);
		currentPanel = to;
		inAnimation = false;
	}
}