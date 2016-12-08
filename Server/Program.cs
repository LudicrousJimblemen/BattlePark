using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Newtonsoft.Json;
using BattlePark.Core;

namespace BattlePark.Server {
	class ServerConfig {
		public AppVersion Version;
		
		public int Port;
		
		public int UserCount;
	}
	
	class Program {
		#region Server
		
		private static ServerConfig serverConfig = new ServerConfig {
			Version = new AppVersion(0, 2, 1),
			Port = 6666,
			UserCount = 2
		};
		
		private static NetServer server;
		private static JsonSerializerSettings serializerSettings;

		private static List<GameUser> users = new List<GameUser>();
		
		public static bool InGame;

		private static void Main(string[] args) {
			for (int i = 0; i < args.Length; i++) {
				if (args[i].ToLower() == "-port" && args[i + 1] != null) {
					if (!Int32.TryParse(args[i + 1], out serverConfig.Port)) {
						Log(String.Format("Specified port '{0}' is invalid.", args[i + 1]));
					} else {
						if (serverConfig.Port < 0 || serverConfig.Port > 25565) {
							Log(String.Format("Specified port '{0}' is out of bounds (0-65535).", args[i + 1]));
							serverConfig.Port = 6666;
						}
					}
				} else if (args[i].ToLower() == "-usercount" && args[i + 1] != null) {
					if (!Int32.TryParse(args[i + 1], out serverConfig.UserCount)) {
						Log(String.Format("Specified user count '{0}' is invalid.", args[i + 1]));
					} else {
						if (serverConfig.UserCount != 1 && serverConfig.UserCount != 2 && serverConfig.UserCount != 4) {
							Log("User count must be 1, 2, or 4.");
							serverConfig.UserCount = 1;
						}
					}
				}
			}
			
			UpdateTitle();

			serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
			
			NetPeerConfiguration config = new NetPeerConfiguration(AppConfig.Name);
			config.Port = serverConfig.Port;
			config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
			
			server = new NetServer(config);
			server.Start();

			Log(String.Format("Server started on port {0}.", serverConfig.Port));

			bool shouldExit = false;
			
			while (!shouldExit) {
				NetIncomingMessage msg;
				while ((msg = server.ReadMessage()) != null) {
					UpdateTitle();
					
					switch (msg.MessageType) {
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
							//Log(msg.ReadString(), MessageType.Test);
							break;

						case NetIncomingMessageType.WarningMessage:
							//Log(msg.ReadString(), MessageType.Warning);
							break;

						case NetIncomingMessageType.ErrorMessage:
							//Log(msg.ReadString(), MessageType.Error);
							break;

						case NetIncomingMessageType.ConnectionApproval:
							NetMessage approvalMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);
							ClientApprovalNetMessage castedMsg = (ClientApprovalNetMessage)approvalMessage;
						
							string ip = msg.SenderConnection.RemoteEndPoint.Address.ToString();
						
							string denialReason = String.Empty;
							
							if (castedMsg.Version != serverConfig.Version) {
								denialReason = "networkError.wrongVersion";
								Log(String.Format("User '{0}' ({1}) denied because of wrong version. ({2})", castedMsg.Username, ip, castedMsg.Version));
							} else if (String.IsNullOrEmpty(castedMsg.Username)) {
								denialReason = "networkError.emptyUsername";
								Log(String.Format("User '{0}' ({1}) denied because of empty username.", castedMsg.Username, ip));
							} else if (users.Select(x => x.Username).Contains(castedMsg.Username)) {
								denialReason = "networkError.duplicateUsername";
								Log(String.Format("User '{0}' ({1}) denied because of duplicate username.", castedMsg.Username, ip));
							} else if (users.Count >= serverConfig.UserCount || InGame) {
								denialReason = "networkError.roomFull";
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
								var response = server.CreateMessage();
								response.Write(JsonConvert.SerializeObject(new ServerApprovalNetMessage(), serializerSettings));
								
								msg.SenderConnection.Approve();
								msg.SenderConnection.SendMessage(response, NetDeliveryMethod.ReliableOrdered, 0);
								
								GameUser newUser = new GameUser(
									msg.SenderConnection.RemoteUniqueIdentifier,
									castedMsg.Username,
									msg.SenderConnection
								);
							
								users.Add(newUser);
								
								SendToAll(new ServerUserJoinNetMessage { NewUser = newUser });
								SendToAll(new ServerUserUpdateNetMessage { Users = users });
								
								Log(String.Format("User '{0}' ({1}) approved.", castedMsg.Username, ip));
							}

							break;

						case NetIncomingMessageType.StatusChanged:
							Log(String.Format("Status changed: {0}", (NetConnectionStatus)msg.ReadByte()));
							break;
							
						case NetIncomingMessageType.Data:
							//double timestamp = msg.ReadTime(false);
							NetMessage netMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);
	
							#region Casting
							
							if (netMessage is ClientDisconnectNetMessage) {
								ClientDisconnectCallback((ClientDisconnectNetMessage)netMessage, msg.SenderConnection);
								break;
							}
							
							if (netMessage is ClientRequestPlayersNetMessage) {
								ClientRequestPlayersCallback((ClientRequestPlayersNetMessage)netMessage, msg.SenderConnection);
								break;
							}
							
							if (netMessage is ClientChatNetMessage) {
								ClientChatCallback((ClientChatNetMessage)netMessage, msg.SenderConnection);
								break;
							}
							
							if (netMessage is ClientLobbyReadyNetMessage) {
								ClientLobbyReadyCallback((ClientLobbyReadyNetMessage)netMessage, msg.SenderConnection);
								break;
							}
							
							if (netMessage is ClientGameReadyNetMessage) {
								ClientGameReadyCallback((ClientGameReadyNetMessage)netMessage, msg.SenderConnection);
								break;
							}
							
							if (netMessage is ClientGridObjectPlacedNetMessage) {
								ClientGridObjectPlacedCallback((ClientGridObjectPlacedNetMessage)netMessage, msg.SenderConnection);
								break;
							}
							
							#endregion

							Log(String.Format("Unhandled netMessage data type: {0}", ((object)netMessage).GetType().Name), MessageType.Warning);
							break;
						
						default:
							Log(String.Format("Unhandled message type {0}: {1}", msg.MessageType, msg.ReadString()), MessageType.Warning);
							break;
					}
				}
			}
		}

		public static void SendToAll(NetMessage matchMsg) {
			string matchMsgSerialized = JsonConvert.SerializeObject(matchMsg, serializerSettings);

			NetOutgoingMessage netMsg = server.CreateMessage();
			netMsg.Write(matchMsgSerialized);
			
			server.SendToAll(netMsg, NetDeliveryMethod.ReliableOrdered);
		}
		
		public static void UpdateTitle() {
			Console.Title = String.Format(
				"Battle Park {0}, {1}/{2} users",
				serverConfig.Version,
				users.Count,
				serverConfig.UserCount
			);
		}
		
		public static GameUser GetUser(long id) {
			return users.First(x => x.Id == id);
		}
		
		#endregion
		
		#region Callbacks
		
		public static void ClientDisconnectCallback(ClientDisconnectNetMessage message, NetConnection sender) {
			SendToAll(new ServerUserLeaveNetMessage { User = GetUser(sender.RemoteUniqueIdentifier) });
			
			Log(String.Format("User '{0}' left.", GetUser(sender.RemoteUniqueIdentifier).Username));
			
			users.Remove(GetUser(sender.RemoteUniqueIdentifier));
			
			SendToAll(new ServerUserUpdateNetMessage { Users = users });
			
			if (InGame && users.Count <= 1) {
				if (users.Count > 0) {
					SendToAll(new ServerEndGameNetMessage { Winner = GetUser(users.First().Id) });
				}
				InGame = false;
			}
		}
		
		public static void ClientRequestPlayersCallback(ClientRequestPlayersNetMessage message, NetConnection sender) {
			SendToAll(new ServerUserUpdateNetMessage { Users = users });
			
			Log(String.Format("User '{0}' requests player list.", GetUser(sender.RemoteUniqueIdentifier).Username));
		}
		
		public static void ClientChatCallback(ClientChatNetMessage message, NetConnection sender) {
			SendToAll(new ServerChatNetMessage {
				Sender = GetUser(sender.RemoteUniqueIdentifier),
				Message = message.Message
			});
			
			Log(String.Format("User '{0}' chats: {1}", GetUser(sender.RemoteUniqueIdentifier).Username, message.Message));
		}
		
		public static void ClientLobbyReadyCallback(ClientLobbyReadyNetMessage message, NetConnection sender) {
			GetUser(sender.RemoteUniqueIdentifier).LobbyReady = true;
			
			SendToAll(new ServerUserUpdateNetMessage {
				Users = users
			});
			
			Log(String.Format("User '{0}' is ready.", GetUser(sender.RemoteUniqueIdentifier).Username));
			
			if (users.Count == serverConfig.UserCount) {
				if (users.All(x => x.LobbyReady)) {
					InGame = true;
					SendToAll(new ServerInitializeGameNetMessage());
					Log(String.Format("All users are ready, sent lobby start message."));
				}
			}
		}
		
		public static void ClientGameReadyCallback(ClientGameReadyNetMessage message, NetConnection sender) {
			GetUser(sender.RemoteUniqueIdentifier).GameReady = true;
			
			Log(String.Format("Player '{0}' is loaded into game.", GetUser(sender.RemoteUniqueIdentifier).Username));
			
			if (users.All(x => x.GameReady)) {
				SendToAll(new ServerStartGameNetMessage {
					Ids = users.Select(x => x.Id).ToList(),
					GridSize = 16
				});
				Log(String.Format("All player are loaded into game, sent game start message."));
			}
		}
		
		
		public static void ClientGridObjectPlacedCallback(ClientGridObjectPlacedNetMessage message, NetConnection sender) {
			SendToAll(new ServerGridObjectPlacedNetMessage {
				Sender = GetUser(sender.RemoteUniqueIdentifier),
				GridObject = message.GridObject,
				Type = message.Type
			});
			
			Log(String.Format(
				"User '{0}' placed a GridObject {1} at {2}, {3}, {4}.",
				GetUser(sender.RemoteUniqueIdentifier).Username,
				message.Type,
				message.GridObject.X,
				message.GridObject.Y,
				message.GridObject.Z
			));
		}

		#endregion
		
		#region Logging
		
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
		
		#endregion
	}
}