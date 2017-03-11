using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Person : NetworkBehaviour {
	/// <summary>
	/// Represents the name of the person (in-game identity).
	/// </summary>
	public string Name = "Greg";

	/// <summary>
	/// Represents how much money a person has.
	/// </summary>
	[Range(0, Int32.MaxValue)]
	[SyncVar]
	public Money Money;

	/// <summary>
	/// Represents how hungry a person is.
	/// </summary>
	/// <remarks>
	/// 0 is not hungry at all, 100 is starving (to death).
	/// </remarks>
	[Range(0, 100f)]
	[SyncVar]
	public float Hunger = 0;
	
	/// <summary>
	/// Represents how thirsty a person is.
	/// </summary>
	/// <remarks>
	/// 0 is 100% water, 100 is absolutely parched (to death).
	/// </remarks>
	[Range(0, 100f)]
	[SyncVar]
	public float Thirst = 0;
	
	/// <summary>
	/// Represents how nauseous a person is.
	/// </summary>
	/// <remarks>
	/// 0 is not nauseous at all, 100 means that the person will vomit on the spot.
	/// </remarks>
	[Range(0, 100f)]
	[SyncVar]
	public float Nausea = 0;
	
	/// <summary>
	/// Represents how badly a person needs to use to the bathroom.
	/// </summary>
	/// <remarks>
	/// 0 is perfectly fine, 100 means that the person will explode on the spot.
	/// </remarks>
	[Range(0, 100f)]
	[SyncVar]
	public float Urgency = 0;
	
	/// <summary>
	/// Represents how good the mood of a person is.
	/// </summary>
	/// <remarks>
	/// 0 is very mad, and 100 is very glad.
	/// </remarks>
	[Range(0, 100f)]
	[SyncVar]
	public float Mood = 100;
	
	/// <summary>
	/// Represents how suspicious a person is.
	/// </summary>
	/// <remarks>
	/// 0 is not suspicious at all, 100 means the person will explode on the spot.
	/// </remarks>
	[Range(0, 100f)]
	[SyncVar]
	public float Suspicion = 0;
	
	public Queue<Desire> Desires = new Queue<Desire>();
	public List<Thought> Thoughts = new List<Thought>();
	public List<GridObject> SeenObjects = new List<GridObject>();
	public List<InventoryItem> Inventory = new List<InventoryItem>();
	
	private AIPath aiPath;
	
	private void Awake() {
		Reroll();
		
		aiPath = GetComponent<AIPath>();
	}

	private void Start() {
		GameManager.Instance.Guests.Add(this);
		
		foreach (var gridObject in Grid.Instance.Objects) {
			SeenObjects.Add(gridObject);
		}
		
		Desires.Enqueue(new DesireFood(ItemFood.Macaroni));
		Thoughts.Add(new Thought("person.thoughts.wantFood.ludicrous", "inventoryItems.macaroni.singular"));
	}
	
	private void Update() {
		Desire firstDesire = Desires.Peek();
		
		DesireFood foodDesire = firstDesire as DesireFood;
		//TODO optimise - it's not necessary to perform these operations every frame
		if (foodDesire != null) {
			if (foodDesire.Target != null) {
				aiPath.target = foodDesire.Target.transform;
			} else {
				if (foodDesire.Food != null) {
					aiPath.target = SeenObjects.OfType<GridVendor>()
						.Where(vendor => vendor.Product is ItemFood)
						.Where(vendor => vendor.Product.Id == foodDesire.Food)
						.OrderBy(vendor => (vendor.transform.position - this.transform.position).sqrMagnitude)
						.First().transform;
				} else {
					aiPath.target = SeenObjects.OfType<GridVendor>()
						.Where(vendor => vendor.Product is ItemFood)
						.OrderBy(vendor => (vendor.transform.position - this.transform.position).sqrMagnitude)
						.First().transform;
				}
			}
			
			if ((aiPath.target.position - this.transform.position).sqrMagnitude < 5) {
				if (aiPath.target.GetComponent<GridVendor>().SellTo(this)) {
					Desires.Dequeue();
				}
			}
		}
	}
	
	private void Reroll() {
		Name = GenerateName();
		// between 20µ and 100µ (inclusive)
		Money = Money.FromSmall(UnityEngine.Random.Range(2000, 10001));
		Hunger = UnityEngine.Random.Range(0, 20f);
		Thirst = UnityEngine.Random.Range(0, 15f);
		Nausea = UnityEngine.Random.Range(0, 2f);
		Urgency = UnityEngine.Random.Range(0, 10f);
		Mood = UnityEngine.Random.Range(50f, 100f);
		Suspicion = UnityEngine.Random.Range(0, 1f);
	}

	private static string GenerateName() {
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