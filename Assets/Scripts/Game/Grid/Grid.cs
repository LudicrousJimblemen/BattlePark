using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using BattlePark.Core;

namespace BattlePark {
	public class Grid : MonoBehaviour {
		public LayerMask RaycastLayerMask;
		public LayerMask VerticalConstrainRaycastLayerMask;
	
		public GridObjects Objects = new GridObjects();
		public List<Park> Parks = new List<Park>();
	
		public int GridSizeX = 16;
		public int GridSizeZ = 16;
	
		public float GridStepXZ = 1f;
		public float GridStepY = 0.5f;
		
		public GridSummoner GridSummoner;
		
		public GameObject FencePrefab;
		public GameObject GatePrefab;
		
		private Client client;
		
		private void Awake() {
			client = FindObjectOfType<Client>();
			
			client.CreateListener<ServerStartGameNetMessage>(OnServerStartGame);
		}

		public void GenerateMesh(int xSize, int zSize, float checkerboardWidth = 4) {
			MeshFilter meshFilter = GetComponent<MeshFilter>();
		
			meshFilter.mesh = new Mesh();
			meshFilter.mesh.name = "Grid";
		
			Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];
			Vector2[] uv = new Vector2[vertices.Length];
			for (int i = 0, z = 0; z <= zSize; z++) {
				for (int x = 0; x <= xSize; x++, i++) {
					vertices[i] = new Vector3(x, 0, z);
					uv[i] = new Vector2((float)x * (1f / checkerboardWidth) * 0.5f, (float)z * (1f / checkerboardWidth) * 0.5f);
				}
			}
		
			int[] triangles = new int[xSize * zSize * 6];
			for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++) {
				for (int x = 0; x < xSize; x++, ti += 6, vi++) {
					triangles[ti] = vi;
					triangles[ti + 3] = triangles[ti + 2] = vi + 1;
					triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
					triangles[ti + 5] = vi + xSize + 2;
				}
			}
		
			meshFilter.mesh.vertices = vertices;
			meshFilter.mesh.triangles = triangles;
			meshFilter.mesh.uv = uv;
			meshFilter.mesh.RecalculateNormals();
		
			GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
		}
	
		public Vector3 ToGridSpace(Vector3 position) {
			return new Vector3 {
				x = Mathf.RoundToInt((position.x - 0.5f) / GridStepXZ),
				y = Mathf.RoundToInt(position.y / GridStepY),
				z = Mathf.RoundToInt((position.z - 0.5f) / GridStepXZ)
			};
		}
	
		public bool ValidRegion(Vector3 position, long id) {
			return Parks.First(park => park.Owner == id).Regions.Any(region => region.Inside(ToGridSpace(position)));
		}
		
		private void OnServerStartGame(ServerStartGameNetMessage message) {
			switch (message.Ids.Count) {
				case 1:
					GenerateMesh(message.GridSize, message.GridSize);
					Parks.Add(new Park(1, 1, message.GridSize - 2, message.GridSize - 2, message.Ids[0]));
					break;
				case 2:
					GenerateMesh(message.GridSize * 2, message.GridSize);
					Parks.Add(new Park(1, 1, message.GridSize - 2, message.GridSize - 2, message.Ids[0]));
					Parks.Add(new Park(1 + message.GridSize, 1, message.GridSize - 2, message.GridSize - 2, message.Ids[1]));
					break;
				case 4:
					GenerateMesh(message.GridSize * 2, message.GridSize * 2);
					Parks.Add(new Park(
						1,
						1,
						message.GridSize - 2,
						message.GridSize - 2,
					message.Ids[0]));
					Parks.Add(new Park(
						1 + message.GridSize,
						1,
						message.GridSize - 2,
						message.GridSize - 2,
					message.Ids[1]));
					Parks.Add(new Park(
						1,
						1 + message.GridSize,
						message.GridSize - 2,
						message.GridSize - 2,
					message.Ids[2]));
					Parks.Add(new Park(
						1 + message.GridSize,
						1 + message.GridSize,
						message.GridSize - 2,
						message.GridSize - 2,
					message.Ids[3]));
					break;
			}
			
			foreach (var park in Parks) {
				GridRegion region = park.Regions.First();
				
				for (int i = 0; i < region.Width; i++) {
					if (i < Mathf.RoundToInt(region.Width / 2f) - 2 || i >= Mathf.RoundToInt(region.Width / 2f) + 2) {
						Fence southFence = GridSummoner.SummonGridObject(FencePrefab, park.Owner).GetComponent<Fence>();
						southFence.X = region.X + i;
						southFence.Y = 0;
						southFence.Z = region.Z;
						southFence.Direction = Direction.South;
						southFence.UpdatePosition();
					}
					
					Fence northFence = GridSummoner.SummonGridObject(FencePrefab, park.Owner).GetComponent<Fence>();
					northFence.X = region.X + i;
					northFence.Y = 0;
					northFence.Z = region.Z + region.Length - 1;
					northFence.Direction = Direction.North;
					northFence.UpdatePosition();
				}
				for (int i = 0; i < region.Length; i++) {
					Fence westFence = GridSummoner.SummonGridObject(FencePrefab, park.Owner).GetComponent<Fence>();
					westFence.X = region.X;
					westFence.Y = 0;
					westFence.Z = region.Z + i;
					westFence.Direction = Direction.West;
					westFence.UpdatePosition();
					
					Fence eastFence = GridSummoner.SummonGridObject(FencePrefab, park.Owner).GetComponent<Fence>();
					eastFence.X = region.X + region.Length - 1;
					eastFence.Y = 0;
					eastFence.Z = region.Z + i;
					eastFence.Direction = Direction.East;
					eastFence.UpdatePosition();
				}
				
				Gate gate = GridSummoner.SummonGridObject(GatePrefab, park.Owner).GetComponent<Gate>();
				gate.X = region.X + Mathf.RoundToInt(region.Width / 2f) - 2;
				gate.Y = 0;
				gate.Z = region.Z - 1;
				gate.UpdatePosition();
			}
			
			GridOverlay gridOverlay = FindObjectOfType<GridOverlay>();
			GridRegion ownRegion = Parks.First(park => park.Owner == client.GetUniqueId()).Regions.First();
			gridOverlay.GridSizeX = ownRegion.Width;
			gridOverlay.GridSizeZ = ownRegion.Length;
			gridOverlay.StartX = ownRegion.X;
			gridOverlay.StartZ = ownRegion.Z;
		}

		private void OnDrawGizmos() {
			foreach (var park in Parks) {
				foreach (var region in park.Regions) {
					UnityEngine.Random.InitState(park.Owner.GetHashCode());
					Gizmos.color = UnityEngine.Random.ColorHSV(0, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
					Gizmos.DrawCube(region.GetCenter(this), new Vector3(region.Width, 0.1f, region.Length));
				}
			}
		}
	}
}