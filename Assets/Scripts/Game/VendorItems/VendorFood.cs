using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class VendorFood : VendorItem {
	[Range(0, 100)]
	public float Explosivosity;
	
	public abstract float Saltiness { get { return 0; } }
	public abstract float Sourness { get { return 0; } }
	public abstract float Sweetness { get { return 0; } }
	public abstract float Bitterness { get { return 0; } }
	public abstract float Juiciness { get { return 0; } }
	public abstract float Spiciness { get { return 0; } }
}
