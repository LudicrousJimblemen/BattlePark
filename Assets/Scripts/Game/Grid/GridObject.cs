using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class GridObject : NetworkBehaviour {
	#region Data Variables

	/// <summary>
	/// Direction in which the grid object faces.
	/// </summary>
	public Direction Direction { get; set; }
	/// <summary>
	/// Position of the grid object in terms of grid coordinates.
	/// </summary>
	public Vector3 GridPosition { get; set; }

	/// <summary>
	/// Positions - relative to the origin - which the grid object occupies.
	/// Used for validity checking when other objects are being placed.
	/// </summary>
	public abstract Vector3[] OccupiedOffsets { get; }

	/// <summary>
	/// Cost of the grid object, in cents.
	/// </summary>
	public virtual Money Cost { get { return new Money(); } }

	/// <summary>
	/// If the grid object is often placed multiple times.
	/// Used for objects like paths so that a user need not summon a placeholder multiple times.
	/// </summary>
	public virtual bool PlaceMultiple { get { return false; } }
	/// <summary>
	/// If the grid object is allowed to rotate when being placed.
	/// </summary>
	public virtual bool CanRotate { get { return true; } }

	/// <summary>
	/// The player number of the owner of this grid object.
	/// </summary>
	public int Owner;

	#endregion

	public void Start() {
		OnPlaced();
	}

	public virtual void OnPlaced() {
		Grid.Instance.Objects.Add(GetPosition(), this);
	}
	public virtual void OnDemolished() { }

	public Vector3 GetPosition() {
		return Grid.Instance.SnapToGrid(transform.position, Owner);
	}

	public Quaternion GetRotation() {
		return Quaternion.Euler(0, (int) Direction * 90, 0);
	}

	public Vector3[] RotatedOffsets() {
		return RotatedOffsets(Direction);
	}
	public Vector3[] RotatedOffsets(Direction direction) {
		Vector3[] ReturnList = new Vector3[OccupiedOffsets.Length];
		// Default is north
		// 	multiply x and z by 1
		// East
		// 	x becomes z, z become x
		// North
		// 	multiply x and z by -1
		// West
		// 	x becomes -z, z becomes -x
		switch (direction) {
			case Direction.East:
				for (int i = 0; i < OccupiedOffsets.Length; i++) {
					ReturnList[i] = new Vector3(
						OccupiedOffsets[i].z,
						OccupiedOffsets[i].y,
						-OccupiedOffsets[i].x
					);
				}
				break;
			case Direction.South:
				for (int i = 0; i < OccupiedOffsets.Length; i++) {
					ReturnList[i] = new Vector3(
						-OccupiedOffsets[i].x,
						OccupiedOffsets[i].y,
						-OccupiedOffsets[i].z
					);
				}
				break;
			case Direction.West:
				for (int i = 0; i < OccupiedOffsets.Length; i++) {
					ReturnList[i] = new Vector3(
						-OccupiedOffsets[i].z,
						OccupiedOffsets[i].y,
						OccupiedOffsets[i].x
					);
				}
				break;
			case Direction.North:
				ReturnList = OccupiedOffsets;
				break;
			default:
				return null;
		}
		for (int i = 0; i < OccupiedOffsets.Length; i++) {
			ReturnList[i].x *= Grid.Instance.GridStepXZ;
			ReturnList[i].z *= Grid.Instance.GridStepXZ;
			ReturnList[i].y *= Grid.Instance.GridStepY;
		}
		return ReturnList;
	}
	
	public virtual bool Valid (Vector3 position, Direction direction, int player) {
		Vector3[] offsets = RotatedOffsets(direction);
		/*
		for (int i = 0; i < offsets.Length; i ++) {
			Vector3 offset = RotatedOffsets(direction)[i];
			//offset.x *= Grid.Instance.GridStepXZ;
			//offset.z *= Grid.Instance.GridStepXZ;
			//offset.y *= Grid.Instance.GridStepY;
		}
		*/
		return Grid.Instance.IsValid(position,offsets,player);
	}

	private void OnDrawGizmos() {
		foreach (Vector3 offset in RotatedOffsets()) {
			Gizmos.DrawWireCube(offset + GetPosition(), new Vector3(Grid.Instance.GridStepXZ,Grid.Instance.GridStepY,Grid.Instance.GridStepXZ));
			//UnityEditor.Handles.Label(offset + GetPosition(),(offset + GetPosition()).ToString());
		}
		
		Gizmos.color = Color.red;
		Gizmos.DrawRay (GetPosition () + Vector3.up * 3f, transform.forward);
		//UnityEditor.Handles.color = Color.white;
		//UnityEditor.Handles.Label(GridPosition + Vector3.up * 2,((int)Direction).ToString());
	}
}
