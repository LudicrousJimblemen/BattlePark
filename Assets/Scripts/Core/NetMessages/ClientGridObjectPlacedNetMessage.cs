using System;
using BattlePark;

namespace BattlePark.Core {
	public class ClientGridObjectPlacedNetMessage : NetMessage {
		public GridObjectData GridObject { get; set; }
		public string Type { get; set; }
	}
}