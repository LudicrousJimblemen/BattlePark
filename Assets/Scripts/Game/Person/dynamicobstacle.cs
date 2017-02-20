using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicobstacle : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		AstarPath.active.UpdateGraphs(GetComponent<Collider>().bounds);
	}
}
