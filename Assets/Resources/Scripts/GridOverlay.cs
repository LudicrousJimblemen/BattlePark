using UnityEngine;
using System.Collections.Generic;
using System.Linq;
 
public class GridOverlay : MonoBehaviour {

	public class Overlay {
		public int GridSizeX = 63;
		public int GridSizeZ = 63;

		public float MainStep = 1;

		public float StartX = 0f;
		public float StartZ = 0f;
	}
	public List<Overlay> overlays;
	
	//public GameObject plane;
 	
	public bool ShowGrid = true;
 
	private Material lineMaterial;
 
	public  Color mainColor = new Color(0f, 0f, 0f, 42f / 255f);
 
	void Start () {
		overlays = new List<Overlay>();
		FindObjectOfType<Client>().ThrowConnectionEvent += OnConnect;
	}

	void OnConnect() {
		Grid grid = FindObjectOfType<Grid>();
		foreach(GridRegion region in grid.Regions.Where(x => x.Owner == FindObjectOfType<Client>().PlayerId || x.Owner == 0)) {
			overlays.Add(new Overlay {
				GridSizeX = region.Width,
				GridSizeZ = region.Length,
				MainStep = grid.GridStepXZ,
				StartX = region.X,
				StartZ = region.Z
			});
		}
	}

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
		print("overlay active");
		if (ShowGrid) {
			foreach(Overlay overlay in overlays) {
				CreateLineMaterial();
				// set the current material
				lineMaterial.SetPass(0);

				GL.Begin(GL.LINES);

				GL.Color(mainColor);
				
				float startX = overlay.StartX * overlay.MainStep;
				float startZ = overlay.StartZ * overlay.MainStep;
				float sizeX = overlay.GridSizeX * overlay.MainStep;
				float sizeZ = overlay.GridSizeZ * overlay.MainStep;

				//X axis lines
				for(float i = 0; i <= sizeX; i += overlay.MainStep) {
					GL.Vertex3(startX,0,startZ + i);
					GL.Vertex3(startX + sizeX,0,startZ + i);
				}

				//Z axis lines
				for(float i = 0; i <= sizeX; i += overlay.MainStep) {
					GL.Vertex3(startX + i,0,startZ);
					GL.Vertex3(startX + i,0,startZ + sizeZ);
				}

				GL.End();
			}			
		}
	}
}
