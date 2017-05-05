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
	
	[Header("BuildRollercoaster")]
	public GameObject Coaster;
	
	private void Awake() {
		if(FindObjectsOfType<BodgeManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
		
		if (Application.loadedLevelName == "PostIntro") {
			StartCoroutine(PostIntro());
		}
		if (Application.loadedLevelName == "BuildRollercoaster") {
			StartCoroutine(BuildRollercoaster());
		}
	}
	
	private IEnumerator PostIntro() {
		for (int i = 0; i < 90; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity);
			yield return new WaitForSeconds(0.05f);
		}
	}
	
	private IEnumerator BuildRollercoaster() {
		List<GameObject> coasterParts = new List<GameObject>();
		for (int i = 0; i < Coaster.transform.childCount; i++) {
			coasterParts.Add(Coaster.transform.GetChild(i).gameObject);
		}
		
		foreach (var part in coasterParts) {
			part.SetActive(false);
		}
		
		for (int i = 0; i < 90; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity);
		}
		
		yield return new WaitForSeconds(5f);
		
		foreach (var part in coasterParts) {
			part.SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	private void OnGUI() {
		if (Application.isEditor) {
			GUI.Label(new Rect(12f, 12f, 200f, 200f), (Time.frameCount * Time.timeScale).ToString());
		}
	}
}
