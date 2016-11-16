using UnityEngine;
using System.Collections;
 
public class GridOverlay : MonoBehaviour {
 
	//public GameObject plane;
 	
	public bool ShowGrid = true;
	
	public int GridSizeX = 100;
	public int GridSizeZ = 100;
 
	public float MainStep = 1;
 
	public float StartX = -104f;
	public float StartZ = -50f;
 
	private Material lineMaterial;
 
	public  Color mainColor = new Color(0f, 0f, 0f, 42f / 255f);
 
	void CreateLineMaterial() {
		if (!lineMaterial) {
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			var shader = Shader.Find("Hidden/Internal-Colored");
			lineMaterial = new Material(shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt("_ZWrite", 0);
		}
	}
 
	void OnPostRender() {
		if (ShowGrid) {
			CreateLineMaterial();
			// set the current material
			lineMaterial.SetPass(0);
	 
			GL.Begin(GL.LINES);
	 
			GL.Color(mainColor);
	
			//X axis lines
			for (float i = 0; i <= GridSizeZ; i += MainStep) {
				GL.Vertex3(StartX, 0, StartZ + i);
				GL.Vertex3(StartX + GridSizeX, 0, StartZ + i);
			}
	
			//Z axis lines
			for (float i = 0; i <= GridSizeX; i += MainStep) {
				GL.Vertex3(StartX + i, 0, StartZ);
				GL.Vertex3(StartX + i, 0, StartZ + GridSizeZ);
			}
	 
			GL.End();
		}
	}
}
