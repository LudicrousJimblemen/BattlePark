using System;
using UnityEngine;

public class GridOverlay : MonoBehaviour {
	public static GridOverlay Instance;

	public bool ShowGrid = true;
	
	/*
	[Range(1, 2048)]
	public int GridSizeX = 2;
	[Range(1, 2048)]
	public int GridSizeZ = 2;

	[Range(1, 2048)]
	public float MainStep = 1;

	public float StartX;
	public float StartZ;
	*/
	public Color mainColor = new Color(0f, 0f, 0f, 42f / 255f);

	private Material lineMaterial;

	private void Awake() {
		if(FindObjectsOfType<GridOverlay>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;
	}
	
	private void CreateLineMaterial() {
		if (!lineMaterial) {
			var shader = Shader.Find("Hidden/Internal-Colored");
			lineMaterial = new Material(shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			lineMaterial.SetInt("_ZWrite", 0);
		}
	}

	private void OnPostRender() {
		if (ShowGrid) {
			Grid grid = Grid.Instance;
			CreateLineMaterial();
			lineMaterial.SetPass(0);
 
			GL.Begin(GL.LINES);
 
			GL.Color(mainColor);
			for (int p = 0; p < 2; p ++) { // for each park
				Vector3 start = grid.parkCenters[p] - new Vector3 (grid.GridSizeX, 0, grid.GridSizeZ) * grid.GridStepXZ / 2f;
				for(float z = 0; z <= grid.GridSizeZ * grid.GridStepXZ; z += grid.GridStepXZ) {
					GL.Vertex3(start.x,0,start.z + z);
					GL.Vertex3(start.x + grid.GridSizeX * grid.GridStepXZ,0,start.z + z);
				}
				for(float x = 0; x <= grid.GridSizeX * grid.GridStepXZ; x += grid.GridStepXZ) {
					GL.Vertex3(start.x + x,0,start.z);
					GL.Vertex3(start.x + x,0,start.z + grid.GridSizeZ * grid.GridStepXZ);
				}
			}
 
			GL.End();
		}
	}
}