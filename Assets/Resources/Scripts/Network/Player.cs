using System;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	void Start()
	{
		DontDestroyOnLoad(this);
	}
}