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
		public List<GridRegion> Regions = new List<GridRegion>();
	
		public int GridSizeX = 16;
		public int GridSizeZ = 16;
	
		public float GridStepXZ = 1f;
		public float GridStepY = 0.5f;
		
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
			return Regions.Any(x => x.Inside(ToGridSpace(position)) && x.Valid(id));
		}
		
		private void OnServerStartGame(ServerStartGameNetMessage message) {
			switch (message.Ids.Count) {
				case 1:
					GenerateMesh(message.GridSize, message.GridSize);
					Regions.Add(new GridRegion(1, 1, message.GridSize - 1, message.GridSize - 1, message.Ids[0]));
					break;
				case 2:
					GenerateMesh(message.GridSize * 2, message.GridSize);
					Regions.Add(new GridRegion(1, 1, message.GridSize - 1, message.GridSize - 1, message.Ids[0]));
					Regions.Add(new GridRegion(1 + message.GridSize, 1, message.GridSize - 1, message.GridSize - 1, message.Ids[1]));
					break;
				case 4:
					GenerateMesh(message.GridSize * 2, message.GridSize * 2);
					Regions.Add(new GridRegion(1, 1, message.GridSize - 1, message.GridSize - 1, message.Ids[0]));
					Regions.Add(new GridRegion(1 + message.GridSize, 1, message.GridSize - 1, message.GridSize - 1, message.Ids[1]));
					Regions.Add(new GridRegion(1, 1 + message.GridSize, message.GridSize - 1, message.GridSize - 1, message.Ids[2]));
					Regions.Add(new GridRegion(1 + message.GridSize, 1 + message.GridSize - 1, message.GridSize - 1, message.GridSize, message.Ids[3]));
					break;
			}
			
			GridOverlay gridOverlay = FindObjectOfType<GridOverlay>();
			GridRegion ownRegion = Regions.FirstOrDefault(x => x.Owner == client.GetUniqueId());
			gridOverlay.GridSizeX = ownRegion.Width;
			gridOverlay.GridSizeZ = ownRegion.Length;
			gridOverlay.StartX = ownRegion.X;
			gridOverlay.StartZ = ownRegion.Z;
		}

		private void OnDrawGizmos() {
			foreach (var region in Regions) {
				if (region.Owner == -1) {
					Gizmos.color = Color.grey;
				} else if (region.Owner == 0) {
					Gizmos.color = Color.white;
				} else {
					UnityEngine.Random.InitState(region.Owner.GetHashCode());
					Gizmos.color = UnityEngine.Random.ColorHSV(0, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
				}
				Gizmos.DrawCube(region.GetCenter(this), new Vector3(region.Width, 0.1f, region.Length));
			}
		}
	}
}