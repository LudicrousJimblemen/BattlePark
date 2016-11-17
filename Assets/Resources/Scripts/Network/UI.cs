using System;
using System.Net;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	private NetworkManager networkManager;
	
	void Start() {
		FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text = "25001";
		try {	
			FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
		} catch (Exception e) {
			System.Diagnostics.Debug.WriteLine(e.Message);				
			FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = "127.0.0.1";
		}
		string username = Username ();
		FindObjectsOfType<InputField> ().First (x => x.name == "UsernameInput").text = username;
		networkManager = FindObjectOfType<NetworkManager>();
	}
	
	public void StartServer() {
		networkManager.IsServer = true;
		networkManager.Ip = FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text;
		networkManager.Port = Int32.Parse(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		networkManager.Username = FindObjectsOfType<InputField> ().First (x => x.name == "UsernameInput").text;
		UnityEngine.SceneManagement.SceneManager.LoadScene("GridTest");
	}

	public void StartClient() {
		networkManager.IsServer = false;
		networkManager.Ip = FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text;
		networkManager.Port = Int32.Parse(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		networkManager.Username = FindObjectsOfType<InputField> ().First (x => x.name == "UsernameInput").text;
		UnityEngine.SceneManagement.SceneManager.LoadScene("GridTest");
	}

	public string Username () {
		string consonants = "bbbbbbbbbbbbbbbbbcdfghjklmnpppppppppppppppppqrssstvwxzzz";
		string vowels = "aaeeiioooooooouuuuuuuuy";
		int type = Mathf.RoundToInt (UnityEngine.Random.Range (0,1));

		System.Random random = new System.Random ();

		string Name = String.Empty;
		for (int i = 0; i < 14; i++) {
			if (i != 7) {
				float chance = UnityEngine.Random.value;
				if (type == 0) {
					Name += consonants.ElementAt(UnityEngine.Random.Range (0, consonants.Length));
					if (chance <= 0.5) {
						type = 1;
					}
				} else {
					Name += vowels.ElementAt(UnityEngine.Random.Range (0,vowels.Length));
					if (chance <= 0.6) {
						type = 0;
					}
				}
			} else {
				Name += " ";
			}
		}
		return Name;
	}
}