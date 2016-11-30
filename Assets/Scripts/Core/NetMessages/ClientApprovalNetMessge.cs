using System;

namespace BattlePark.Core {
	public class ClientApprovalNetMessage : NetMessage {
		public string Username { get; set; }
		public GameVersion Version { get; set; }
	}
}