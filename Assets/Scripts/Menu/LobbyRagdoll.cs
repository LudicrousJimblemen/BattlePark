using System.Linq;
using UnityEngine;

namespace BattlePark.Menu {
	public class LobbyRagdoll : MonoBehaviour {
		public Rigidbody Waist;
		[Range(60, 360)]
		public int DestroyTime = 240;
	
		private int timer;
	
		private void Start() {
			Waist.AddForceAtPosition(new Vector3(
				Random.Range(-30f, 30f),
				Random.Range(110f, 120f),
				Random.Range(35f, 45f)
			), transform.position + new Vector3(Random.Range(-0.15f, 0.15f), -0.1f, -0.25f), ForceMode.Impulse);
		}
	
		private void Update() {
			if (timer == DestroyTime) {
				Waist.AddForce(new Vector3(
					0,
					Random.Range(200f, 300f),
					0
				), ForceMode.Impulse);
			}
			if (timer == DestroyTime + 30) {
				Destroy(gameObject);
			}
		
			timer++;
		}
	}
}