using UnityEngine;
using System.Linq;

public class GridPath : GridObject {
	public override Vector3[] OccupiedOffsets {
		get {
			return new[] { Vector3.zero };
		}
	}

	public override bool PlaceMultiple { get { return true; } }
	public override bool CanRotate { get { return false; } }

	public override void OnPlaced() {
		base.OnPlaced();
		for (int i = 0; i < 2; i ++) {
			for(int j = 0; j < 2; j++) {
				Vector3 nodePos = GetPosition() + new Vector3(-.75f,0,-.75f) + new Vector3(i*1.5f,0,j*1.5f);
                Instantiate(GameManager.Instance.PathNode,nodePos,Quaternion.identity,GameObject.FindGameObjectWithTag("PathNode").transform);
			}
		}
		AstarPath.active.Scan();
    }

	public override bool Valid(Vector3 position,Direction direction,int player) {
		//Debug.Log(Grid.Instance.Objects.AdjacentObjects(position).Count);
		return base.Valid(position,direction,player) && Grid.Instance.Objects.AdjacentObjects (position).Any (x => x.GetType () == typeof(GridPath));
	}
}
