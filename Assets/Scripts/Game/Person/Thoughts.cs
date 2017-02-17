using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Thoughts : IEnumerable<Thought> {
	private List<Thought> thoughts = new List<Thought>();

	IEnumerator IEnumerable.GetEnumerator() {
		return thoughts.GetEnumerator();
	}

	public IEnumerator<Thought> GetEnumerator() {
		return thoughts.AsEnumerable().GetEnumerator();
	}

	public void Think(string thought, params string[] parameters) {
		thoughts.Add(new Thought(thought, parameters));
	}
}