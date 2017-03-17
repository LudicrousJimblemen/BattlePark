using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class Person : NetworkBehaviour {
	/// <summary>
	/// Represents the name of the person (in-game identity).
	/// </summary>
	public string Name = "Greg";

	/// <summary>
	/// Represents how much money a person has.
	/// </summary>
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
	
	/// <summary>
	/// If a person is currently in an attraction.
	/// </summary>
	public bool InAttraction;

	public Queue<Desire> Desires = new Queue<Desire>();
	public List<Thought> Thoughts = new List<Thought>();
	public List<GridObject> SeenObjects = new List<GridObject>();
	public List<InventoryItem> Inventory = new List<InventoryItem>();

	public PathWalker walker;

	private void Awake() {
		Reroll();
		walker = GetComponent<PathWalker>();
	}

	private void Start() {
		GameManager.Instance.Guests.Add(this);
		
		// TODO make this real
		
		Desires.Enqueue(new DesireAttraction(Attraction.FunRide));
		Desires.Enqueue(new DesireFood(ItemFood.Macaroni));
		print (Money);
		print (Money.Value);
	}
	
	private void Update () {
		SeenObjects = Grid.Instance.Objects.Dictionary.Values.Where(x => !x.GetType().IsAssignableFrom(typeof(GridPath))).ToList();
		
		if (Desires.Any() && !InAttraction) {
			Desire firstDesire = Desires.Peek();
			//TODO optimise - it's not necessary to perform these operations every frame
			switch ((DesireType)firstDesire) {
				case DesireType.Food:
					DesireFood foodDesire = (DesireFood)firstDesire;
					if (foodDesire.Target != null) {
						walker.Target = foodDesire.Target.transform;
					} else {
						GridVendor Target = null;
						IEnumerable<GridVendor> valid = SeenObjects.OfType<GridVendor>().Where(vendor => vendor.Product is ItemFood);
						if(valid.Any()) {
							if(foodDesire.Food != null) {
								Target = valid
									.Where(vendor => vendor.Product.Id == foodDesire.Food.Id)
									.OrderBy(vendor => (vendor.transform.position - this.transform.position).sqrMagnitude)
									.First();
							} else {
								Target = valid
									.OrderBy(vendor => (vendor.transform.position - this.transform.position).sqrMagnitude)
									.First();
							}
						}
						if(Target != null) {
							walker.Target = Target.transform;
						}
					}
					if (walker.Target != null && (walker.Target.position - this.transform.position).sqrMagnitude < 7) {
						if (walker.Target.GetComponent<GridVendor>().SellTo(this)) {
							Desires.Dequeue();
							Thoughts.Add(new Thought("person.thoughts.likeFood.ludicrous", ((ItemFood) walker.Target.GetComponent<GridVendor>().Product).PluralString));
							walker.Stop();
						}
					}
					break;
				case DesireType.Attraction:
					DesireAttraction attractionDesire = (DesireAttraction)firstDesire;
					if (attractionDesire.Target != null) {
						walker.Target = attractionDesire.Target.transform;
					} else {
						GridAttraction Target = null;
						IEnumerable<GridAttraction> valid = SeenObjects.OfType<GridAttraction>();
						if(valid.Any()) {
							if(attractionDesire.Attraction != null) {
								Target = valid
									.Where(attraction => attraction.Attraction.Id == attractionDesire.Attraction.Id)
									.OrderBy(attraction => (attraction.transform.position - this.transform.position).sqrMagnitude)
									.First();
							} else {
								Target = valid
									.OrderBy(attraction => (attraction.transform.position - this.transform.position).sqrMagnitude)
									.First();
							}
						}
						if(Target != null) {
							walker.Target = Target.transform;
						}
					}
					if (walker.Target != null && (walker.Target.position - this.transform.position).sqrMagnitude < 7) {
						if (walker.Target.GetComponent<GridAttraction>().Admit(this)) {
							Desires.Dequeue();
						}
					}
					break;
			}
		} else {
			// wander
		}
	}

	private void Reroll() {
		Name = GenerateName();
		// between 20µ and 100µ (inclusive)
		Money = (Money)UnityEngine.Random.Range(1000, 500001);
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