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
						Log(msg.ReadString(), MessageType.Test);
						break;

					case NetIncomingMessageType.WarningMessage:
						Log(msg.ReadString(), MessageType.Warning);
						break;

					case NetIncomingMessageType.ErrorMessage:
						Log(msg.ReadString(), MessageType.Error);
						break;

					case NetIncomingMessageType.ConnectionApproval:
						//TODO approve

						break;

					case NetIncomingMessageType.Data:
						//double timestamp = msg.ReadTime(false);
						NetMessage netMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

						/*
						if (netMessage is ATypeOfMessage) {
							var castedMsg = (ATypeOfMessage) netMessage;

							SendToAll(castedMsg);
							return;
						}
						*/

						Log(String.Format("Unhandled netMessage data type: {0}", msg.ReadString()), MessageType.Warning);
						break;
						
					default:
						Log(String.Format("Unhandled message type {0}: {1}", msg.MessageType, msg.ReadString()), MessageType.Warning);
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

		public enum MessageType {
			Info,
			Warning,
			Error,
			Test
		}
		private readonly object consoleLock = new object();
		public static void Log(object message, MessageType type = MessageType.Info) {
			lock (consoleLock) {
					DateTime time = DateTime.Now;
				if (type == MessageType.Info) {
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write("[INFO ");
				} else if (type == MessageType.Warning) {
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("[WARN ");
				} else if (type == MessageType.Error) {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("[ERRR ");
				} else if (type == MessageType.Test) {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("[TEST ");
				}
	
				Console.Write(time.ToString("HH:mm:ss dd/MM/yyyy"));
				Console.Write("] ");
				Console.ResetColor();
				try {
					Console.WriteLine(message);
				} catch (NullReferenceException) {
					Console.WriteLine("NULL");
				}
			}
		}
	}
}