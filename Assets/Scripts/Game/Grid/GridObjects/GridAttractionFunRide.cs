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
				/* TODO make these the real offsets later
				new Vector3(1, 0, 0),
				new Vector3(1, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(-1, 0, 0),
				new Vector3(-1, 0, -1),
				new Vector3(0, 0, -1),
				new Vector3(1, 0, -1),
				new Vector3(-1, 0, 1),
				*/
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
		for (float i = 0; i < 1f; i+= Time.deltaTime) {
			foreach (var slot in PassengerSlots) {
				slot.transform.parent.localPosition = new Vector3(0, Random.value * Mathf.SmoothStep(0, 0.025f, i), 0);
			}
			yield return null;
		}
		for (float i = 0; i < 8; i+= Time.deltaTime) {
			foreach (var slot in PassengerSlots) {
				slot.transform.parent.localPosition = new Vector3(0, Random.value * 0.025f, 0);
			}
			yield return null;
		}
		for (float i = 0; i < 1f; i+= Time.deltaTime) {
			foreach (var slot in PassengerSlots) {
				slot.transform.parent.localPosition = new Vector3(0, Random.value * Mathf.SmoothStep(0.025f, 0, i), 0);
			}
			yield return null;
		}
		EndCycle();
	}
}
