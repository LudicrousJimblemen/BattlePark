using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridObjects : IEnumerable {
	private Dictionary<Vector3, GridObject> dictionary = new Dictionary<Vector3, GridObject>();
	
	public int Count { get { return dictionary.Count; } }
	
	public IEnumerator GetEnumerator() {
		return dictionary.GetEnumerator();
	}

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
		}
		
		return null;
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
				foreach (var offset in item.RotatedOffsets()) {
					if (item.GridPosition + offset == location) {
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
	/// Tests if a position and its occupation offsets will intersect with an existing objct
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
	/// <summary>
	/// Tests if there exists any object at any height of an x/z coordinate
	/// </summary>
	/// <param name="location">The position to test, y value automagically ignored</param>
	/// <returns>True if the test is true, yeah</returns>
	public bool HasVerticalClearance(Vector3 location) {
		throw new NotImplementedException();
	}

	public GridObject[] AdjacentObjects(Vector3 location, bool corners = false) {
		//List<GridObject> objects = new List<GridObject>();
		GridObject[] objects = new GridObject[corners ? 8 : 4];

		float step = Grid.Instance.GridStepXZ;

		objects[0] = ObjectAt(location + new Vector3(-1, 0, 0) * step); // west
		objects[1] = ObjectAt(location + new Vector3(1, 0, 0) * step); // east
		objects[2] = ObjectAt(location + new Vector3(0, 0, -1) * step); // south
		objects[3] = ObjectAt(location + new Vector3(0, 0, 1) * step); // north
		
		if (corners) {
			objects[4] = (ObjectAt(location + new Vector3(-1, 0, -1) * step)); // southwest
			objects[5] = (ObjectAt(location + new Vector3(-1, 0, 1) * step)); // northwest
			objects[6] = (ObjectAt(location + new Vector3(1, 0, -1) * step)); // southeast
			objects[7] = (ObjectAt(location + new Vector3(1, 0, 1) * step)); // northeast
		}
		
		return objects;
	}
}