using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (MeshFilter)]
public class Grid : MonoBehaviour {
	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;

	public GridObjects Objects = new GridObjects(); //should not yet work
	public List<GridRegion> Regions = new List<GridRegion>();

	public int GridSizeX = 4;
	public int GridSizeZ = 2;

	public float GridStepXZ = 1f;
	public float GridStepY = 0.5f;
	
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
}
