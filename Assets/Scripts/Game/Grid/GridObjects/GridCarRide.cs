using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridCarRide : GridObject {
	public GameObject Station;
	public GameObject Forward;
	public GameObject ForwardLeft;
	public GameObject ForwardRight;
	public GameObject ForwardDown;
	public GameObject ForwardUp;
	public GameObject Up;
	public GameObject Down;
	public GameObject UpForward;
	public GameObject DownForward;
	
	public List<GameObject> Track;
	
	private Vector3 nextTrackLocation;
	
	public override int Cost { get { return 1000000; } }

	public override Vector3[] OccupiedOffsets {
		get {
			return new[] { Vector3.zero };
		}
	}
}
