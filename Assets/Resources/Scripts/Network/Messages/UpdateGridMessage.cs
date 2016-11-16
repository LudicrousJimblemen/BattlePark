using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class UpdateGridMessage : MessageBase {
	public const short Code = 1022;
	
	//if true, only the stuff that's changed in the grid is sent
	//if false, the entire grid is sent
	public bool OnlyUpdate;
	//TODO everything
	public string Grid1;
	public string Grid2;
}
