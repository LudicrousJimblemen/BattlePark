using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class VendorMacaroni : VendorFood {
	public override float Saltiness { get { return 20f; } }
	public override float Sourness { get { return 2f; } }
	public override float Sweetness { get { return 0; } }
	public override float Bitterness { get { return 2f; } }
	public override float Juiciness { get { return 5f; } }
	public override float Spiciness { get { return 0; } }
}
