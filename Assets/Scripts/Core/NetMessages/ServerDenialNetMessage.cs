using System;

namespace BattlePark.Core {
	public class ServerDenialNetMessage : NetMessage {
		public string Reason { get; set; }
		
		public string Username { get; set; }
		
		public GameVersion ClientVersion { get; set; }
		public GameVersion ServerVersion { get; set; }
	}
}