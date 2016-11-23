using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NetworkManager {
	public static string Ip = "127.0.0.1";
	public static int Port = 6666;

	public static string Username;
	public static bool IsServer;
	
	public static int MaxUsers = 2;
}