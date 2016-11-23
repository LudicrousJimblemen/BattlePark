using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEngine;
using Lidgren.Network;

public class MatchMessageListener {
	public Type MessageType { get; private set; }
	public object Handler { get; private set; }

	public MatchMessageListener(Type messageType, object handler) {
		MessageType = messageType;
		Handler = handler;
	}
}

public class NetMessenger {
	private NetClient client;

	private Newtonsoft.Json.JsonSerializerSettings serializerSettings;
	
	private List<MatchMessageListener> listeners = new List<MatchMessageListener>();

	public event EventHandler OnChat;

	public NetMessenger(NetClient client) {
		this.client = client;

		serializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
		serializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
	}

	public void CreateListener<T>(MatchMessageHandler<T> handler) where T : MatchMessage {
		listeners.Add(new MatchMessageListener(typeof(T), handler));
	}

	public bool RemoveListener<T>(MatchMessageHandler<T> handler) where T : MatchMessage {
		for (int i = 0; i < listeners.Count; i++) {
			MatchMessageListener listenerEntry = listeners[i];
			if (listenerEntry.MessageType == typeof(T) && (MatchMessageHandler<T>)listenerEntry.Handler == handler) {
				listeners.Remove(listenerEntry);
				return true;
			}
		}
		return false;
	}

	public void SendMessage<T>(T message) {
		NetOutgoingMessage netMessage = client.CreateMessage();
		netMessage.WriteTime(false);

		string data = Newtonsoft.Json.JsonConvert.SerializeObject(message, serializerSettings);
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
					NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
					string statusMsg = msg.ReadString();

					switch (status) {
						default:
							Debug.Log("Status change received: " + status + " - Message: " + statusMsg);
							break;
					}
					break;

				case NetIncomingMessageType.Data:
					double timestamp = msg.ReadTime(false);
					MatchMessage message = Newtonsoft.Json.JsonConvert.DeserializeObject<MatchMessage>(msg.ReadString(), serializerSettings);

					MethodInfo methodToCall = typeof(NetMessenger).GetMethod("ReceiveMessage");
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
		client.Disconnect("Client left the match");
	}
	
	public long GetUniqueId() {
		return client.UniqueIdentifier;
	}
	
	public void JoinOnlineGame(string ip = "127.0.0.1", int port = 6666) {
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
		
		NetPeerConfiguration conf = new NetPeerConfiguration("Battle Park");
		client = new NetClient(conf);
		client.Start();

		NetOutgoingMessage approval = client.CreateMessage();
		
		approval.Write(Newtonsoft.Json.JsonConvert.SerializeObject(info));

		client.Connect(endpoint);
	}


	private void ReceiveMessage<T>(T message, double timestamp) where T : MatchMessage {
		float travelTime = (float)(NetTime.Now - timestamp);

		for (int i = 0; i < listeners.Count; i++) {
			MatchMessageListener listener = listeners[i];
			if (listener.MessageType == message.GetType()) {
				((MatchMessageHandler<T>)listener.Handler).Invoke(message, travelTime);
			}
		}
	}
}