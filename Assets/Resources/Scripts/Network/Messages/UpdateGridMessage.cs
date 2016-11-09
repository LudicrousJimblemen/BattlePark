using System;
using UnityEngine.Networking;

public class UpdateGridMessage : MessageBase {
	public const short Code = 1022;
	
	//if true, only the stuff that's changed in the grid is sent
	//if false, the entire grid is sent
	public bool OnlyUpdate;
	//TODO everything
}
