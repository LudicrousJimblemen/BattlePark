using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UpdatePlayerListMessage : MessageBase {
	public const short Code = 1004;
	
	public int[] PlayerList;
	//public List<int> PlayerList;
}
