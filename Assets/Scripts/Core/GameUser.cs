using System;
using System.Linq;
using Lidgren.Network;
using Newtonsoft.Json;

namespace BattlePark.Core {
	public class GameUser {
		public long Id { get; private set; }
		public string Username { get; set; }

		[JsonIgnore]
		public NetConnection Connection { get; private set; }

		public bool LobbyReady { get; set; }
		public bool GameReady { get; set; }

		public GameUser(long id, string username, NetConnection connection) {
			Id = id;
			Username = username;
			Connection = connection;
		}
	}
}
