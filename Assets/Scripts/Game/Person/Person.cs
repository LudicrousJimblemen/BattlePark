using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Person : MonoBehaviour {
	[HideInInspector]
	public PersonProperty[] properties = new PersonProperty[] {
			new PersonProperty("name","default",typeof(string)),
			// Represents how much money a person has in cents
			new PersonProperty("money",0,typeof(int),0,int.MaxValue),
			new PersonProperty("hunger",0,typeof(float)),
			new PersonProperty("thirst",0,typeof(float)),
			new PersonProperty("nausea",0,typeof(float)),
			new PersonProperty("bathroomosity",0,typeof(float)),
			new PersonProperty("happiness",100,typeof(float)),
			new PersonProperty("anger",0,typeof(float)),
			new PersonProperty("suspicion",0,typeof(float))
		};


	// re-roll properties
	public void reroll() {
		SetProperty("name",GenerateUsername());
		SetProperty("money",UnityEngine.Random.Range(2000,10001));
		SetProperty("hunger",UnityEngine.Random.Range(0,21f));
		SetProperty("thirst",UnityEngine.Random.Range(0,16f));
		SetProperty("nausea",UnityEngine.Random.Range(0,3f));
		SetProperty("bathroomosity",UnityEngine.Random.Range(0,11f));
		SetProperty("happiness",UnityEngine.Random.Range(50f,100f));
		SetProperty("anger",UnityEngine.Random.Range(0,5f));
		SetProperty("suspicion",UnityEngine.Random.Range(0,2));
	}

	private void Awake() {
		reroll();
	}

	public object GetProperty(string key) {
		for(int i = 0; i < properties.Length; i++) {
			if(properties[i].Key == key.ToLower()) {
				return properties[i].Value;
			}
		}
		return null;
	}
	public void SetProperty(string key,object value) {
		print(properties.Length);
		for(int i = 0; i < properties.Length; i++) {
			if(properties[i].Key == key.ToLower()) {
				properties[i].Value = value;
			}
		}
	}

	public List<Thought> Thoughts = new List<Thought>();

	private void Start() {
		Thoughts.Add(new Thought("person.thoughts.wantFood.ludicrous","MINDBLOWING MACARONI"));
	}

	private static string GenerateUsername() {
		const string consonants = "bbbbbbbbbbbbbbbbbbbbbbbcdffgghjklmnppppppppppppppppppppppqrsssstvwxzzzz";
		const string vowels = "aaeeiiiiiooooooooooouuuuuuuuuuuuuy";
		int type = Mathf.RoundToInt(UnityEngine.Random.Range(0,1));

		string returnedName = String.Empty;
		for(int i = 0; i < 14; i++) {
			if(i != 7) {
				float chance = UnityEngine.Random.value;
				bool upper = (i == 0 || returnedName[i - 1].Equals(' '));
				string source;
				if(type == 0) {
					source = consonants;
					if(upper)
						source = source.ToUpper();
					returnedName += source.ElementAt(UnityEngine.Random.Range(0,consonants.Length));
					if(chance <= 0.5) {
						type = 1;
					}
				} else {
					source = vowels;
					if(upper)
						source = source.ToUpper();
					returnedName += source.ElementAt(UnityEngine.Random.Range(0,vowels.Length));
					if(chance <= 0.6) {
						type = 0;
					}
				}
			} else {
				returnedName += " ";
			}
		}

		return returnedName;
	}
}