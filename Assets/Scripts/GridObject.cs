using UnityEngine;

public class GridObject : MonoBehaviour
{
	private NetworkManager networkManager;
	
	void Start() {
		networkManager = FindObjectOfType<NetworkManager>();
		GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
	}
	
	void Update() {
		//PUT THE THINGS IN HERE?E?E?E?E
	}
}