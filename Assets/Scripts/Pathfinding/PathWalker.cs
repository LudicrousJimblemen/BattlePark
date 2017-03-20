using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Pathfinding {
	[RequireComponent(typeof(CharacterController))]
	public class PathWalker : MonoBehaviour {
		public Action DestinationReachedCallback;
		
		public NodeGraph graph;
		public float Speed = 5;
		public float TurnSpeed = 5;
		public float TurnDistance = 1;
		public float StopDistance = 0.1f;
		[Tooltip("The time in seconds of being still before the unit stops trying to path")]
		public float GiveUpTime = 5f;
		[Tooltip("One half of the angle of the cone representing a unit's field of view")]
		public float FOVAngle;
		
		public float RepathRate = 1f;
		
		public Transform Target;
		
		public bool Wandering;
		private Vector3 angleDir;
		private CharacterController controller;
		private Animator anim;
		public void Awake() {
			controller = GetComponent<CharacterController>();
			anim = GetComponent<Animator>();
			Wandering = true;
		}
		public void Start() {
			if (graph == null) {
				graph = FindObjectsOfType<NodeGraph>().First();
			}
		}
		public void OnEnable() {
			StartCoroutine("Repath");
		}
		public void OnDisable() {
			StopCoroutine("Repath");
		}
		public void SetDestination(Vector3 destination) {
			graph.RequestPath(transform.position, destination, FollowPath);
		}
		public void SetDestination(Transform destination) {
			graph.RequestPath(transform.position, destination.position, FollowPath);
		}
		public void Wander() {
			Wandering = true;
			if (followPathRoutine != null) {
				StopCoroutine(followPathRoutine);
			}
		}
		// RUN F O R E V E R
		IEnumerator Repath() {
			while (true) {
				if(Target != null) {
					SetDestination(Target);
				}
				yield return new WaitForSeconds(RepathRate);
			}
		}
		public IEnumerator followPathRoutine;
		public void FollowPath(Path path, bool success) {
			if (!success)
				return;
			if (followPathRoutine != null) {
				StopCoroutine(followPathRoutine);
			}
			followPathRoutine = _followPath(path);
			StartCoroutine(followPathRoutine);
		}
		IEnumerator _followPath(Path path) {
			Wandering = false;
			int count = path.Count;
			int waypointIndex = 0; // index of next node to go towards
			bool following = true;
			float speedPercent = 1f;
			//Stopwatch giveUp = new Stopwatch();
			float giveUpTimer = 0;
			while (following) {
				float frameSpeedPercent = speedPercent;
				Vector3 adjustedPos = transform.position;
				adjustedPos.y = path[0].Position.y;
				if (Vector3.SqrMagnitude(adjustedPos - path[waypointIndex].Position) < TurnDistance * TurnDistance) {
					if (waypointIndex != path.Count - 1) {
						waypointIndex++;
					}
				}
				if (waypointIndex == path.Count - 1) {
					speedPercent = Mathf.Clamp01(Vector3.Distance(adjustedPos, path[waypointIndex].Position) / StopDistance);
					frameSpeedPercent = speedPercent;
					if (speedPercent < 0.1f) {
						following = false;
						break;
					}
				}
				/*
				RaycastHit[] lookHit;
				if (controller != null) {
					Ray ray = new Ray (controller.center + transform.position + transform.forward * controller.radius, transform.forward);
					if (Physics.SphereCastNonAlloc(ray, controller.radius, lookHit, 1f) > 0) {
						for (int i = 0; i < lookHit.Length; i ++) {
							if (Vector3.AngleBetween (transform.forward, lookHit[i].point - transform.position) > FOVAngle) continue;
						}
					}
				}
				*/
				Vector3 flatLook = path[waypointIndex].Position - transform.position;
				flatLook.y = 0;
				Quaternion targetRot = Quaternion.LookRotation(flatLook.normalized);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, TurnSpeed * Time.deltaTime);
				if (controller != null) {
					controller.SimpleMove(transform.forward * Speed * frameSpeedPercent);
					if (anim != null) {
						anim.SetFloat("Speed", frameSpeedPercent * Mathf.Clamp01(controller.velocity.magnitude));
					}
					if (controller.velocity.magnitude < 0.1f) {
						giveUpTimer += Time.deltaTime;
						if (giveUpTimer >= GiveUpTime) {
							following = false;
							print("Unit gave up");
							break;
						}
					} else {
						giveUpTimer = 0;
					}
				} else {
					transform.Translate(transform.forward * Time.deltaTime * Speed * frameSpeedPercent, Space.World);
				}
				yield return null;
			}
			StopInternal();
		}
		public void Stop() {
			if (followPathRoutine != null) {
				StopCoroutine(followPathRoutine);
			}
			StopInternal();
		}
		private void StopInternal() {
			Target = null;
			if (anim != null) {
				anim.SetFloat("Speed", 0);
			}
			Wandering = false;
			followPathRoutine = null;
		}
	}
}