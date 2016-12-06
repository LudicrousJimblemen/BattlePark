using System;

namespace BattlePark.Core {
	public class ServerEndGameNetMessage : NetMessage {
		public GameUser Winner { get; set; }
	}
}