using System;
using System.Linq;
using UnityEngine;

public class GridAttractionFunRide : GridAttraction {
	public override Money Cost { get { return new Money(600, 00); } }
	
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] {
				Vector3.zero,
				new Vector3(0, 1, 0)
			};
		}
	}
	
	public override int MaximumPassengers { get { return 9; } }

	public override Attraction Attraction { get { return Attraction.FunRide; } }

	private void Awake() {
		PassengerSlots = new GameObject[MaximumPassengers];
		Passengers = new GameObject[MaximumPassengers];
	}
	
	private void Update() {
		if (!InCycle) {
			if (TimeSinceLastCycle >= MaximumWaitTime) {
				// TODO: Check if anyone is waiting to enter the ride, let them in
				StartCycle();
			} else {
				if (Passengers.Count(passenger => passenger != null) >= MinimumPassengers) {
					// TODO: Check if anyone is waiting to enter the ride, let them in
					StartCycle();
				}
			}
		}
	}
	
	public void StartCycle() {
		// do it
	}
}
