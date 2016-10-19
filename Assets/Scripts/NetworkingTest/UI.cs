using System.Net;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	public void StartServer() {
		FindObjectOfType<Server>().StartServer(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		FindObjectsOfType<InputField>().First(x => x.name == "ChatInput").interactable = true;
		FindObjectsOfType<InputField>().First(x => x.name == "PortInput").interactable = false;
		FindObjectsOfType<InputField>().First(x => x.name == "IPInput").interactable = false;
        WebResponse resp = WebRequest.Create("http://checkip.dyndns.org").GetResponse();
        StreamReader reader = new StreamReader(resp.GetResponseStream());
        FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = reader.ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<')[0];
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