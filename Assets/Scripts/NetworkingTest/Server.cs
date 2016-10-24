using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Server : MonoBehaviour {
	public void StartServer(string port) {
		NetworkServer.Listen(Int32.Parse(port.Trim()));
		NetworkServer.RegisterHandler(ChatMessage.Code, OnChatMessage);
		print("Server started");
	}
	
	public void OnChatMessage(NetworkMessage incoming) {
        var message = incoming.ReadMessage<ChatMessage>();
        print("ChatMessage: " + message.Message);
    }

	public void SendServerMessage() {
		ChatMessage outgoingMessage = new ChatMessage() {
			Message = FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").text
		};
		FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").text = String.Empty;
		NetworkServer.SendToAll(ChatMessage.Code, outgoingMessage);
	}
}