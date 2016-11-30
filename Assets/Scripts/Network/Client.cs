using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEngine;
using Lidgren.Network;
using Newtonsoft.Json;
using BattlePark.Core;

namespace BattlePark {
	public class Client : MonoBehaviour {
		private NetClient client = new NetClient(new NetPeerConfiguration(GameConfig.Name));

		private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

		private List<NetMessageListener> listeners = new List<NetMessageListener>();

		public void CreateListener<T>(NetMessageHandler<T> handler) where T : NetMessage {
			listeners.Add(new NetMessageListener(typeof(T), handler));
		}

		public bool RemoveListener<T>(NetMessageHandler<T> handler) where T : NetMessage {
			for (int i = 0; i < listeners.Count; i++) {
				NetMessageListener listenerEntry = listeners[i];
				if (listenerEntry.MessageType == typeof(T) && (NetMessageHandler<T>)listenerEntry.Handler == handler) {
					listeners.Remove(listenerEntry);
					return true;
				}
			}
			return false;
		}

		public void SendMessage<T>(T message) {
			NetOutgoingMessage netMessage = client.CreateMessage();
			
			netMessage.Write(JsonConvert.SerializeObject(message, serializerSettings));

			client.SendMessage(netMessage, NetDeliveryMethod.ReliableOrdered);
		}

		public void Close() {
			client.Disconnect(String.Empty);
		}

		public long GetUniqueId() {
			return client.UniqueIdentifier;
		}

		public void JoinOnlineGame(string username, string ip = "127.0.0.1", int port = 6666) {
			client.Start();
			
			var approval = client.CreateMessage();
			approval.Write(JsonConvert.SerializeObject(new ClientApprovalNetMessage { Username = username, Version = GameConfig.Version }, serializerSettings));

			client.Connect(ip, port, approval);
		}
		
		private void Start() {
			DontDestroyOnLoad(gameObject);
		}

		private void Update() {
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
						
						switch (status) {
							case NetConnectionStatus.Connected:
								NetMessage connectionMessage;
								try {
									connectionMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);
								} catch (Exception) {
									connectionMessage = new ServerApprovalNetMessage();
								}
								
								ReceiveMessage<ServerApprovalNetMessage>((ServerApprovalNetMessage)connectionMessage);
								break;
								
							case NetConnectionStatus.Disconnected:
								NetMessage disconnectionMessage;
								try {
									disconnectionMessage = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);
								} catch (Exception) {
									disconnectionMessage = new ServerDenialNetMessage { Reason = "error.notFound" };
								}
								
								ReceiveMessage<ServerDenialNetMessage>((ServerDenialNetMessage)disconnectionMessage);
								break;
								
							default:
								Debug.Log("Status change received: " + status + " - Message: " + msg.ReadString());
								break;
						}
						break;

					case NetIncomingMessageType.Data:
						NetMessage message = JsonConvert.DeserializeObject<NetMessage>(msg.ReadString(), serializerSettings);

						MethodInfo methodToCall = typeof(Client).GetMethod("ReceiveMessage", BindingFlags.NonPublic | BindingFlags.Instance);
						MethodInfo genericVersion = methodToCall.MakeGenericMethod(message.GetType());
						genericVersion.Invoke(this, new object[] {
							message
						});
						break;

					default:
						Debug.Log("Received unhandled message of type " + msg.MessageType);
						break;
				}
			}
		}

		private void ReceiveMessage<T>(T message) where T : NetMessage {
			for (int i = 0; i < listeners.Count; i++) {
				NetMessageListener listener = listeners[i];
				if (listener.MessageType == message.GetType()) {
					((NetMessageHandler<T>)listener.Handler).Invoke(message);
				}
			}
		}
	}
}