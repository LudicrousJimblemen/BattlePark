using System.Collections.Generic;
using UnityEngine;

public class GridObjects <Position, GridObject>
{
	Dictionary<Vector3,GridObject> dict;
	public GridObjects () {
		dict = new Dictionary<Vector3,GridObject> ();
	}
}
