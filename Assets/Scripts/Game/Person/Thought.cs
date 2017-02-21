using System;

public struct Thought {
	public DateTime Time { get; private set; }
	public string ThoughtString { get; private set; }
	public string[] Parameters { get; private set; }

	public Thought(string thought, params string[] parameters) {
		this.Time = DateTime.Today;
		this.ThoughtString = thought;
		this.Parameters = parameters;
	}
}