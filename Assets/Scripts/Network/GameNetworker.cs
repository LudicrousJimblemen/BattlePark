using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using BattlePark.Core;

namespace BattlePark.Menu {
	public class GameNetworker : MonoBehaviour {
		public GameGUI GUI;
		
		private Client client;
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerStartGameNetMessage>(OnServerStartGame);
			
			client.SendMessage<ClientGameReadyNetMessage>(new ClientGameReadyNetMessage());
		}
		
		private void OnServerStartGame(ServerStartGameNetMessage message) {
			Grid grid = FindObjectOfType<Grid>();
			grid.GenerateMesh(message.GridSize * 2, message.GridSize);
			grid.Regions.Add(new GridRegion(0, 0, message.GridSize, message.GridSize, message.Ids[0]));
			grid.Regions.Add(new GridRegion(0 + message.GridSize, 0, message.GridSize, message.GridSize, message.Ids[1]));
			
			GridOverlay gridOverlay = FindObjectOfType<GridOverlay>();
			GridRegion ownRegion = grid.Regions.FirstOrDefault(x => x.Owner == client.GetUniqueId());
			gridOverlay.GridSizeX = ownRegion.Width;
			gridOverlay.GridSizeZ = ownRegion.Length;
			gridOverlay.StartX = ownRegion.X;
			gridOverlay.StartZ = ownRegion.Z;
			
			
			StartCoroutine(GUI.FadeGraphic(GUI.Fade, 0, 60f, Color.black, Color.clear));
		}
	}
}