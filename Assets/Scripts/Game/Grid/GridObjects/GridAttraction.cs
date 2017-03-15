using System;
using System.Linq;
using UnityEngine;

public abstract class GridAttraction : GridObject {
	/// <summary>
	/// The type of attraction.
	/// </summary>
	public abstract Attraction Attraction { get; }

	/// <summary>
	/// The maximum number of people that can be on an attraction at once.
	/// </summary>
	public abstract int MaximumPassengers { get; }
	
	private bool inCycle;
	/// <summary>
	/// If the attraction is currently in a cycle.
	/// </summary>
	public bool InCycle {
		get { return inCycle; }
		protected set {
			if (inCycle != value) {
				inCycle = value;
				if (value == false) {
					lastCycle = DateTime.Now;
				}
			}
		}
	}
	
	private DateTime lastCycle = DateTime.Now;
	/// <summary>
	/// The time since the last cycle ended.
	/// </summary>
	public TimeSpan TimeSinceLastCycle { get { return DateTime.Now - lastCycle; } }
	
	/// <summary>
	/// An array of bones to which people can be parented when on an attraction.
	/// </summary>
	public GameObject[] PassengerSlots;
	
	/// <summary>
	/// An array of people on the attraction.
	/// </summary>
	public GameObject[] Passengers;
	
	/// <summary>
	/// The number of people an attraction must have before starting an attraction cycle.
	/// </summary>
	public int MinimumPassengers = 1;
	/// <summary>
	/// The maximum time before an attraction starts a cycle, regardless of the number of passengers.
	/// </summary>
	public TimeSpan MaximumWaitTime = new TimeSpan(0, 0, 15);
	/// <summary>
	/// The minimum time before an attraction starts a cycle, regardless of the number of passengers.
	/// </summary>
	public TimeSpan MinimumWaitTime = new TimeSpan(0, 0, 10);
	
	/// <summary>
	/// Tries to admit a person into the attraction.
	/// </summary>
	/// <param name="person">The person who would be admitted into the attraction.</param>
	/// <returns>True if the person is allowed into the attraction, false otherwise.</returns>
	public bool Admit(Person person) {
		if (!InCycle) {
			for (int i = 0; i < MaximumPassengers; i++) {
				if (Passengers[i] == null) {
					Passengers[i] = person.gameObject;
					person.GetComponent<Pathfinding.PathWalker>().enabled = false;
					person.transform.SetParent(PassengerSlots[i].transform, false);
					person.transform.localPosition = Vector3.zero;
					person.InAttraction = true;
					return true;
				}
			}
		}
		return false;
	}
	
	public void EndCycle() {
		for (int i = 0; i < MaximumPassengers; i++) {
			if (Passengers[i] != null) {
				Passengers[i].transform.SetParent(null, true);
				Passengers[i].GetComponent<Person>().InAttraction = false;
				Passengers[i].GetComponent<Pathfinding.PathWalker>().enabled = true;
				Passengers[i] = null;
			}
		}
		InCycle = false;
	}
	
	protected void Awake() {
		Attraction.Price = Attraction.DefaultPrice;
	}
}
