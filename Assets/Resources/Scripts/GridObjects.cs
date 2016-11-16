using System.Collections.Generic;
using UnityEngine;

public class GridObjects {
	private Dictionary<Vector3, GridObject> dictionary = new Dictionary<Vector3, GridObject>();
	
	public int Count { get { return dictionary.Count; } }
	
	public void Add(Vector3 position, GridObject gridObject) {
		dictionary.Add(position, gridObject);
	}
	
	public void Remove(Vector3 position) {
		dictionary.Remove(position);
	}
	
	/// <summary>
	/// Searches for an object whose origin is at the specified location, ignoring size.
	/// </summary>
	public GridObject ObjectAt(Vector3 location) {
		GridObject foundObject;
		if (dictionary.TryGetValue(location, out foundObject)) {
			return foundObject;
		} else {
			return null;
		}
	}
	
	
	/// <summary>
	/// Searches for an object which occupies a location.
	/// </summary>
	public GridObject ObjectIn(Vector3 location) {
		GridObject foundObject;
		if (dictionary.TryGetValue(location, out foundObject)) {
			return foundObject;
		} else {
			foreach (var item in dictionary.Values) {
				foreach (var offset in item.OccupiedOffsets) {
					if (new Vector3(item.X, item.Y, item.Z) + offset == location) {
						return item;
					}
				}
			}
			
			return null;
		}
	}
	
	/// <summary>
	/// Tests if a location is occupied
	/// </summary>
	public bool Occupied(Vector3 location) {
		return ObjectIn(location) != null;
	}
}
