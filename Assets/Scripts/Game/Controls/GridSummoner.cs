using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using BattlePark.Core;
using BattlePark.Menu;

namespace BattlePark {
	public class GridSummoner : MonoBehaviour {
		public List<GameObject> GridObjects;
		
		public List<string> Hotbar;
		
		public GameGUI GUI;
		
		public GridOverlay GridOverlay;
		public GameObject VerticalConstraint;
		
		private Client client;
		
		private GridPlaceholder currentPlaceholder;
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerGridObjectPlacedNetMessage>(OnServerGridObjectPlaced);
			
			GUI.PathsButton.onClick.AddListener(() => {
				currentPlaceholder = SummonGridObjectPlaceholder("Path").GetComponent<GridPlaceholder>();
			});
		}
		
		private void Update() {
			for (int i = 0; i < Hotbar.Count; i++) {
				if (Input.GetKeyDown(KeyCode.Alpha1 + i)) {
					if (currentPlaceholder != null) {
						Destroy(currentPlaceholder.gameObject);
					}
					
					currentPlaceholder = SummonGridObjectPlaceholder(Hotbar[i]).GetComponent<GridPlaceholder>();
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
					currentPlaceholder = null;
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
		
		public GameObject SummonGridObjectPlaceholder(string gridObjectName) {
			GameObject returned = SummonGridObjectPlaceholder(GridObjects.First(x => x.name == gridObjectName));
			returned.name = gridObjectName;
			return returned;
		}
		
		public GameObject SummonGridObjectPlaceholder(GameObject gridObject) {
			GameObject returned = (GameObject)Instantiate(gridObject);
			returned.name = gridObject.name;
			return returned;
		}
		
		public GameObject SummonGridObject(string gridObjectName, long userId) {
			GameObject newObject = SummonGridObjectPlaceholder(gridObjectName);
			newObject.name = gridObjectName;
			
			GridObject newGridObject = newObject.GetComponent<GridObject>();
			Destroy(newObject.GetComponent<GridPlaceholder>());
			newGridObject.Grid = FindObjectOfType<Grid>();
			newGridObject.Owner = userId;
			return newObject;
		}
		
		public GameObject SummonGridObject(GameObject gridObject, long userId) {
			GameObject newObject = SummonGridObjectPlaceholder(gridObject);
			newObject.name = gridObject.name;
			
			GridObject newGridObject = newObject.GetComponent<GridObject>();
			Destroy(newObject.GetComponent<GridPlaceholder>());
			newGridObject.Grid = FindObjectOfType<Grid>();
			newGridObject.Owner = userId;
			return newObject;
		}
		
		private void OnServerGridObjectPlaced(ServerGridObjectPlacedNetMessage message) {
			GridObject newObject = SummonGridObject(message.Type, message.Sender.Id).GetComponent<GridObject>();
			
			newObject.Deserialize(message.GridObject);
			newObject.UpdatePosition();
			
			newObject.OnPlaced();
			
			FindObjectOfType<Grid>().Objects.Add(newObject.GridPosition(), newObject);
		}
	}
}