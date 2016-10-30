using System;
using UnityEngine;
using UnityEngine.Networking;

public class GridObjectPlacedNetMessage : MessageBase {
	public const short Code = 1001;
	
	public int ConnectionId;
	
	public string Type;
	
	#region GridObject Data
	public Direction Direction;
	public Vector3 Position;
	#endregion
	
	#region Scenery Data
	public bool IsScenery;
	public bool IsNice;
	#endregion
}