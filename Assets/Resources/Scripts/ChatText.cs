using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatText : Text {
	public void AddText(string input) {
		if (text.Equals (String.Empty)) {
			text += input;
		} else {
			text += "\n" + input;
		}
	}
}