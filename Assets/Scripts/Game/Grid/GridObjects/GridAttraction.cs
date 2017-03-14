using System;
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
				if (value == false) {
					lastCycle = DateTime.Now;
				}
			}
		}
	}
	
	private DateTime lastCycle;
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
	public int MinimumPassengers;
	/// <summary>
	/// The maximum time before an attraction starts a cycle, regardless of the number of passengers.
	/// </summary>
	public TimeSpan MaximumWaitTime;
}
