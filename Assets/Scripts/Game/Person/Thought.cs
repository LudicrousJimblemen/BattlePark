using System;

public class Thought {
	public DateTime Time { get; private set; }
	public string ThoughtString { get; private set; }
	public string[] Parameters { get; private set; }

	public Thought(string thought, params string[] parameters) {
		this.ThoughtString = thought;
		this.Parameters = parameters;
	}
}