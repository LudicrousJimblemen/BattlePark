using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Newtonsoft.Json;
using BattlePark.Core;

namespace BattlePark.Server {
	struct ServerConfig {
		public GameVersion Version;
		
		public int Port;
		
		public int MaxUsers;
	}
	
	class Program {
		private static ServerConfig serverConfig = new ServerConfig {
			Version = new GameVersion(0, 2, 0),
			Port = 6666,
			MaxUsers = 2
		};
		
		private static NetServer server;
		private static JsonSerializerSettings serializerSettings;

		private static List<GameUser> users = new List<GameUser>();

		private static void Main(string[] args) {
			UpdateTitle();
			
			NetPeerConfiguration config = new NetPeerConfiguration(GameConfig.Name);
			config.Port = serverConfig.Port;
			config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

			serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			
			server = new NetServer(new NetPeerConfiguration(GameConfig.Name));
			server.Start();

			Log(String.Format("Server started on port {0}.", serverConfig.Port));

			bool shouldExit = false;
			
			while (!shouldExit) {
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
							NetMessage approvalMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);
							ClientApprovalNetMessage castedMsg = (ClientApprovalNetMessage)approvalMessage;
						
							string ip = msg.SenderConnection.RemoteEndPoint.Address.ToString();
						
							string denialReason = String.Empty;
							
							if (castedMsg.Version != serverConfig.Version) {
								denialReason = "error.wrongVersion";
								Log(String.Format("User '{0}' ({1}) denied because of wrong version.", castedMsg.Username, ip));
							} else if (String.IsNullOrEmpty(castedMsg.Username)) {
								denialReason = "error.emptyUsername";
								Log(String.Format("User '{0}' ({1}) denied because of empty username.", castedMsg.Username, ip));
							} else if (users.Select(x => x.Username).Contains(castedMsg.Username)) {
								denialReason = "error.duplicateUsername";
								Log(String.Format("User '{0}' ({1}) denied because of duplicate username.", castedMsg.Username, ip));
							} else if (users.Count >= serverConfig.MaxUsers) {
								denialReason = "error.roomFull";
								Log(String.Format("User '{0}' ({1}) denied because of a full room.", castedMsg.Username, ip));
							}
							
							if (denialReason != String.Empty) {
								msg.SenderConnection.Deny(JsonConvert.SerializeObject(new ServerDenialNetMessage {
									Reason = denialReason,
									Username = castedMsg.Username,
									ClientVersion = castedMsg.Version,
									ServerVersion = serverConfig.Version,
								}, serializerSettings));
							} else {
								msg.SenderConnection.Approve();
							
								GameUser newUser = new GameUser(
									msg.SenderConnection.RemoteUniqueIdentifier,
									users.Count,
									castedMsg.Username,
									msg.SenderConnection
								);
							
								users.Add(newUser);
							
								Log(String.Format("User '{0}' ({1}) approved.", castedMsg.Username, ip));								
							}

							break;

						case NetIncomingMessageType.Data:
							//double timestamp = msg.ReadTime(false);
							NetMessage netMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);
	
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
		}

		private static void SendToAll(NetMessage matchMsg) {
			string matchMsgSerialized = JsonConvert.SerializeObject(matchMsg, serializerSettings);

			NetOutgoingMessage netMsg = server.CreateMessage();
			netMsg.WriteTime(false);
			netMsg.Write(matchMsgSerialized);
			server.SendToAll(netMsg, NetDeliveryMethod.ReliableOrdered);
		}
		
		private static void UpdateTitle() {
			Console.Title = String.Format(
				"Battle Park {0}, {1}/{2} users",
				serverConfig.Version,
				users.Count,
				serverConfig.MaxUsers
			);
		}

		public enum MessageType {
			Info,
			Warning,
			Error,
			Test
		}
		private static object consoleLock = new object();
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