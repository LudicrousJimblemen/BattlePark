using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Pathfinding;

public class BodgePerson : MonoBehaviour {
	public PathWalker Walker;
	
	public Color Red;
	public Color Blue;
	
	public GameObject Macaroni;
	
	private void Awake() {
		Walker = GetComponent<PathWalker>();
	}

	private void Start() {
		if (Application.loadedLevelName == "Intro") {
			GetComponentInChildren<SkinnedMeshRenderer>().material.color = Blue;
			print("intro");
			
			Walker.SetDestination(new Vector3(-12.5f, 0, -6f));
			Walker.Speed = 5f;
			
			StartCoroutine(Intro());
		}
	}
	
	private IEnumerator Intro() {
		yield return new WaitForSeconds(3.1f);
		Macaroni.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		Walker.SetDestination(new Vector3(-8.5f, 0, 6f));
	}
}