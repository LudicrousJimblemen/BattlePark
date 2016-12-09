using UnityEngine;
using System.Collections;

namespace BattlePark {
	public struct GridObjectProperty {
		public string Name;
		public object Value;
		public PropertyType PropertyType;
	}
	public enum PropertyType {
		UNCLAMPED_NUM,
		CLAMPED_NUM,
		BOOLEAN,
		STRING,
		COLOR,
	}
}
