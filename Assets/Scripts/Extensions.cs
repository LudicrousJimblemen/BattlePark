using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

	#region Vector3
	public static Vector3 SetX(this Vector3 vec,float Value) {
		Vector3 ret = vec;
		ret.x = Value;
		vec = ret;
		return ret;
	}
	public static Vector3 SetY(this Vector3 vec,float Value) {
		Vector3 ret = vec;
		ret.y = Value;
		vec = ret;
		return ret;
	}
	public static Vector3 SetZ(this Vector3 vec,float Value) {
		Vector3 ret = vec;
		ret.z = Value;
		vec = ret;
		return ret;
	}
	#endregion
}
