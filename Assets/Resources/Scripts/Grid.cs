using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Grid : MonoBehaviour {
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;
	
	public int PlayerId;
	
	public int GridSize = 30;
	
	public float GridXZ = 1f;
	public float GridY = 0.5f;
	
	public GridObjects Objects;
	
	void Awake() {
		GenerateMesh(GridSize, GridSize, 4);
	}
	
	void Start() {
		PlayerId = int.Parse(name.Substring(4, name.Length - 4));
		Objects = new GridObjects();
	}
	
	public string Serialize() {
		return JsonConvert.SerializeObject(Objects);
	}
	public void Deserialize(string message) {
		Objects = JsonConvert.DeserializeObject<GridObjects>(message);
	}
	
	public void GenerateMesh(int xSize, int zSize, float checkerboardWidth) {
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
		
		transform.Translate(0, 0, -zSize / 2f, Space.Self);
	}
}