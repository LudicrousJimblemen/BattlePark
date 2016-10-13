using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	public void StartServer() {
		FindObjectOfType<Server>().StartServer();
		FindObjectOfType<InputField>().interactable = true;
		FindObjectsOfType<Button>().First(x => x.name == "StartClient").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "StartServer").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "SendClient").interactable = false;
		FindObjectsOfType<Button>().First(x => x.name == "SendServer").interactable = true;
	}
	
	public void StartClient() {
		FindObjectOfType<Client>().StartClient();
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