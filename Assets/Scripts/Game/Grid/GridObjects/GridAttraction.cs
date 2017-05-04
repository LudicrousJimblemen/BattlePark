using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Pathfinding;

public abstract class GridAttraction : GridObject {
	/// <summary>
	/// The type of attraction.
	/// </summary>
	public abstract Attraction Attraction { get; }
	
	/// <summary>
	/// The transform defining where persons will path towards to ride the ride 
	/// </summary>
	public Transform Entrance;
	
	/// <summary>
	/// The transform defining where exiting persons will path towards first
	/// </summary>
	public Transform Exit;
	
	private PathNode[] Nodes = new PathNode[2];

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
					person.GetComponent<UnityEngine.Networking.NetworkTransform>().enabled = false;
					person.Walker.Stop();
					person.Walker.enabled = false;
					person.transform.SetParent(PassengerSlots[i].transform, true);
					person.transform.localPosition = Vector3.zero;
					Passengers[i].GetComponent<CharacterController>().enabled = false;
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
				Passengers[i].transform.position = Exit.position;
				Passengers[i].GetComponent<CharacterController>().enabled = true;
				Passengers[i].GetComponent<UnityEngine.Networking.NetworkTransform>().enabled = true;
				Passengers[i].GetComponent<Person>().InAttraction = false;
				Passengers[i].GetComponent<Pathfinding.PathWalker>().enabled = true;
				//Passengers[i].GetComponent<Pathfinding.PathWalker>().Target = Exit;
				Passengers[i] = null;
			}
		}
		InCycle = false;
	}
	
	protected void Awake() {
		Attraction.Price = Attraction.DefaultPrice;
	}
	
	public override void OnPlaced() {
		base.OnPlaced();
		ServerAddNodes();
	}
	
	public override void OnDemolished() {
		ServerRemoveNodes();
		base.OnDemolished();
	}
	
	[Server]
	public void ServerAddNodes() {
		Pathfinding.NodeGraph graph = GameManager.Instance.Graphs[Owner-1];
		Nodes[0] = graph.AddNode(Entrance.position, graph.ScanDistance);
		Nodes[1] = graph.AddNode(Exit.position, graph.ScanDistance);
	}
	
	[Server]
	private void ServerRemoveNodes () {
		NodeGraph graph = GameManager.Instance.Graphs[Owner-1];
		foreach (PathNode node in Nodes) {
			graph.RemoveNode (node);
		}
	}
}
