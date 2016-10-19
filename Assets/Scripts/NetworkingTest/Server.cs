using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

public class Server : MonoBehaviour {
	public NetServer server = new NetServer(new NetPeerConfiguration("Battle Park") { Port = 6666 });

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
						print("Server unhandled message with type: " + message.MessageType + " - " + message.ReadString());
						break;
			    }
			}
		}
	}

	public void StartServer(string port) {
        server = new NetServer(new NetPeerConfiguration("Battle Park") { Port = Int32.Parse(port.Trim()) });
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