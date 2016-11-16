using System;
using UnityEngine.Networking;

public class UpdatePlayerAssignment : MessageBase
{
	public const short Code = 1004;

	public int PlayerID = -1;
}
