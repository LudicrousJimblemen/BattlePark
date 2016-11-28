using System.Linq;
using UnityEngine;

namespace BattlePark {
	public class LobbyBackground : MonoBehaviour {
		public GameObject Ragdoll;
		[Range(15, 180)]
		public int RagdollInterval = 15;
	
		public Vector3 SpawnPosition = new Vector3(0, 12f, -2f);
	
		private int timer;
	
		private void Update() {
			if (timer % RagdollInterval == 0) {
				Instantiate(Ragdoll, SpawnPosition, Quaternion.Euler(
					Random.Range(0, 360f),
					Random.Range(0, 360f),
					Random.Range(0, 360f)
				));
			}
		
			timer++;
		}
	}
}