using System;
using UnityEngine.Networking;

public class ChatNetMessage : MessageBase {
	public const short Code = 1000;
	
	public int ConnectionId;
	
	public string Message;
}
