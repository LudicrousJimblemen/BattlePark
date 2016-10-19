using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	public void StartServer() {
		FindObjectOfType<Server>().StartServer(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").interactable = true;
		FindObjectsOfType<InputField>().First(x => x.name == "PortInput").interactable = false;
		FindObjectsOfType<InputField>().First(x => x.name == "IPInput").interactable = false;
		FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
		FindObjectsOfType<Button>().First(x => x.name == "StartClient").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "StartServer").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "SendClient").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "SendServer").interactable = true;
	}

	public void StartClient() {
		FindObjectOfType<Client>().StartClient(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").interactable = true;
		FindObjectsOfType<InputField>().First(x => x.name == "PortInput").interactable = false;
		FindObjectsOfType<InputField>().First(x => x.name == "IPInput").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "StartClient").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "StartServer").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "SendClient").interactable = true;
		FindObjectsOfType<Button>().First(x => x.name == "SendServer").interactable = false;
	}

	public void SendServerMessage() {
		FindObjectOfType<Server>().SendServerMessage();
	}

	public void SendClientMessage() {
		FindObjectOfType<Client>().SendClientMessage();
	}
}