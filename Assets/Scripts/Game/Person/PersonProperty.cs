[System.Serializable]
public struct PersonProperty {
	public string Key;
	public object Value;
	public object[] Range;
	public PersonProperty(string key,object value, System.Type valueType,float min = 0f, float max = 0f) {
		Key = key;
		Value = value;
		if (valueType == typeof(int) ||	valueType == typeof(float)) {
			Range = new object[2] { System.Convert.ChangeType(min,valueType),System.Convert.ChangeType(min,valueType) };
		} else {
			Range = null;
		}
	}
}
