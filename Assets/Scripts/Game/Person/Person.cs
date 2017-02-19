using UnityEngine;
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
	public int Money = UnityEngine.Random.Range(2000, 10001);

	[Range(0, 100f)]
	public float Hunger = UnityEngine.Random.Range(0, 21f);
	[Range(0, 100f)]
	public float Thirst = UnityEngine.Random.Range(0, 16f);
	[Range(0, 100f)]
	public float Nausea = UnityEngine.Random.Range(0, 3f);
	[Range(0, 100f)]
	public float Bathroomosity = UnityEngine.Random.Range(0, 11f);

	[Range(0, 100f)]
	public float Happiness = UnityEngine.Random.Range(50f, 100f);
	[Range(0, 100f)]
	public float Anger = UnityEngine.Random.Range(0, 5f);

	[Range(0, 100f)]
	public float Suspicion = UnityEngine.Random.Range(0, 2f);

	public List<Thought> Thoughts = new List<Thought>();

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