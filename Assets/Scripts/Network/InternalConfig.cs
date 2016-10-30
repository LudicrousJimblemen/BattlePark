using System.Collections.Generic;

public static class InternalConfig
{
	private static Dictionary<string, string> gridObjectPaths = new Dictionary<string,string> {
		{"Pingu", "Scenery/Sculpture"}
	};
	
	public static string GetGridObjectPath(string objectName) {
		string value;
		
		if (gridObjectPaths.TryGetValue(objectName, out value)) {
			return value;
		} else {
			return null;
		}
	}
}