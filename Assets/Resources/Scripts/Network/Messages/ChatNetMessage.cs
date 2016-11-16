using System;
using UnityEngine.Networking;

public class ChatNetMessage : MessageBase
{
	public const short Code = 1003;
	
	public string Message;
}
