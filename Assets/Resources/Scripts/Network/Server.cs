using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : MonoBehaviour {
	public NetworkManager networkManager;

	//key = connection id
	//value = username
	Dictionary<int,string> Usernames;
	void Start() {
		Usernames = new Dictionary<int,string> ();
		networkManager = FindObjectOfType<NetworkManager>();
		if (!networkManager.IsServer) {
			Destroy(this);
		} else {
			StartServer();
		}
	}
	
	public void StartServer() {
		NetworkServer.RegisterHandler(ChatNetMessage.Code, ResendMessage);
		NetworkServer.RegisterHandler(GridObjectPlacedNetMessage.Code, ResendMessage);
		NetworkServer.RegisterHandler(ClientJoinedMessage.Code, SendToClients);
		NetworkServer.Listen(networkManager.Port);
		FindObjectOfType<Client>().StartClient();
	}
	
	public void ResendMessage(NetworkMessage incoming) {
		switch (incoming.msgType) {
			case ChatNetMessage.Code:
				ChatNetMessage message = incoming.ReadMessage<ChatNetMessage> ();
				string username;
				if (Usernames.TryGetValue (incoming.conn.connectionId,out username)) {
					message.Username = username;
				} else {
					message.Username = incoming.conn.connectionId.ToString ();
				}
				print (message.Message);
				NetworkServer.SendToAll (incoming.msgType,new ChatNetMessage () {
					Message = message.Message,
					Username = username
				});
				break;
			case GridObjectPlacedNetMessage.Code:
				NetworkServer.SendToAll(incoming.msgType, incoming.ReadMessage<GridObjectPlacedNetMessage>());
				break;
		}
	}
	
	public void SendToClients(NetworkMessage incoming) {
		ClientJoinedMessage message = incoming.ReadMessage<ClientJoinedMessage>();
		Usernames.Add (incoming.conn.connectionId,message.Username);
		NetworkServer.SendToClient(incoming.conn.connectionId, UpdatePlayerAssignment.Code, new UpdatePlayerAssignment() {
			PlayerId = incoming.conn.connectionId
		});
	}
}