using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	private NetworkManager networkManager;
	
	void Start() {
		FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text = "6666";
		networkManager = FindObjectOfType<NetworkManager>();
	}
	
	public void StartServer() {
		networkManager.IsServer = true;
		networkManager.Ip = "127.0.0.1";
		networkManager.Port = Int32.Parse(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		UnityEngine.SceneManagement.SceneManager.LoadScene("GridTest");
	}

	public void StartClient() {
		networkManager.IsServer = false;
		networkManager.Ip = FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text;
		networkManager.Port = Int32.Parse(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		UnityEngine.SceneManagement.SceneManager.LoadScene("GridTest");
	}
}