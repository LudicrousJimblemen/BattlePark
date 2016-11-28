using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextLocalizer : MonoBehaviour {
	Regex regex = new Regex("({{)(.*)(}})");

	private void Awake() {
		GetComponent<Text>().text = regex.Replace(GetComponent<Text>().text, match => LanguageManager.GetString(match.Groups[2].Value));
	}
}