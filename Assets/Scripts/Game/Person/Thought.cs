using System;
using UnityEngine;

public class Thought {
	public DateTime Time { get; private set; }
	
	public string ThoughtString;
	public string[] Parameters;
	
	public Thought(string thoughtString, params string[] parameters) {
		this.Time = DateTime.Now;
		this.ThoughtString = thoughtString;
		this.Parameters = parameters;
	}
}