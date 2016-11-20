using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class Grid : MonoBehaviour {
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	
	public GridObjects Objects = new GridObjects();
	public List<GridRegion> Regions = new List<GridRegion>();
	
	public int GridSizeX = 129;
	public int GridSizeZ = 63;
	
	public float GridStepXZ = 1f;
	public float GridStepY = 0.5f;
	
	
	private void Awake() {
		GenerateMesh(GridSizeX, GridSizeZ, 3);
		Regions.Add (new GridRegion (0,0,GridSizeZ,GridSizeZ,1));
		Regions.Add (new GridRegion (GridSizeX - GridSizeZ,0,GridSizeZ,GridSizeZ,2));
		Regions.Add (new GridRegion (GridSizeZ,0,GridSizeX - 2*GridSizeZ,GridSizeZ,-1));
	}

	private void OnDrawGizmos () {
		/*
		foreach (var region in Regions) {
			if (region.Owner == -1) {
				Gizmos.color = Color.grey;
			} else if (region.Owner == 0) {
				Gizmos.color = Color.white;
			} else if (region.Owner == 1) {
				Gizmos.color = Color.blue;
			} else if (region.Owner == 2) {
				Gizmos.color = Color.red;
			}
			Gizmos.DrawCube (region.GetCenter (this),new Vector3 (region.Width,0.1f,region.Length));
		}
		*/
	}

	private void GenerateMesh(int xSize, int zSize, float checkerboardWidth) {
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
	
	public string Serialize() {
		return JsonConvert.SerializeObject(Objects);
	}
	public void Deserialize(string message) {
		Objects = JsonConvert.DeserializeObject<GridObjects>(message);
	}
	
	public Vector3 ToGridSpace(Vector3 position) {
		return new Vector3 {
			x = Mathf.RoundToInt((position.x - 0.5f) / GridStepXZ),
			y = Mathf.RoundToInt(position.y / GridStepY),
			z = Mathf.RoundToInt((position.z - 0.5f) / GridStepXZ)
		};
	}
	
	public bool ValidRegion(Vector3 position, int id) {
		return Regions.Any(x => x.Inside(ToGridSpace(position)) && x.Valid(id));
	}
}