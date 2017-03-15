
using System.Collections;
using System.Linq;
using UnityEngine;

public class GridAttractionFunRide : GridAttraction {
	public override Money Cost { get { return new Money(600, 00); } }
	
	// TODO Fix offsets
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
	
	private void Update() {
		if (!InCycle) {
			if (TimeSinceLastCycle >= MaximumWaitTime) {
				// TODO: Check if anyone is waiting to enter the ride, let them in
				StartCoroutine(StartCycle());
			} else {
				if (Passengers.Count(passenger => passenger != null) >= MinimumPassengers && TimeSinceLastCycle >= MinimumWaitTime) {
					// TODO: Check if anyone is waiting to enter the ride, let them in
					StartCoroutine(StartCycle());
				}
			}
		}
	}
	
	public IEnumerator StartCycle() {
		InCycle = true;
		for (int i = 0; i < 100; i++) {
			foreach (var slot in PassengerSlots) {
				slot.transform.localPosition = new Vector3(-Random.value * Mathf.SmoothStep(0, 2.5f, i / 100f), 0, 0);
			}
			yield return null;
		}
		for (int i = 0; i < 400; i++) {
			foreach (var slot in PassengerSlots) {
				slot.transform.localPosition = new Vector3(-Random.value * 2.5f, 0, 0);
			}
			yield return null;
		}
		for (int i = 0; i < 100; i++) {
			foreach (var slot in PassengerSlots) {
				slot.transform.localPosition = new Vector3(-Random.value * Mathf.SmoothStep(2.5f, 0, i / 100f), 0, 0);
			}
			yield return null;
		}
		EndCycle();
	}
}
