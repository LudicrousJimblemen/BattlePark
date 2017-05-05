using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class BodgeLifetime : MonoBehaviour {
	private int timer;
	private void Update() {
		timer++;
		
		if (timer > 2 * 60) {
			timer = 0;
			gameObject.SetActive(false);
		}
	}
}
