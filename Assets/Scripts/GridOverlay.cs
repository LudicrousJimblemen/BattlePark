using System;
using UnityEngine;

namespace BattlePark {
	public class GridOverlay : MonoBehaviour {
		public bool ShowGrid = true;
		
		[Range(1, Single.PositiveInfinity)]
		public int GridSizeX = 2;
		[Range(1, Single.PositiveInfinity)]
		public int GridSizeZ = 2;
 
		[Range(1, Single.PositiveInfinity)]
		public float MainStep = 1;
 
		public float StartX;
		public float StartZ;
 
		public Color mainColor = new Color(0f, 0f, 0f, 42f / 255f);
 
		private Material lineMaterial;
 
		void CreateLineMaterial() {
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
 
		void OnPostRender() {
			if (ShowGrid) {
				CreateLineMaterial();
				lineMaterial.SetPass(0);
	 
				GL.Begin(GL.LINES);
	 
				GL.Color(mainColor);
	
				for (float i = 0; i <= GridSizeZ; i += MainStep) {
					GL.Vertex3(StartX, 0, StartZ + i);
					GL.Vertex3(StartX + GridSizeX, 0, StartZ + i);
				}
	
				for (float i = 0; i <= GridSizeX; i += MainStep) {
					GL.Vertex3(StartX + i, 0, StartZ);
					GL.Vertex3(StartX + i, 0, StartZ + GridSizeZ);
				}
	 
				GL.End();
			}
		}
	}
}