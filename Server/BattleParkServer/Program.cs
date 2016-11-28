using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Newtonsoft.Json;
using BattlePark.Core;

namespace BattlePark.Server {
	class Program {
		private static NetServer server;
		private static JsonSerializerSettings serializerSettings;

		private static List<GameUser> users = new List<GameUser>();

		private static void Main(string[] args) {
			Log("Starting server...");

			NetPeerConfiguration config = new NetPeerConfiguration("Battle Park");
			config.Port = 6666; //TODO make dynamic
			config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

			server = new NetServer(config);

			serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

			server.Start();

			Log("Server started.");

			NetIncomingMessage msg;
			while ((msg = server.ReadMessage()) != null) {
				switch (msg.MessageType) {
					case NetIncomingMessageType.DebugMessage:
					case NetIncomingMessageType.VerboseDebugMessage:
						Log(msg.ReadString());
						break;

					case NetIncomingMessageType.WarningMessage:
						Log(msg.ReadString());
						break;

					case NetIncomingMessageType.ErrorMessage:
						Log(msg.ReadString());
						break;

					case NetIncomingMessageType.ConnectionApproval:
						//TODO approve

						break;

					case NetIncomingMessageType.Data:
						double timestamp = msg.ReadTime(false);
						NetMessage netMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

						/*
						if (netMessage is ATypeOfMessage) {
							var castedMsg = (ATypeOfMessage) netMessage;

							SendToAll(castedMsg);
							return;
						}
						*/

						Log("ah! Ah!!! AH!!!!! UNHANLED MESSAGE TYPE IN SERVER!!!!!!! " + msg.ReadString());

						break;
					default:
						Log(String.Format("UNHANDLED!!! AAA!!! {0} MESSAGE: {1}", msg.MessageType, msg.ReadString()));
						break;
				}
			}
		}

		private static void SendToAll(NetMessage matchMsg) {
			string matchMsgSerialized = JsonConvert.SerializeObject(matchMsg, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

			NetOutgoingMessage netMsg = server.CreateMessage();
			netMsg.WriteTime(false);
			netMsg.Write(matchMsgSerialized);
			server.SendToAll(netMsg, NetDeliveryMethod.ReliableOrdered);
		}

		public static void Log(object message) {
			//make this pretty sometime
			Console.WriteLine(message);
		}
	}
}
