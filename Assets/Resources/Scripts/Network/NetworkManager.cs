using System;
using System.Linq;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	public string Ip;
	public int Port;
	
	public int MaxPlayers = 2;
	
	public bool IsServer;
	
	void Start()
	{
		DontDestroyOnLoad(this);
	}
}