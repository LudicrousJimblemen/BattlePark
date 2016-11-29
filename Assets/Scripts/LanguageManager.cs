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
			string text = "ERROR";
			JsonConvert.DeserializeObject<Dictionary<string, string>>(language.text).TryGetValue(key, out text);
			return text;
		}
	}
}