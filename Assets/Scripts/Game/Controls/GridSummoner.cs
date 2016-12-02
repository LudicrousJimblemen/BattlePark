using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using BattlePark.Core;

namespace BattlePark {
	public class GridSummoner : MonoBehaviour {
		public List<GameObject> GridObjects;
		
		public List<string> Hotbar;
		
		public GridOverlay GridOverlay;
		public GameObject VerticalConstraint;
		
		private Client client;
		
		private GridPlaceholder currentPlaceholder;
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerGridObjectPlacedNetMessage>(OnServerGridObjectPlaced);
		}
		
		private void Update() {
			for (int i = 0; i < Hotbar.Count; i++) {
				if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
					if (currentPlaceholder != null) {
						Destroy(currentPlaceholder.gameObject);
					}
					
					currentPlaceholder = SummonPlaceholder(Hotbar[i]).GetComponent<GridPlaceholder>();
					currentPlaceholder.name = Hotbar[i];
				}
			}
			
			GridOverlay.ShowGrid = currentPlaceholder != null;
			
			if (currentPlaceholder != null) {
				VerticalConstraint.gameObject.SetActive(Input.GetKey(KeyCode.LeftControl));
				if (Input.GetKeyDown(KeyCode.LeftControl)) {
					Vector3 correctedPosition = new Vector3 {
						x = Camera.main.transform.position.x,
						y = 0,
						z = Camera.main.transform.position.z
					};
					VerticalConstraint.transform.position = currentPlaceholder.transform.position;
					VerticalConstraint.transform.rotation = Quaternion.LookRotation (currentPlaceholder.transform.position - correctedPosition) * Quaternion.Euler (-90,0,0);
				}
				
				currentPlaceholder.Position(Input.mousePosition, Input.GetKey(KeyCode.LeftControl));
				currentPlaceholder.Snap();
				
				if (Input.GetKeyDown(KeyCode.Escape)) {
					Destroy(currentPlaceholder.gameObject);
				}
				
				if (Input.GetKeyDown(KeyCode.Z)) {
					currentPlaceholder.Rotate(-1);
				} else if (Input.GetKeyDown(KeyCode.X)) {
					currentPlaceholder.Rotate(1);
				}
				
				if (Input.GetMouseButtonDown(0)) {
					if (currentPlaceholder.PlaceObject()) {
						currentPlaceholder = null;
					}
				}
			}
		}
		
		private GameObject SummonPlaceholder(string gridObjectName) {	
			return (GameObject)Instantiate(GridObjects.First(x => x.name == gridObjectName));
		}
		
		private void OnServerGridObjectPlaced(ServerGridObjectPlacedNetMessage message) {
			GridObject newObject = Instantiate(GridObjects.First(x => x.name == message.Type)).GetComponent<GridObject>();
			Destroy(newObject.GetComponent<GridPlaceholder>());
			newObject.Grid = FindObjectOfType<Grid>();
			newObject.Owner = message.Sender.Id;
			newObject.Deserialize(message.GridObject);
			newObject.UpdatePosition();
			newObject.OnPlaced();
			FindObjectOfType<Grid>().Objects.Add(newObject.GridPosition(), newObject);
		}
	}
}