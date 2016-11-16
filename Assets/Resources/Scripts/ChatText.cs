using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatText : Text {
	public void AddText(string input) {
		text += "\n" + input;
	}
}