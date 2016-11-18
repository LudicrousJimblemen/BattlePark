using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenBackground : MonoBehaviour {
	public GameObject Fade;
	
	[Range(0f, 20f)]
	public float RotationSpeed = 1f;
	
	public GameObject LeftLeg;
	public GameObject RightLeg;
	public GameObject Body;
	
	[Range(1f, 6f)]
	public float TimerMultiple = 3f;
	
	public GameObject Plane;
	[Range(0f, 0.5f)]
	public float PlaneSpeed = 0.1f;
	
	private int timer;
	
	private Quaternion leftLegRotationPrev;
	private Quaternion rightLegRotationPrev;
	private Quaternion bodyRotationPrev;
	
	private Quaternion leftLegRotation;
	private Quaternion rightLegRotation;
	private Quaternion bodyRotation;
	
	private Image fade;
	
	private void Awake() {
		fade = Fade.GetComponent<Image>();
	}
	
	private void Update() {
		fade.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0), Mathf.SmoothStep(0f, 1f, timer / 60f));
		
		if (timer <= 130) {
			transform.position = Vector3.Lerp(
				new Vector3(0, 0.5f, -1.25f),
				new Vector3(0, 1.8f, -3f),
				Mathf.SmoothStep(0f, 1f, timer / 130f)
			);
		}
		
		transform.RotateAround(Vector3.zero, Vector3.up, RotationSpeed * Mathf.SmoothStep(0f, 1f, (timer - 130f) / 180f));
		
		if (timer % Mathf.RoundToInt(TimerMultiple) == 0) {
			leftLegRotationPrev = LeftLeg.transform.rotation;
			rightLegRotationPrev = RightLeg.transform.rotation;
			bodyRotationPrev = Body.transform.rotation;
			
			leftLegRotation = leftLegRotationPrev * Quaternion.Euler(
				0,
				Random.Range(-25f * TimerMultiple, -15f * TimerMultiple),
				0
			);
			rightLegRotation = rightLegRotationPrev * Quaternion.Euler(
				0,
				Random.Range(-25f * TimerMultiple, -15f * TimerMultiple),
				0
			);
			bodyRotation = bodyRotationPrev * Quaternion.Euler(
				0,
				Random.Range(-8f * TimerMultiple, 8f * TimerMultiple),
				0
			);
		}
		
		LeftLeg.transform.rotation = Quaternion.Lerp(leftLegRotationPrev, leftLegRotation, ((timer % TimerMultiple) + 1) / TimerMultiple);
		RightLeg.transform.rotation = Quaternion.Lerp(rightLegRotationPrev, rightLegRotation, ((timer % TimerMultiple) + 1) / TimerMultiple);
		Body.transform.rotation = Quaternion.Lerp(bodyRotationPrev, bodyRotation, ((timer % TimerMultiple) + 1) / TimerMultiple);
		
		Plane.transform.Translate(0, 0, PlaneSpeed);
		
		if (Plane.transform.position.z >= 8f) {
			Plane.transform.position = Vector3.zero;
		}
		
		timer++;
	}
}