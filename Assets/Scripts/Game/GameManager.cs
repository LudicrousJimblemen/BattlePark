using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	
	public Vector3[] ParkCenters;
	
	public GridObject[] Objects;
	public List<Person> Guests;

	private void Awake() {
		if(FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	private void OnGUI() {
		string text = String.Empty;
		foreach (var person in Guests) {
			text += person.GetProperty("name") + ":\n";
			foreach (var thought in person.Thoughts) {
				Debug.Log(thought.ThoughtString);
				text += "   \"" + String.Format(LanguageManager.GetString(thought.ThoughtString), thought.Parameters);
			}
		}
		UnityEngine.GUI.Label(new Rect(5, 5, 666, 666), text);
	}
}
