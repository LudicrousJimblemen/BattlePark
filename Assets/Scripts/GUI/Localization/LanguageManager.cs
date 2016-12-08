using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public static class LanguageManager {
		public static string Language = String.Empty;
	
		public static string GetString(string key) {
			if (Language == string.Empty) {
				switch (Application.systemLanguage) {
					case SystemLanguage.English:
						Language = "en";
						break;
					case SystemLanguage.Japanese:
						Language = "jp";
						break;
					default:
						Language = "en";
						break;
				}
			}
		
			TextAsset language = (TextAsset)Resources.Load("Language/" + Language);
			string text;
			JsonConvert.DeserializeObject<Dictionary<string, string>>(language.text).TryGetValue(key, out text);
			if (text != null) {
				return text;
			} else {
				Debug.LogError(String.Format("Error localizing string '{0}' in language '{1}'", key, language));
				return "ERROR";
			}
		}
	}
}