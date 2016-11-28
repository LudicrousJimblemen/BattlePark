using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour {
	[Serializable]
	public struct LocalizableText {
		public Text Text;
		public string Reference;
	}

	public List<LocalizableText> Texts;

	private void Update() {
		foreach (var text in Texts) {
			text.Text.text = LanguageManager.GetString(text.Reference);
		}
	}
}