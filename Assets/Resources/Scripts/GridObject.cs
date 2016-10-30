using System;
using UnityEngine;

public class GridObject : MonoBehaviour {
	public bool Active;
	
	#region GridObject Data
	public Direction Direction;
	public Vector3 Position;
	#endregion
	
	#region Scenery Data
	public bool IsScenery;
	public bool IsNice;
	#endregion
	
	public virtual void Start() {
		//
	}
	
	public virtual void Update() {
		Active = !GetComponent<GridPlaceholder>();

		transform.rotation = Quaternion.Euler(-90, 0, (int)Direction * 90);
		
		if (Active) {
			transform.position = Position;
		}
	}
}