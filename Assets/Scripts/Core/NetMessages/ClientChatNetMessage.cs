using System;

namespace BattlePark.Core {
	public class ClientChatNetMessage : NetMessage {
		public string Message { get; set; }
	}
}