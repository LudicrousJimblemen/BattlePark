using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	
	public Vector3[] ParkCenters;
	
	public GridObject[] Objects;

	void Awake() {
		if(FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
}
