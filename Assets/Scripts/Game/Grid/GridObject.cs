using UnityEngine;
using System.Collections;

public abstract class GridObject : MonoBehaviour {
	#region Data Variables
	public Direction Direction;
	public Vector3 GridPosition;

	public abstract bool PlaceMultiple { get; set; }
	public abstract Vector3[] OccupiedOffsets { get; set; }
	#endregion

	public virtual void OnPlaced() { }
	public virtual void OnDemolished() { }

	public void UpdatePosition() {
		//transform.position = Grid.Instance.SnapToGrid (GridPosition, )
		transform.rotation = Quaternion.Euler(-90,0,(int)Direction * 90);
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
				return ReturnList;
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
