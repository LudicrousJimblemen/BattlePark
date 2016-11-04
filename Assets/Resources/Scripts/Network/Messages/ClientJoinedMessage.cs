using System;
using UnityEngine;
using UnityEngine.Networking;

public class ClientJoinedMessage : MessageBase
{
	public const short Code = 1001;
	
	public int ConnectionId;
}
