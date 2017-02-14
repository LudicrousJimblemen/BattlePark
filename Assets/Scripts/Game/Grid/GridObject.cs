using UnityEngine;
using System.Collections;

public abstract class GridObject : MonoBehaviour {
	#region Data Variables
	public Direction Direction { get; set; }
	public Vector3 GridPosition { get; set; }

	public abstract bool PlaceMultiple { get; }
	public abstract Vector3[] OccupiedOffsets { get; }
	#endregion
	
	public void Start () {
		OnPlaced ();
	}

	public virtual void OnPlaced() { }
	public virtual void OnDemolished() { }

	public Vector3 GetPosition() {
		return Grid.Instance.SnapToGrid (GridPosition);
	}
	
	public Quaternion GetRotation() {
		return Quaternion.Euler(-90,0,(int)Direction * 90);
	}

	public Vector3[] RotatedOffsets() {
		Vector3[] ReturnList = new Vector3[OccupiedOffsets.Length];
		//Default is south
		//	multiply x and z by 1
		//East
		//	x becomes z, z become x
		//North
		//	multiply x and z by -1
		//West
		//	x becomes -z, z becomes -x
		switch(Direction) {
			case Direction.East:
				for(int i = 0; i < OccupiedOffsets.Length; i++) {
					ReturnList[i] = new Vector3(
						OccupiedOffsets[i].z,
						OccupiedOffsets[i].y,
						-OccupiedOffsets[i].x
					);
				}
				break;
			case Direction.North:
				for(int i = 0; i < OccupiedOffsets.Length; i++) {
					ReturnList[i] = new Vector3(
						-OccupiedOffsets[i].x,
						OccupiedOffsets[i].y,
						-OccupiedOffsets[i].z
					);
				}
				break;
			case Direction.West:
				for(int i = 0; i < OccupiedOffsets.Length; i++) {
					ReturnList[i] = new Vector3(
						-OccupiedOffsets[i].z,
						OccupiedOffsets[i].y,
						OccupiedOffsets[i].x
					);
				}
				break;
			case Direction.South:
				ReturnList = OccupiedOffsets;
				break;
			default:
				return null;
		}
		return ReturnList;
	}
}
