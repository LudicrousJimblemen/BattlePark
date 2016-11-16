using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Grid : MonoBehaviour {
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	
	public int PlayerId;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
	
	public GridObjects Objects;
	
	void Start() {
		PlayerId = int.Parse(name.Substring(4, name.Length - 4));
		Objects = new GridObjects();
	}
	
	public string Serialize() {
		return JsonConvert.SerializeObject(Objects);
	}
	public void Deserialize(string message) {
		Objects = JsonConvert.DeserializeObject<GridObjects>(message);
	}
	
	//TEMPORARY GIZMOS STUFF
	
	[SerializeField][Range(-10, 10)] private int testX;
	[SerializeField][Range(-10, 10)] private int testY;
	[SerializeField][Range(-10, 10)] private int testZ;
	void OnDrawGizmos() {
		if (Objects.ObjectAt(new Vector3(testX, testY, testZ)) != null) {
			Gizmos.color = Color.green;
		} else if (Objects.Occupied(new Vector3(testX, testY, testZ))) {
			Gizmos.color = Color.blue;
		} else {
			Gizmos.color = Color.white;
		}
		
		Gizmos.DrawSphere(new Vector3(testX * GridXZ, testY * GridY, testZ * GridXZ), 0.5f);
	}
}