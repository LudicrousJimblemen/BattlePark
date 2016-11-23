using System;
using UnityEngine;
using Lidgren.Network;

public class GameClient : MonoBehaviour {
	private NetMessenger messenger;
	
	public long UniqueId;
	
	public void Chat(string from, string text) {
		messenger.SendMessage(new ChatNetMessage(from, text));
	}
	
	private void Start() {
		DontDestroyOnLoad(gameObject);
		
		messenger.CreateListener<ChatNetMessage>(ChatCallback);
		
		UniqueId = messenger.GetUniqueId();
	}
	
	private void ChatCallback(ChatNetMessage message) {
	}
}