using System;
using UnityEngine;
using UnityEngine.Networking;

public class GridObjectMessage : MessageBase {
	public const short Code = 1001;
	
	public Vector3 position;
}
