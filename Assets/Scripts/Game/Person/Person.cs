using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Person : MonoBehaviour {
	public string Name = GenerateUsername();
	
	/// <summary>
	/// Represents how much money a person has in cents.
	/// </summary>
	[Range(0, Int32.MaxValue)]
	public int Money = 0; // flat broke

	[Range(0, 100f)]
	public float Hunger = 0; // has no stomach, therefore doesn't feel hunger
	[Range(0, 100f)]
	public float Thirst = 0; // literally 100% water
	[Range(0, 100f)]
	public float Nausea = 0; // he's good, he puked before coming over
	[Range(0, 100f)]
	public float Bathroomosity = 0; // he made sure to take all the rest stops on the highway

	[Range(0, 100f)]
	public float Happiness = 100; // happiest man alive
	[Range(0, 100f)]
	public float Anger = 0; // see above
	[Range(0, 100f)]
	public float Suspicion = 0; // criminals don't exist, that's illegal

	public List<Thought> Thoughts = new List<Thought>();

	private void Awake () {
		Reroll();
	}

	private void Reroll () {
		Money = UnityEngine.Random.Range(2000,10001);
		Hunger = UnityEngine.Random.Range(0,20f);
		Thirst = UnityEngine.Random.Range(0,15f);
		Nausea = UnityEngine.Random.Range(0, 2f);
		Bathroomosity = UnityEngine.Random.Range(0,10f);

		Happiness = UnityEngine.Random.Range(50f,100f);
		Anger = UnityEngine.Random.Range(0,5f);
		Suspicion = UnityEngine.Random.Range(0,1f);
	}

	private void Start() {
		Thoughts.Add(new Thought("person.thoughts.wantFood.ludicrous", "MINDBLOWING MACARONI"));
	}

	private static string GenerateUsername() {
		const string consonants = "bbbbbbbbbbbbbbbbbbbbbbbcdffgghjklmnppppppppppppppppppppppqrsssstvwxzzzz";
		const string vowels = "aaeeiiiiiooooooooooouuuuuuuuuuuuuy";
		int type = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));

		string returnedName = String.Empty;
		for (int i = 0; i < 14; i++) {
			if (i != 7) {
				float chance = UnityEngine.Random.value;
				bool upper = (i == 0 || returnedName[i - 1].Equals(' '));
				string source;
				if (type == 0) {
					source = consonants;
					if (upper)
						source = source.ToUpper();
					returnedName += source.ElementAt(UnityEngine.Random.Range(0, consonants.Length));
					if (chance <= 0.5) {
						type = 1;
					}
				} else {
					source = vowels;
					if (upper)
						source = source.ToUpper();
					returnedName += source.ElementAt(UnityEngine.Random.Range(0, vowels.Length));
					if (chance <= 0.6) {
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