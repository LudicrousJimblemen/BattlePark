using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Person))]
public class DisplayPersonProperties : Editor {
	bool expand = true;
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Person person = target as Person;
		if (GUILayout.Button("Re-roll"))
			person.Reroll();
		expand = EditorGUILayout.Foldout(expand, "Properties");
		if (expand) {
			if (person.properties == null)
				person.Reroll();
			using (new EditorGUI.DisabledScope(true)) {
				for (int i = 0; i < person.properties.Length; i++) {
					string name = person.properties[i].Key.First().ToString().ToUpper() + person.properties[i].Key.Substring(1);
					EditorGUILayout.TextField(name, person.properties[i].Value.ToString());
				}
			}
		}
	}
}
