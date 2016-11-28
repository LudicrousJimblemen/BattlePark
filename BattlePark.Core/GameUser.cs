using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Newtonsoft.Json;

namespace BattlePark.Core {
	public class GameUser {
		public long Id { get; private set; }
		public int Number { get; private set; }
		public string Username { get; private set; }

		[JsonIgnore]
		public NetConnection Connection { get; private set; }

		public bool ReadyToStart { get; set; }

		public GameUser(long id, int number, string username, NetConnection connection) {
			Id = id;
			Number = number;
			Username = username;
			Connection = connection;
		}
	}
}
