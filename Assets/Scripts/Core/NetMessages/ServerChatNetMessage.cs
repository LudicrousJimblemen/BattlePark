using System;

namespace BattlePark.Core {
	public class ServerChatNetMessage : NetMessage {
		public GameUser Sender { get; set; }
		public string Message { get; set; }
	}
}