using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Client : MonoBehaviour {
	public NetworkClient client;

	void Start() {
		client = new NetworkClient();
		client.RegisterHandler(ChatMessage.Code, OnChatMessage);
	}

	public void StartClient(string port) {
		client.Connect(FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text.Trim(), Int32.Parse(port.Trim()));
	}
	
	public void OnChatMessage(NetworkMessage incoming) {	
        ChatMessage message = incoming.ReadMessage<ChatMessage>();
        print("ChatMessage: " + message.Message);
    }

	public void SendClientMessage() {
		ChatMessage outgoingMessage = new ChatMessage() {
			Message = FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").text
		};
		FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").text = String.Empty;
		client.Send(ChatMessage.Code, outgoingMessage);
	}
}
