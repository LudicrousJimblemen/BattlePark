using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof (MeshFilter), typeof (MeshCollider), typeof (MeshRenderer))]
public class Grid : MonoBehaviour {

	public LayerMask RaycastLayerMask;
	public LayerMask VerticalConstrainRaycastLayerMask;

	public GridObjects Objects = new GridObjects(); //should not yet work
	public List<GridRegion> Regions = new List<GridRegion>();

	public int GridSizeX = 2;
	public int GridSizeZ = 2;

	public float GridStepXZ = 1f;
	public float GridStepY = 0.5f;

	public int PathWidth;

	public Material ParkMat;
	public Material PathMat;

	public void Start() {
		GenerateMesh(GridSizeX,GridSizeZ,2); //temp
	}

	public void GenerateMesh(int xSize, int zSize, int parks, float checkerboardWidth = 4) {
		Vector3[] parkCenters = new Vector3[parks];
		float px, pz;
		int pathVertexCount = 0;
		switch(parks) {
			case 2:
				px = ((float)xSize + PathWidth) / 2f * GridStepXZ;
				pz = 0;
				parkCenters[0] = new Vector3(-px,0,pz);
				parkCenters[1] = new Vector3(px,0,-pz);
				pathVertexCount = 4;
				break;
			case 4:
				px = ((float)xSize + PathWidth) / 2f * GridStepXZ;
				pz = ((float)zSize + PathWidth) / 2f * GridStepXZ;
				parkCenters[0] = new Vector3(-px,0,pz);
				parkCenters[1] = new Vector3(px,0,pz);
				parkCenters[2] = new Vector3(px,0,-pz);
				parkCenters[3] = new Vector3(-px,0,-pz);
				pathVertexCount = 12;
				break;
			default:
				print("invalid park count, defaulting to 2");
				px = ((float)xSize + PathWidth) / 2f * GridStepXZ;
				pz = 0;
				parkCenters[0] = new Vector3(-px,0,pz);
				parkCenters[1] = new Vector3(px,0,-pz);
				pathVertexCount = 4;
				break;
		}
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		meshFilter.mesh = new Mesh();
		meshFilter.mesh.name = "Grid";
	
		Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1) * parks + pathVertexCount + 1];
		Vector2[] uv = new Vector2[vertices.Length];
		meshFilter.mesh.subMeshCount = parks + 1;
		for(int p = 0; p < parks; p++) {
			Vector3 start = (parkCenters[p] - new Vector3(xSize / 2,0,zSize / 2));
			print(start);
			for(int i = 0, z = 0; z <= zSize; z++) {
				for(int x = 0; x <= xSize; x++, i++) {
					vertices[i + (xSize + 1) * (zSize + 1) * p] = new Vector3(x + start.x,0,z + start.z);
					uv[i + (xSize + 1) * (zSize + 1) * p] = new Vector2((float)x * (1f / checkerboardWidth) * 0.5f,(float)z * (1f / checkerboardWidth) * 0.5f);
                }
			}
		}

		int pvc = (xSize + 1) * (zSize + 1) * parks; //park vertex count
		print(pvc);
		float inDist = PathWidth / 2;
		float outDistX = PathWidth / 2 + xSize;
		float outDistZ = PathWidth / 2 + zSize;
		if(pathVertexCount == 4) {
			vertices[pvc] = new Vector3(-inDist,0,-zSize / 2);
			vertices[pvc + 1] = new Vector3(inDist,0,-zSize / 2);
			vertices[pvc + 2] = new Vector3(-inDist,0,zSize / 2);
			vertices[pvc + 3] = new Vector3(inDist,0,zSize / 2);
		} else if (pathVertexCount == 12) {
			vertices[pvc] = new Vector3(-inDist,0,-outDistZ);
			vertices[pvc + 1] = new Vector3(inDist,0,-outDistZ);
			vertices[pvc + 2] = new Vector3(-outDistX,0,-inDist);
			vertices[pvc + 3] = new Vector3(-inDist,0,-inDist);
			vertices[pvc + 4] = new Vector3(inDist,0,-inDist);
			vertices[pvc + 5] = new Vector3(outDistX,0,-inDist);
			vertices[pvc + 6] = new Vector3(-outDistX,0,inDist);
			vertices[pvc + 7] = new Vector3(-inDist,0,inDist);
			vertices[pvc + 8] = new Vector3(inDist,0,inDist);
			vertices[pvc + 9] = new Vector3(outDistX,0,inDist);
			vertices[pvc + 10] = new Vector3(-inDist,0,outDistZ);
			vertices[pvc + 11] = new Vector3(inDist,0,outDistZ);
		}
		for(int i = 0; i < pathVertexCount; i++) {
			uv[pvc + i] = new Vector2((vertices[pvc + i].x + PathWidth / checkerboardWidth * 2) * (1f / checkerboardWidth) * 0.5f,
									  (vertices[pvc + i].z + PathWidth / checkerboardWidth * 2) * (1f / checkerboardWidth) * 0.5f);
		}
		meshFilter.mesh.vertices = vertices;
		meshFilter.mesh.uv = uv;
		for(int sub = 0; sub < parks; sub++) {
			int[] triangles = new int[xSize * zSize * 6];
			for(int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++) {
				for(int x = 0; x < xSize; x++, ti += 6, vi++) {
					triangles[ti] = vi + (xSize + 1) * (zSize + 1) * sub;
					triangles[ti + 3] = triangles[ti + 2] = vi + (xSize + 1) * (zSize + 1) * sub + 1;
					triangles[ti + 4] = triangles[ti + 1] = vi + (xSize + 1) * (zSize + 1) * sub + xSize + 1;
					triangles[ti + 5] = vi + (xSize + 1) * (zSize + 1) * sub + xSize + 2;
				}
			}
			print(sub);
			meshFilter.mesh.SetTriangles(triangles, sub);
		}
		int[] pathTri = new int[(pathVertexCount - 2) * 3];
		for (int pti = 0, pt = 0; pt < pathVertexCount - 2; pt += 2, pti += 6) {
			if (pathVertexCount == 12) {
				if(pt < 2) {
					pathTri[pti] = pathTri[pti + 3] = pt + pvc;
					pathTri[pti + 1] = pathTri[pti + 5] = pt + 4 + pvc;
					pathTri[pti + 2] = pt + 1 + pvc;
					pathTri[pti + 4] = pt + 3 + pvc;
				} else if(pt > 7) {
					pathTri[pti] = pathTri[pti + 3] = pt - 1 + pvc;
					pathTri[pti + 1] = pathTri[pti + 5] = pt + 3 + pvc;
					pathTri[pti + 2] = pt + pvc;
					pathTri[pti + 4] = pt + 2 + pvc;
				} else {
					pathTri[pti] = pathTri[pti + 3] = pt / 2 + 1 + pvc;
					pathTri[pti + 1] = pathTri[pti + 5] = pt / 2 + 6 + pvc;
					pathTri[pti + 2] = pt / 2 + 2 + pvc;
					pathTri[pti + 4] = pt / 2 + 5 + pvc;
				}
			} else {
				pathTri[pti] = pathTri[pti + 3] = pt + pvc;
				pathTri[pti + 1] = pathTri[pti + 5] = pt + 3 + pvc;
				pathTri[pti + 2] = pt + 1 + pvc;
				pathTri[pti + 4] = pt + 2 + pvc;
			}
		}
		meshFilter.mesh.SetTriangles(pathTri,parks);
		meshFilter.mesh.RecalculateNormals();

		Material[] materials = new Material[parks + 1];
		for(int i = 0; i < materials.Length - 1; i++) {
			print("wow");
			materials[i] = ParkMat;
		}
		materials[materials.Length - 1] = PathMat;

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
}
