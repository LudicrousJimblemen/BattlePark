using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Person : MonoBehaviour {
	public Thoughts Thoughts { get; set; }

	private void Start() {
		Thoughts.Think("person.thoughts.wantFood.ludicrous", "MINDBLOWING MACARONI");
	}
}