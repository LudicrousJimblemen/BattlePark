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
	/// <param name="location">Location to check.</param>
	/// <returns>The GridObject found if there is one, null otherwise.</returns>
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
	/// <param name="location">Location to check.</param>
	/// <returns>The GridObject found if there is one, null otherwise.</returns>
	public GridObject ObjectIn(Vector3 location) {
		GridObject foundObject;
		if (dictionary.TryGetValue(location, out foundObject)) {
			return foundObject;
		} else {
			foreach (var item in dictionary.Values) {
				foreach (var offset in item.RotatedOffsets ()) {
					if (item.GridPosition() + offset == location) {
						return item;
					}
				}
			}
			return null;
		}
	}
	
	/// <summary>
	/// Tests if a location is the origin of an object.
	/// </summary>
	/// <param name="location">Location to check.</param>
	/// <returns>True if an object's origin is at the location, false otherwise.</returns>
	public bool OccupiedAt(Vector3 location) {
		return ObjectAt(location) != null;
	}
	
	/// <summary>
	/// Tests if a location is occupied.
	/// </summary>
	/// <param name="location">Location to check.</param>
	/// <returns>True if an object occupies the location, false otherwise.</returns>
	public bool OccupiedIn(Vector3 location) {
		return ObjectIn(location) != null;
	}
	
	/// <summary>
	/// Tests if a position and its occupation offsets will
	/// </summary>
	/// <param name="location">The starting position to test</param>
	/// <param name="offsets">The occupation offsets to test.</param>
	/// <returns>True if the given position or its offsets intersect with an existing object.</returns>
	public bool WillIntersect(Vector3 location, Vector3[] offsets) {
		if (OccupiedIn(location)) {
			return true;
		}
		
		foreach (var offset in offsets) {
			if (OccupiedIn(location + offset)) {
				return true;
			}
		}
		
		return false;
	}
	
	public List<GridObject> AdjacentObjects(Vector3 location, bool corners = false) {
		List<GridObject> objects = new List<GridObject>();
	
		if (OccupiedAt(location + new Vector3(1, 0, 0))) objects.Add(ObjectAt(location + new Vector3(1, 0, 0)));
		if (OccupiedAt(location + new Vector3(-1, 0, 0))) objects.Add(ObjectAt(location + new Vector3(-1, 0, 0)));
		if (OccupiedAt(location + new Vector3(0, 0, 1))) objects.Add(ObjectAt(location + new Vector3(0, 0, 1)));
		if (OccupiedAt(location + new Vector3(0, 0, -1))) objects.Add(ObjectAt(location + new Vector3(0, 0, -1)));
		
		if (corners) {
			if (OccupiedAt(location + new Vector3(1, 0, 1))) objects.Add(ObjectAt(location + new Vector3(1, 0, 1)));
			if (OccupiedAt(location + new Vector3(-1, 0, -1))) objects.Add(ObjectAt(location + new Vector3(-1, 0, -1)));
			if (OccupiedAt(location + new Vector3(1, 0, -1))) objects.Add(ObjectAt(location + new Vector3(1, 0, -1)));
			if (OccupiedAt(location + new Vector3(-1, 0, 1))) objects.Add(ObjectAt(location + new Vector3(-1, 0, 1)));
		}
		
		return objects;
	}
}
