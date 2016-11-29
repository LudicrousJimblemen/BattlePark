using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEngine;
using Lidgren.Network;
using Newtonsoft.Json;
using BattlePark.Core;

public class Server {
	private NetServer server = new NetServer(new NetPeerConfiguration("Battle Park"));

	private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

	private List<GameUser> users = new List<GameUser>();

	public void UpdateListeners() {
		NetIncomingMessage msg;
		while ((msg = server.ReadMessage()) != null) {
			switch (msg.MessageType) {
				case NetIncomingMessageType.DebugMessage:
				case NetIncomingMessageType.VerboseDebugMessage:
					Debug.Log(msg.ReadString());
					break;

				case NetIncomingMessageType.WarningMessage:
					Debug.LogWarning(msg.ReadString());
					break;

				case NetIncomingMessageType.ErrorMessage:
					Debug.LogError(msg.ReadString());
					break;

				case NetIncomingMessageType.ConnectionApproval:
					
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

					Debug.Log("ah! Ah!!! AH!!!!! UNHANLED MESSAGE TYPE IN SERVER!!!!!!! " + msg.ReadString());

					break;
				default:
					Debug.Log(String.Format("UNHANDLED!!! AAA!!! {0} MESSAGE: {1}", msg.MessageType, msg.ReadString()));
					break;
			}
		}
	}

	public void Close() {
		server.Shutdown(String.Empty);
	}

	public void StartServer(int port = 6666) {
		NetPeerConfiguration config = new NetPeerConfiguration("Battle Park");
		config.Port = port;
		config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
		server = new NetServer(config);
		server.Start();
	}

	private void SendToAll(NetMessage matchMsg) {
		string matchMsgSerialized = JsonConvert.SerializeObject(matchMsg, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

		NetOutgoingMessage netMsg = server.CreateMessage();
		netMsg.WriteTime(false);
		netMsg.Write(matchMsgSerialized);
		server.SendToAll(netMsg, NetDeliveryMethod.ReliableOrdered);
	}
}