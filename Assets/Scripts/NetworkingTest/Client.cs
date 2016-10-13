using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

public class Client : MonoBehaviour {
	public NetClient client;

	void Start () {
		client = new NetClient(new NetPeerConfiguration("Battle Park"));
	}
	
	void Update ()
	{
		if (client.ConnectionStatus == NetConnectionStatus.Connected) {
			NetIncomingMessage message;
			while ((message = client.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.Data:
						print("Client Data: " + message.ReadString());
						break;
						
					case NetIncomingMessageType.StatusChanged:
						print("Client Status: " + message.ReadString());
						break;
						
					case NetIncomingMessageType.DebugMessage:
						print("Client Debug: " + message.ReadString());
						break;
						
					default:
						print("Client unhandled message with type: " 	+ message.MessageType);
						break;
			    }
			}
		}
	}
	
	public void StartClient() {
		if (client.ConnectionStatus == NetConnectionStatus.Disconnected) {
			client.Start();
			client.Connect(FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text, 12345);
		}
	}
	
	public void SendClientMessage() {
		var outgoingMessage = client.CreateMessage();
		outgoingMessage.Write(FindObjectOfType<InputField>().text);
		FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = String.Empty;
		client.SendMessage(outgoingMessage, NetDeliveryMethod.ReliableOrdered);
	}
}
