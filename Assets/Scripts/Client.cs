using UnityEngine;
using Lidgren.Network;

public class Client : MonoBehaviour {
	public NetClient client;
	
	void Start () {
		client = new NetClient(new NetPeerConfiguration("Battle Park"));
		
		if (client.ConnectionStatus == NetConnectionStatus.Disconnected) {
			client.Start();
			client.Connect("127.0.0.1", 12345);
		}
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			var message = client.CreateMessage();
			message.Write("bep bep!!!");
			client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
		}
		
		if (client.ConnectionStatus == NetConnectionStatus.Connected) {
			NetIncomingMessage message;
			while ((message = client.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.Data:
						print("Client Data: " + message.ReadString());
						break;
						
					/*case NetIncomingMessageType.StatusChanged:
						switch(message.SenderConnection.Status)
						{
						}
						break;
					*/	
					case NetIncomingMessageType.DebugMessage:
						print("Client Debug: " + message.ReadString());
						break;
						
						/* .. */
					default:
						print("unhandled message with type: " 	+ message.MessageType);
						break;
			    }
			}
		}
	}
}
