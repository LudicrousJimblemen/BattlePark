using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Person))]
public class PersonInspector : Editor {
	public void OnSceneGUI () {
		Person person = target as Person;
		string label = String.Format("<color=white><size=10>{0}\n",person.Name);
		label += String.Format("    Money: {0}\n", person.Money.ToString (LanguageManager.GetString("game.gui.numericCurrencySmall")));
		label += String.Format("    Hunger: {0}\n",Math.Round(person.Hunger,1));
		label += String.Format("    Thirst: {0}\n",Math.Round(person.Thirst,1));
		label += String.Format("    Nausea: {0}\n",Math.Round(person.Nausea,1));
		label += String.Format("    Urgency: {0}\n",Math.Round(person.Urgency,1));
		label += String.Format("    Mood: {0}\n",Math.Round(person.Mood,1));
		label += String.Format("    Suspicion: {0}\n",Math.Round(person.Suspicion,1));

		label += "    Desires:\n";
		foreach(var desire in person.Desires) {
			DesireType type = (DesireType)desire;
			switch (type) {
				case DesireType.Food:
					var f = (DesireFood) desire;
					label += String.Format("        DesireFood: Food = ({0}), Target = ({1})\n",LanguageManager.GetString(f.Food.ProperString),f.Target);
					break;
				case DesireType.Attraction:
					var d = (DesireAttraction) desire;
					label += String.Format("        DesireAttraction: Attraction = ({0}), Target = ({1})\n",LanguageManager.GetString(d.Attraction.ProperString),d.Target);
					break;
			}
		}

		label += "    Target:\n";
		if(person.walker.Target != null) {
			label += String.Format("        Name: {0}\n",person.walker.Target.name);
			label += String.Format("        Square Distance: {0}\n",Math.Round((person.walker.Target.position - person.transform.position).sqrMagnitude,1));
		} else {
			label += "        null\n";
		}

		label += "    Thoughts:\n";
		foreach(var thought in person.Thoughts) {
			label += "        " + String.Format(LanguageManager.GetString(thought.ThoughtString),thought.Parameters.Select(parameter => LanguageManager.GetString(parameter)).ToArray()) + "\n";
		}

		label += "    Inventory:\n";
		foreach(var item in person.Inventory) {
			InventoryFood foodItem = item as InventoryFood;
			if(item != null) {
				label += String.Format("        InventoryFood: Food = ({0}), Amount = ({1})\n",LanguageManager.GetString(foodItem.Food.ProperString),foodItem.Amount);
			}
		}

		label += "</size></color>";

		Handles.BeginGUI();
		Handles.Label((person.InAttraction ? person.transform.position.Flat() : person.transform.position) + 3 * Vector3.up,label,new GUIStyle { richText = true,alignment = TextAnchor.LowerLeft });

		if(person.walker.Target != null) {
			Handles.color = Color.white;
			Handles.DrawLine(person.transform.position + Vector3.up,person.walker.Target.position);
		}
		Handles.EndGUI();
	}
}
