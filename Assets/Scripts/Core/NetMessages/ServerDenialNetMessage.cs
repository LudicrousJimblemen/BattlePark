using System;

namespace BattlePark.Core {
	public class ServerDenialNetMessage : NetMessage {
		public string Reason { get; set; }
		
		public string Username { get; set; }
		
		public AppVersion ClientVersion { get; set; }
		public AppVersion ServerVersion { get; set; }
	}
}