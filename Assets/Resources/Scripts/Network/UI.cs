using System;
using System.Net;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	void Start() {
		FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text = "25001";
		try {	
			FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
		} catch (Exception e) {
			System.Diagnostics.Debug.WriteLine(e.Message);				
			FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text = "127.0.0.1";
		}
		
		GenerateUsername();
	}
	
	public void StartServer() {
		NetworkManager.IsServer = true;
		NetworkManager.Ip = FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text;
		NetworkManager.Port = Int32.Parse(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		NetworkManager.Username = FindObjectsOfType<InputField>().First(x => x.name == "UsernameInput").text;
		UnityEngine.SceneManagement.SceneManager.LoadScene("GridTest");
	}

	public void StartClient() {
		NetworkManager.IsServer = false;
		NetworkManager.Ip = FindObjectsOfType<InputField>().First(x => x.name == "IPInput").text;
		NetworkManager.Port = Int32.Parse(FindObjectsOfType<InputField>().First(x => x.name == "PortInput").text);
		NetworkManager.Username = FindObjectsOfType<InputField>().First(x => x.name == "UsernameInput").text;
		UnityEngine.SceneManagement.SceneManager.LoadScene("GridTest");
	}
	
	public void GenerateUsername() {
		if (Input.GetMouseButton(0)) {
			return;
		}
		
		string consonants = "bbbbbbbbbbbbbbbbbcdfghjklmnpppppppppppppppppqrssstvwxzzz";
		string vowels = "aaeeiioooooooouuuuuuuuy";
		int type = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));

		string returnedName = String.Empty;
		for (int i = 0; i < 14; i++) {
			if (i != 7) {
				float chance = UnityEngine.Random.value;
				if (type == 0) {
					returnedName += consonants.ElementAt(UnityEngine.Random.Range(0, consonants.Length));
					if (chance <= 0.5) {
						type = 1;
					}
				} else {
					returnedName += vowels.ElementAt(UnityEngine.Random.Range(0, vowels.Length));
					if (chance <= 0.6) {
						type = 0;
					}
				}
			} else {
				returnedName += " ";
			}
		}
		
		FindObjectsOfType<InputField>().First(x => x.name == "UsernameInput").text = returnedName;
	}
}