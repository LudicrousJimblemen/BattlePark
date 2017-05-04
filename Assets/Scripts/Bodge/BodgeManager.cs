using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class BodgeManager : MonoBehaviour {
	public static BodgeManager Instance;
	public Vector3[] ParkCenters;
	public Vector3[] ParkGates;
	public NodeGraph[] Graphs;

	public GameObject[] PlayerObjectParents = new GameObject[2];
	public GameObject PersonObj;

	public GridObject[] Objects;
	public List<Person> Guests;

	private int timer = 0;
	
	private void Awake() {
		if(FindObjectsOfType<BodgeManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
	
	private void Update() {
		timer++;
		print(timer);
	}
}
