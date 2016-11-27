using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEngine;
using Lidgren.Network;
using Newtonsoft.Json;

public class Client {
	private NetClient client = new NetClient(new NetPeerConfiguration("Battle Park"));

	private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

	private List<NetMessageListener> listeners = new List<NetMessageListener>();

	public void CreateListener<T>(NetMessage<T> handler) where T : NetMessage {
		listeners.Add(new NetMessageListener(typeof(T), handler));
	}

	public bool RemoveListener<T>(NetMessage<T> handler) where T : NetMessage {
		for (int i = 0; i < listeners.Count; i++) {
			NetMessageListener listenerEntry = listeners[i];
			if (listenerEntry.MessageType == typeof(T) && (NetMessage<T>) listenerEntry.Handler == handler) {
				listeners.Remove(listenerEntry);
				return true;
			}
		}
		return false;
	}

	public void SendMessage<T>(T message) {
		NetOutgoingMessage netMessage = client.CreateMessage();
		netMessage.WriteTime(false);

		string data = JsonConvert.SerializeObject(message, serializerSettings);
		netMessage.Write(data);

		client.SendMessage(netMessage, NetDeliveryMethod.ReliableOrdered);
	}

	public void UpdateListeners() {
		NetIncomingMessage msg;
		while ((msg = client.ReadMessage()) != null) {
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

				case NetIncomingMessageType.StatusChanged:
					NetConnectionStatus status = (NetConnectionStatus) msg.ReadByte();
					string statusMsg = msg.ReadString();

					switch (status) {
						default:
							Debug.Log("Status change received: " + status + " - Message: " + statusMsg);
							break;
					}
					break;

				case NetIncomingMessageType.Data:
					double timestamp = msg.ReadTime(false);
					NetMessage message = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);

					MethodInfo methodToCall = typeof(Client).GetMethod("ReceiveMessage", BindingFlags.NonPublic | BindingFlags.Instance);
					MethodInfo genericVersion = methodToCall.MakeGenericMethod(message.GetType());
					genericVersion.Invoke(this, new object[] {
						message,
						timestamp
					});
					break;

				default:
					Debug.Log("Received unhandled message of type " + msg.MessageType);
					break;
			}
		}
	}

	public void Close() {
		client.Disconnect(String.Empty);
	}

	public long GetUniqueId() {
		return client.UniqueIdentifier;
	}

	public void JoinOnlineGame(string ip = "127.0.0.1", int port = 6666) {
		NetPeerConfiguration conf = new NetPeerConfiguration("Battle Park");
		client = new NetClient(conf);
		client.Start();

		client.Connect(ip, port);
	}

	private void ReceiveMessage<T>(T message, double timestamp) where T : NetMessage {
		for (int i = 0; i < listeners.Count; i++) {
			NetMessageListener listener = listeners[i];
			if (listener.MessageType == message.GetType()) {
				((NetMessage<T>) listener.Handler).Invoke(message);
			}
		}
	}
}