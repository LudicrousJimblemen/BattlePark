using System.Net;
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
						switch(message.SenderConnection.Status)
						{
							default:
								break;
						}
						break;
					case NetIncomingMessageType.DebugMessage:
						break;
					default:
						print("Server unhandled message with type: " + message.MessageType);
						break;
			    }
			}
		}
	}

	public void StartServer() {
		print(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString());
		server.Start();
	}
	
	public void SendServerMessage() {
		var outgoingMessage = server.CreateMessage();
		outgoingMessage.Write(FindObjectOfType<InputField>().text);
		foreach (var connection in server.Connections) {
			server.SendMessage(outgoingMessage, connection, NetDeliveryMethod.ReliableOrdered);				
		}
	}
}