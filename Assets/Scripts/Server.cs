using UnityEngine;
using Lidgren.Network;

public class Server : MonoBehaviour {
	public NetServer server;

	// Use this for initialization
	void Start () {
		server = new NetServer(new NetPeerConfiguration("Battle Park") { Port = 12345 });
		server.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if (server.Status == NetPeerStatus.Running) {
			NetIncomingMessage message;
			while ((message = server.ReadMessage()) != null)
			{
				switch (message.MessageType)
				{
					case NetIncomingMessageType.Data:
						print("Server Data: " + message.ReadString());
						break;
						
					/*case NetIncomingMessageType.StatusChanged:
						switch(message.SenderConnection.Status)
						{
						}
						break;
					*/	
					case NetIncomingMessageType.DebugMessage:
						print("Server Debug: " + message.ReadString());
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