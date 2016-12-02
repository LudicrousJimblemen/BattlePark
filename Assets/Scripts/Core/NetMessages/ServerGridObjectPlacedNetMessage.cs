using System;
using BattlePark;

namespace BattlePark.Core {
	public class ServerGridObjectPlacedNetMessage : NetMessage {
		public GameUser Sender { get; set; }
		
		public GridObjectData GridObject { get; set; }
		public string Type { get; set; }
	}
}