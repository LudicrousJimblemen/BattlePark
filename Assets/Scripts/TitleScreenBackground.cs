using System.Linq;
using UnityEngine;

namespace BattlePark.Menu {
	public class TitleScreenBackground : MonoBehaviour {
		[Range(0f, 20f)]
		public float CameraRotationSpeed = 0.25f;
	
		[Range(1f, 6f)]
		public float PersonRotationTimerMultiple = 3f;
		public GameObject LeftLeg;
		public GameObject RightLeg;
		public GameObject Body;
	
		[Range(0f, 0.5f)]
		public float PlaneSpeed = 0.1f;
		public GameObject Plane;
	
		private int timer;
	
		private Quaternion leftLegRotationPrev;
		private Quaternion rightLegRotationPrev;
		private Quaternion bodyRotationPrev;
	
		private Quaternion leftLegRotation;
		private Quaternion rightLegRotation;
		private Quaternion bodyRotation;
		
		private void Update() {
			if (timer <= 130) {
				transform.position = Vector3.Lerp(
					new Vector3(0, 0.5f, -1.25f),
					new Vector3(0, 1.8f, -3f),
					Mathf.SmoothStep(0f, 1f, timer / 130f)
				);
			}
		
			transform.RotateAround(Vector3.zero, Vector3.up, CameraRotationSpeed * Mathf.SmoothStep(0f, 1f, (timer - 130f) / 180f));
		
			if (timer % Mathf.RoundToInt(PersonRotationTimerMultiple) == 0) {
				leftLegRotationPrev = LeftLeg.transform.rotation;
				rightLegRotationPrev = RightLeg.transform.rotation;
				bodyRotationPrev = Body.transform.rotation;
			
				leftLegRotation = leftLegRotationPrev * Quaternion.Euler(
					Random.Range(-25f * PersonRotationTimerMultiple, -15f * PersonRotationTimerMultiple),
					0,
					0
				);
				rightLegRotation = rightLegRotationPrev * Quaternion.Euler(
					Random.Range(-25f * PersonRotationTimerMultiple, -15f * PersonRotationTimerMultiple),
					0,
					0
				);
				bodyRotation = bodyRotationPrev * Quaternion.Euler(
					Random.Range(-8f * PersonRotationTimerMultiple, 8f * PersonRotationTimerMultiple),
					0,
					0
				);
			}
		
			LeftLeg.transform.rotation = Quaternion.Lerp(leftLegRotationPrev, leftLegRotation, ((timer % PersonRotationTimerMultiple) + 1) / PersonRotationTimerMultiple);
			RightLeg.transform.rotation = Quaternion.Lerp(rightLegRotationPrev, rightLegRotation, ((timer % PersonRotationTimerMultiple) + 1) / PersonRotationTimerMultiple);
			Body.transform.rotation = Quaternion.Lerp(bodyRotationPrev, bodyRotation, ((timer % PersonRotationTimerMultiple) + 1) / PersonRotationTimerMultiple);
		
			Plane.transform.Translate(0, 0, PlaneSpeed);
		
			if (Plane.transform.position.z >= 8f) {
				Plane.transform.position = Vector3.zero;
			}
		
			timer++;
		}
	}
}