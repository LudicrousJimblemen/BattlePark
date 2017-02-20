using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class GridObject : NetworkBehaviour {
	#region Data Variables

	public Direction Direction { get; set; }
	public Vector3 GridPosition { get; set; }

	public abstract Vector3[] OccupiedOffsets { get; }

	public virtual bool PlaceMultiple { get { return false; } }
	public virtual bool CanRotate { get { return true; } }

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
		return Quaternion.Euler(-90, 0, (int) Direction * 90);
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
			ReturnList[i] *= Grid.Instance.GridStepXZ;
		}
		return ReturnList;
	}

	public virtual bool Valid (Vector3 position, Direction direction, int player) {
		return Grid.Instance.IsValid(position,RotatedOffsets(direction),player);
	}

	private void OnDrawGizmos() {
		foreach (Vector3 offset in RotatedOffsets()) {
			Gizmos.DrawSphere(offset + GridPosition, 0.1f);
		}
		//UnityEditor.Handles.color = Color.white;
		//UnityEditor.Handles.Label(GridPosition + Vector3.up * 2,((int)Direction).ToString());
	}
}
