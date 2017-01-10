using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace BattlePark {
	public static class LanguageManager {
		public static string Language {
			get {
				switch (Application.systemLanguage) {
					case SystemLanguage.English:
						return "en";
					case SystemLanguage.Japanese:
						return "jp";
					default:
						return "en";
				}
			}
		}
	
		public static string GetString(string key, string language = null) {
			if (language == null) {
				language = Language;
			}
		
			TextAsset asset = (TextAsset)(TextAsset)Resources.Load("Language/" + Language);
			
			string localized;
			JsonConvert.DeserializeObject<Dictionary<string, string>>(asset.text).TryGetValue(key, out localized);
			
			if (localized != null) {
				return localized;
			} else {
				Debug.LogError(String.Format("Error localizing string '{0}' in language '{1}'", key, language));
				return key;
			}
		}
	}
}