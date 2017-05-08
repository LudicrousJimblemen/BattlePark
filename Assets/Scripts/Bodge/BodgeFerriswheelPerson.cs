using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class BodgeFerriswheelPerson : MonoBehaviour {
	private Rigidbody rigidbody;
	public GameObject ExplosionPrefab;
	
	private void Awake() {
		rigidbody = GetComponent<Rigidbody>();
	}
	
	private void OnTriggerEnter(Collider collider) {
		rigidbody.isKinematic = true;
		Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
		var infidels = gameObject;
		Destroy(infidels);
	}
}
