using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	
	public Vector3[] ParkCenters;

	void Awake () {
		if(FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}

	void OnDrawGizmos () {
		if(ParkCenters == null)
			return;
		for (int i = 0; i < ParkCenters.Length; i++) {
			Gizmos.DrawSphere(ParkCenters[i],0.25f);
			//UnityEditor.Handles.Label(ParkCenters[i],(i + 1).ToString());
		}
	}
}
