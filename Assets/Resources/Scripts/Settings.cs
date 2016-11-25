using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;

public class Settings {
	[JsonIgnore]
	public string Path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.ApplicationData) + @"\BattlePark\Settings.json";

	#region Camera
	[Range (0,8)]
	public float MovementSpeed = 1f;
	[Range (0,10)]
	public float RotationSpeed = 2f;

	[Range (0,150)]
	public float TiltHeight = 80f;
	[Range (0,50)]
	public float FlatHeight = 40f;

	[Range (0,90)]
	public float TiltAngle = 45f;
	[Range (0,90)]
	public float FlatAngle = 20f;
	#endregion

	#region Controls
	bool InvertRotation = false;
	#endregion

	public Settings () {
		if (!File.Exists (Path)) File.WriteAllText (Path,JsonConvert.SerializeObject (this,Formatting.Indented));
		StreamReader reader = new StreamReader (Path);
		Deserialize (reader.ReadToEnd ());
		Debug.Log (TiltAngle);
	}

	public void Deserialize (string input) {
		/*
		MovementSpeed = read.MovementSpeed;
		RotationSpeed = read.RotationSpeed;
		TiltHeight = read.TiltHeight;
		FlatHeight = read.FlatHeight;
		TiltAngle = read.TiltAngle;
		FlatAngle = read.FlatAngle;
		*/
	}
}
