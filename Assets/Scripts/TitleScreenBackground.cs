using System.Collections;
using UnityEngine;

namespace BattlePark.Menu {
	public class TitleScreenBackground : MonoBehaviour {
		[Range(20f, 50)]
		public float CameraRotationSpeed = 25;
	
		[Range(1f, 6f)]
		public float PersonRotationTimerMultiple = 3f;
		public GameObject LeftLeg;
		public GameObject RightLeg;
		public GameObject Body;
	
		[Range(5, 10)]
		public float PlaneSpeed = 5;
		public GameObject Plane;
	
		private Quaternion leftLegRotationPrev;
		private Quaternion rightLegRotationPrev;
		private Quaternion bodyRotationPrev;
	
		private Quaternion leftLegRotation;
		private Quaternion rightLegRotation;
		private Quaternion bodyRotation;

		public void Start() {
			StartCoroutine(CameraMove());
			StartCoroutine(PlaneMove());
			StartCoroutine(GenerateRotationDeltas());
			StartCoroutine(Run());
		}

		IEnumerator CameraMove() {
			float startTimer = 0;
			while (true) {
				if (startTimer <= 1.3f) {
					transform.position = Vector3.Lerp(
						new Vector3(0,0.5f,-1.25f),
						new Vector3(0,1.8f,-3f),
						Mathf.SmoothStep(0f,1f,startTimer / 1.3f)
					);
				}
				startTimer += Time.deltaTime;
				transform.RotateAround(Vector3.zero,Vector3.up,CameraRotationSpeed * Time.deltaTime * Mathf.SmoothStep(0, 1, (startTimer - 1.3f) / 1.8f));
				yield return null;
			}
		}
		IEnumerator PlaneMove() {
			while (true) {
				Plane.transform.Translate(0,0,PlaneSpeed * Time.deltaTime);
				if(Plane.transform.position.z >= 8f) {
					Plane.transform.position = Vector3.zero;
				}
				yield return null;
			}
		}
		IEnumerator GenerateRotationDeltas() {
			while (true) {
				leftLegRotationPrev = LeftLeg.transform.rotation;
				rightLegRotationPrev = RightLeg.transform.rotation;
				bodyRotationPrev = Body.transform.rotation;

				leftLegRotation = leftLegRotationPrev * Quaternion.Euler(
					Random.Range(-25f * PersonRotationTimerMultiple,-15f * PersonRotationTimerMultiple),
					0,
					0
				);
				rightLegRotation = rightLegRotationPrev * Quaternion.Euler(
					Random.Range(-25f * PersonRotationTimerMultiple,-15f * PersonRotationTimerMultiple),
					0,
					0
				);
				bodyRotation = bodyRotationPrev * Quaternion.Euler(
					Random.Range(-8f * PersonRotationTimerMultiple,8f * PersonRotationTimerMultiple),
					0,
					0
				);
				for (int i = 0; i < PersonRotationTimerMultiple; i ++) {
					yield return null;
				}
			}
		}
		IEnumerator Run () {
			while (true) {
				LeftLeg.transform.rotation = Quaternion.Lerp(LeftLeg.transform.rotation,leftLegRotation,25 * Time.deltaTime);
				RightLeg.transform.rotation = Quaternion.Lerp(RightLeg.transform.rotation,rightLegRotation,25 * Time.deltaTime);
				Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation,bodyRotation,25 * Time.deltaTime);
				yield return null;
			}
		}
	}
}