using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class VendorFood : VendorItem {
	[Range(0, 100)]
	public float Explosivosity;
	
	public virtual float Saltiness { get { return 0; } }
	public virtual float Sourness { get { return 0; } }
	public virtual float Sweetness { get { return 0; } }
	public virtual float Bitterness { get { return 0; } }
	public virtual float Juiciness { get { return 0; } }
	public virtual float Spiciness { get { return 0; } }
}
