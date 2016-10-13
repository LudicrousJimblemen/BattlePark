using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

public class Server : MonoBehaviour {
	public NetServer server;

	void Start () {
		server = new NetServer(new NetPeerConfiguration("Battle Park") { Port = 12345 });
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (server.Status == NetPeerStatus.Running) {
			NetIncomingMessage message;
			while ((message = server.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.Data:
						print("Server Data: " + message.ReadString());
						break;
						
					case NetIncomingMessageType.StatusChanged:
						print("Server Status: " + message.ReadString());
						break;
						
					case NetIncomingMessageType.DebugMessage:
						print("Server Debug: " + message.ReadString());
						break;
						
					default:
						print("Server unhandled message with type: " 	+ message.MessageType);
						break;
			    }
			}
		}
	}

	public void StartServer() {
		server.Start();
	}
	
	public void SendServerMessage() {
		var outgoingMessage = server.CreateMessage();
		outgoingMessage.Write(FindObjectOfType<InputField>().text);
		FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").text = String.Empty;
		foreach (var connection in server.Connections) {
			server.SendMessage(outgoingMessage, connection, NetDeliveryMethod.ReliableOrdered);				
		}
	}
}