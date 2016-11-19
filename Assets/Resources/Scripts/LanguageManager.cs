using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class LanguageManager : MonoBehaviour {
	public string Language = "en";
	
	void Start() {
		DontDestroyOnLoad(this);
		switch (Application.systemLanguage) {
			case SystemLanguage.English:
				Language = "en";
				break;
			default:
				Language = "en";
				break;
		}
	}
	
	public string GetString(string key) {
		TextAsset language = (TextAsset)Resources.Load("Language/" + Language);
		string text = "ERROR";
		JsonConvert.DeserializeObject<Dictionary<string, string>>(language.text).TryGetValue(key, out text);
		return text;
	}
}