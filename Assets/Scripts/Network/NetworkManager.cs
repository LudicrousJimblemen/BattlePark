using System;
using System.Linq;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	public string Ip;
	public int Port;
	
	public bool IsServer;
	
	void Start()
	{
		DontDestroyOnLoad(this);
	}
}