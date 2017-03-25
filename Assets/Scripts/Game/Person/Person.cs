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

	public PathWalker Walker;
	
	private CharacterController controller;
	
	private void Awake() {
		Name = GenerateName();
		// between 20µ and 100µ (inclusive)
		Money = (Money)UnityEngine.Random.Range(1000, 500001);
		Hunger = UnityEngine.Random.Range(0, 20f);
		Thirst = UnityEngine.Random.Range(0, 15f);
		Nausea = UnityEngine.Random.Range(0, 2f);
		Urgency = UnityEngine.Random.Range(0, 10f);
		Mood = UnityEngine.Random.Range(50f, 100f);
		Suspicion = UnityEngine.Random.Range(0, 1f);
		
		Walker = GetComponent<PathWalker>();
		controller = GetComponent<CharacterController>();
		GetComponentInChildren<SkinnedMeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
	}

	private void Start() {
		GameManager.Instance.Guests.Add(this);
		
		// TODO make this real
		
		Desires.Enqueue(new DesireAttraction(Attraction.FunRide));
		Desires.Enqueue(new DesireFood(ItemFood.Macaroni));
	}
	
	private void Update() {
		Walker.Influenceable = !InAttraction;
		SeenObjects = Grid.Instance.Objects.Dictionary.Values.Where(x => !x.GetType().IsAssignableFrom(typeof(GridPath))).ToList();
		
		// TODO fix - magnitude is often nonzero even when not moving, e.g. on a funride
		Hunger += 0.002f * controller.velocity.magnitude;
		Walker.Speed = Mathf.Lerp(5f, 1.5f, Hunger / 40f);
		
		if (Desires.Any() && !InAttraction && Walker.tag != "Exit") {
			Desire firstDesire = Desires.Peek();
			// TODO optimise - it's not necessary to perform these operations every frame
			switch ((DesireType)firstDesire) {
				case DesireType.Food:
					DesireFood foodDesire = (DesireFood)firstDesire;
					if (foodDesire.Target != null) {
						Walker.Target = foodDesire.Target.transform;
					} else {
						GridVendor Target = null;
						IEnumerable<GridVendor> valid = SeenObjects.OfType<GridVendor>().Where(vendor => vendor.Product is ItemFood);
						if (valid.Any()) {
							if (foodDesire.Food != null) {
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
						if (Target != null) {
							Walker.Target = Target.transform;
						}
					}
					if (Walker.Target != null && (Walker.Target.position - this.transform.position).sqrMagnitude < 7) {
						if (Walker.Target.GetComponent<GridVendor>().SellTo(this)) {
							Desires.Dequeue();
							Walker.Stop();
						}
					}
					break;
				case DesireType.Attraction:
					DesireAttraction attractionDesire = (DesireAttraction)firstDesire;
					if (attractionDesire.Target != null) {
						Walker.Target = attractionDesire.Target.transform;
					} else {
						GridAttraction Target = null;
						IEnumerable<GridAttraction> valid = SeenObjects.OfType<GridAttraction>();
						if (valid.Any()) {
							if (attractionDesire.Attraction != null) {
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
						if (Target != null) {
							Walker.Target = Target.GetComponent<GridAttraction>().Entrance;
						}
					}
					if (Walker.Target != null && (Walker.Target.position - this.transform.position).sqrMagnitude < 7) {
						if (Walker.Target.parent.GetComponent<GridAttraction>().Admit(this)) {
							Desires.Dequeue();
						}
					}
					break;
			}
		} else {
			// wander
		}
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