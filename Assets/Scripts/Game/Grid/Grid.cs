// TODO 1 wide border around all parkCount for gate and fences

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
	public static Grid Instance;

	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstraintRaycastLayerMask;

	public GridObjects Objects = new GridObjects();
	// should not yet work
	public List<GridRegion> Regions = new List<GridRegion>();

	public int GridSizeX = 81;
	public int GridSizeZ = 81;

	public float GridStepXZ = 1f;
	public float GridStepY = 0.5f;

	public int PathWidth = 3;

	public Material ParkMaterial;
	public Material PathMaterial;
	public Material BordMaterial;

	private Vector3[] parkCenters;

	public void Awake() {
		if (FindObjectsOfType<Grid>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
		GenerateMesh(GridSizeX, GridSizeZ, 3); // tempborary
	}
	public void Start() {
		GameManager.Instance.ParkCenters = parkCenters;
	}

	public void GenerateMesh(int xSize, int zSize, float checkerboardWidth = 3) {
		if (xSize <= 0) {
			throw new System.ArgumentOutOfRangeException("xSize");
		} else if (zSize <= 0) {
			throw new System.ArgumentOutOfRangeException("zSize");
		} else if (checkerboardWidth <= 0) {
			throw new System.ArgumentOutOfRangeException("checkerboardWidth");
		}
		
		parkCenters = new Vector3[2];
		
		float px, pz;
		int pathVertexCount = 0;
		
		px = (((float)xSize + PathWidth) / 2f + 1f) * GridStepXZ;
		pz = 0;
		parkCenters[0] = new Vector3(-px, 0, pz);
		parkCenters[1] = new Vector3(px, 0, -pz);
		pathVertexCount = 4;
		
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		meshFilter.mesh = new Mesh();
		meshFilter.mesh.name = "Grid";

		Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1) * 2 + pathVertexCount + 12 * 2 + 1];
		Vector2[] uv = new Vector2[vertices.Length];
		meshFilter.mesh.subMeshCount = 2 + 2;

		for (int p = 0; p < 2; p++) {
			Vector3 start = (parkCenters[p] - new Vector3(xSize / 2f, 0, zSize / 2f));
			for (int i = 0, z = 0; z <= zSize; z++) {
				for (int x = 0; x <= xSize; x++, i++) {
					vertices[i + (xSize + 1) * (zSize + 1) * p] = new Vector3(x + start.x, 0, z + start.z);
					uv[i + (xSize + 1) * (zSize + 1) * p] = new Vector2((float)x * (1f / checkerboardWidth) * 0.5f, (float)z * (1f / checkerboardWidth) * 0.5f);
				}
			}
		}

		#region Paths
		int parkVertexCount = (xSize + 1) * (zSize + 1) * 2;
		float inDist = (PathWidth / 2f) * GridStepXZ;
		float outDistX = (PathWidth / 2f + xSize + 2) * GridStepXZ;
		float outDistZ = (PathWidth / 2f + zSize + 2) * GridStepXZ;
		
		vertices[parkVertexCount] = new Vector3(-inDist, 0, -zSize / 2f - 1f);
		vertices[parkVertexCount + 1] = new Vector3(inDist, 0, -zSize / 2f - 1f);
		vertices[parkVertexCount + 2] = new Vector3(-inDist, 0, zSize / 2f + 1f);
		vertices[parkVertexCount + 3] = new Vector3(inDist, 0, zSize / 2f + 1f);
		
		for (int i = 0; i < pathVertexCount; i++) {
			uv[parkVertexCount + i] = new Vector2((vertices[parkVertexCount + i].x) * (1f / checkerboardWidth) * 0.5f,
				(vertices[parkVertexCount + i].z) * (1f / checkerboardWidth) * 0.5f);
		}
		#endregion
		
		#region Border
		int pvcb = parkVertexCount + pathVertexCount;
		float inDistX = xSize / 2f;
		float inDistZ = zSize / 2f;
		outDistX = xSize / 2f + 1f;
		outDistZ = zSize / 2f + 1f;
		// print(pvcb);
		for (int b = 0; b < 2; b++) {
			vertices[pvcb + 12 * b] = new Vector3(-outDistX, 0, -outDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 1] = new Vector3(outDistX, 0, -outDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 2] = new Vector3(outDistX, 0, outDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 3] = new Vector3(-outDistX, 0, outDistZ) + parkCenters[b];

			vertices[pvcb + 12 * b + 4] = new Vector3(-inDistX, 0, -inDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 5] = new Vector3(inDistX, 0, -inDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 6] = new Vector3(inDistX, 0, inDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 7] = new Vector3(-inDistX, 0, inDistZ) + parkCenters[b];

			vertices[pvcb + 12 * b + 8] = new Vector3(0, 0, -outDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 9] = new Vector3(outDistX, 0, 0) + parkCenters[b];
			vertices[pvcb + 12 * b + 10] = new Vector3(0, 0, outDistZ) + parkCenters[b];
			vertices[pvcb + 12 * b + 11] = new Vector3(-outDistX, 0, 0) + parkCenters[b];
		}
		float totalXSize = xSize * 2f + PathWidth + 4f;
		float totalZSize = zSize * 2f + PathWidth + 4f;
		
		for (int i = 0; i < 2 * 12; i++) {
			uv[pvcb + i] = new Vector2((vertices[pvcb + i].x + totalXSize / 2f) / totalXSize,
				(vertices[pvcb + i].z + totalZSize / 2f) / totalXSize);
		}
		// print(vertices.Length);
		meshFilter.mesh.vertices = vertices;
		meshFilter.mesh.uv = uv;
		for (int sub = 0; sub < 2; sub++) {
			int[] triangles = new int[xSize * zSize * 6];
			for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++) {
				for (int x = 0; x < xSize; x++, ti += 6, vi++) {
					triangles[ti] = vi + (xSize + 1) * (zSize + 1) * sub;
					triangles[ti + 3] = triangles[ti + 2] = vi + (xSize + 1) * (zSize + 1) * sub + 1;
					triangles[ti + 4] = triangles[ti + 1] = vi + (xSize + 1) * (zSize + 1) * sub + xSize + 1;
					triangles[ti + 5] = vi + (xSize + 1) * (zSize + 1) * sub + xSize + 2;
				}
			}
			// print(sub);
			meshFilter.mesh.SetTriangles(triangles, sub);
		}
		int[] pathTri = new int[(pathVertexCount - 2) * 3];
		for (int pti = 0, pt = 0; pt < pathVertexCount - 2; pt += 2, pti += 6) {
			if (pathVertexCount == 12) {
				if (pt < 2) {
					pathTri[pti] = pathTri[pti + 3] = pt + parkVertexCount;
					pathTri[pti + 1] = pathTri[pti + 5] = pt + 4 + parkVertexCount;
					pathTri[pti + 2] = pt + 1 + parkVertexCount;
					pathTri[pti + 4] = pt + 3 + parkVertexCount;
				} else if (pt > 7) {
					pathTri[pti] = pathTri[pti + 3] = pt - 1 + parkVertexCount;
					pathTri[pti + 1] = pathTri[pti + 5] = pt + 3 + parkVertexCount;
					pathTri[pti + 2] = pt + parkVertexCount;
					pathTri[pti + 4] = pt + 2 + parkVertexCount;
				} else {
					pathTri[pti] = pathTri[pti + 3] = pt / 2 + 1 + parkVertexCount;
					pathTri[pti + 1] = pathTri[pti + 5] = pt / 2 + 6 + parkVertexCount;
					pathTri[pti + 2] = pt / 2 + 2 + parkVertexCount;
					pathTri[pti + 4] = pt / 2 + 5 + parkVertexCount;
				}
			} else {
				pathTri[pti] = pathTri[pti + 3] = pt + parkVertexCount;
				pathTri[pti + 1] = pathTri[pti + 5] = pt + 3 + parkVertexCount;
				pathTri[pti + 2] = pt + 1 + parkVertexCount;
				pathTri[pti + 4] = pt + 2 + parkVertexCount;
			}
		}
		meshFilter.mesh.SetTriangles(pathTri, 2);
		int[] borderTri = new int[36 * 2];
		for (int i = 0; i < 2; i++) {
			int ind = i * 36;
			for (int s = 0; s < 3; s++) { // first three sides of the border
				borderTri[ind + 9 * s] = s + pvcb + i * 12;
				borderTri[ind + 9 * s + 1] = borderTri[ind + 9 * s + 3] = s + pvcb + 4 + i * 12;
				borderTri[ind + 9 * s + 2] = borderTri[ind + 9 * s + 5] = borderTri[ind + 9 * s + 8] = s + pvcb + 8 + i * 12;
				borderTri[ind + 9 * s + 4] = borderTri[ind + 9 * s + 6] = s + pvcb + 5 + i * 12;
				borderTri[ind + 9 * s + 7] = s + pvcb + 1 + i * 12;
			}
			// last side
			borderTri[ind + 27] = 3 + pvcb + i * 12;
			borderTri[ind + 27 + 1] = borderTri[ind + 27 + 3] = 3 + pvcb + 4 + i * 12;
			borderTri[ind + 27 + 2] = borderTri[ind + 27 + 5] = borderTri[ind + 27 + 8] = 3 + pvcb + 8 + i * 12;
			borderTri[ind + 27 + 4] = borderTri[ind + 27 + 6] = 3 + pvcb + 1 + i * 12;
			borderTri[ind + 27 + 7] = pvcb + i * 12;
		}
		meshFilter.mesh.SetTriangles(borderTri, 2 + 1);
		meshFilter.mesh.RecalculateNormals();
		#endregion
		
		Material[] materials = new Material[2 + 2];
		for (int i = 0; i < materials.Length - 2; i++) {
			materials[i] = ParkMaterial;
		}
		materials[materials.Length - 2] = PathMaterial;
		materials[materials.Length - 1] = BordMaterial;

		meshRenderer.materials = materials;

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

	/// <summary>
	/// Snaps a position to the grid depending on which player is acting.
	/// </summary>
	/// <param name="position">Input position.</param>
	/// <param name="playerNum">Player number of the targeted park, either 1 or 2.</param>
	/// <returns>The snapped position.</returns>
	public Vector3 SnapToGrid(Vector3 position, int playerNum = 1) {
		if (playerNum < 1 || playerNum > 2) {
			throw new System.ArgumentOutOfRangeException("playerNum");
		}
		
		Vector3 center = GameManager.Instance.ParkCenters[playerNum - 1];
		Vector3 snapped = position - center;
		snapped.x = Mathf.Floor((snapped.x + 0.5f) / GridStepXZ) * GridStepXZ + center.x;
		snapped.y = Mathf.Clamp(Mathf.Floor(snapped.y / GridStepY) * GridStepY, 0, Mathf.Infinity);
		snapped.z = Mathf.Floor((snapped.z + 0.5f) / GridStepXZ) * GridStepXZ + center.z;
		return snapped;
	}
}
