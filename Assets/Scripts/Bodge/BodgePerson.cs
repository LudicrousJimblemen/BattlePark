using UnityEngine;
using System.Collections;
using System.Linq;
using Pathfinding;

public class BodgePerson : MonoBehaviour {
	public PathWalker Walker;

	public Color Red;
	public Color Blue;

	[Header("Display")]
	public GameObject Macaroni;
	public GameObject Stetson;
	public GameObject SaltLick;

	private void Awake() {
		Walker = GetComponent<PathWalker>();
	}

	private void Start() {
		if (Application.loadedLevelName == "Intro") {
			StartCoroutine(Intro());
		} else if (Application.loadedLevelName == "PostIntro") {
			StartCoroutine(PostIntro());
		} else if (Application.loadedLevelName == "BuildRollercoaster") {
			StartCoroutine(PostIntro());
		} else if (Application.loadedLevelName == "RideRollercoaster") {
			StartCoroutine(PostIntro());
		} else if (Application.loadedLevelName == "Infuse") {
			StartCoroutine(PostIntro());
		} else if (Application.loadedLevelName == "TwoParks") {
			StartCoroutine(TwoParks());
		} else if (Application.loadedLevelName == "LaunchExplosion") {
			StartCoroutine(TwoParks());
		} else if (Application.loadedLevelName == "LaunchFerriswheel") {
			StartCoroutine(TwoParks());
		} else if (Application.loadedLevelName == "InfuseSalt") {
			StartCoroutine(InfuseSalt());
		} else if (Application.loadedLevelName == "CheesyZoom") {
			StartCoroutine(TwoParks());
		}
	}

	private IEnumerator Intro() {
		GetComponentInChildren<SkinnedMeshRenderer>().material.color = Blue;
		Walker.SetDestination(new Vector3(-12.5f, 0, -6f));
		Walker.Speed = 5f;
		yield return new WaitForSeconds(2.7f);
		GetComponentInChildren<ParticleSystem>().Emit(25);
		yield return new WaitForSeconds(0.4f);
		Macaroni.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		Walker.SetDestination(new Vector3(-8.5f, 0, 6f));
	}

	private IEnumerator PostIntro() {
		GetComponentInChildren<SkinnedMeshRenderer>().material.color = Blue;
		Walker.graph = BodgeManager.Instance.Graphs[0];
		while (true) {
			Walker.DestinationReached = false;
			int random = Random.Range(0, 14);
			if (random == 0 && !Macaroni.activeSelf) {
				if (Random.value < 0.5f) {
					Walker.SetDestination(new Vector3(-11.5f, 0, -4f));
				} else {
					Walker.SetDestination(new Vector3(-30.5f, 0, -6f));
				}

				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.3f);
				Macaroni.SetActive(true);
				GetComponentInChildren<ParticleSystem>().Emit(25);
			} else if (random == 1 && !SaltLick.activeSelf) {
				Walker.SetDestination(new Vector3(-26.5f, 0, -10f));
				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.3f);
				SaltLick.SetActive(true);
				GetComponentInChildren<ParticleSystem>().Emit(25);
			} else if (random == 2 && !Stetson.activeSelf) {
				Walker.SetDestination(new Vector3(-22.5f, 0, -6f));
				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.3f);
				Stetson.SetActive(true);
				GetComponentInChildren<ParticleSystem>().Emit(25);
			} else {
				Walker.SetDestination(Walker.graph.Nodes.ElementAt(Random.Range(0, Walker.graph.Nodes.Count)).Key);
				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.6f);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	private IEnumerator TwoParks() {
		bool side = transform.position.x < 0;

		if (side) {
			GetComponentInChildren<SkinnedMeshRenderer>().material.color = Blue;
		} else {
			GetComponentInChildren<SkinnedMeshRenderer>().material.color = Red;
		}
		while (true) {
			Walker.DestinationReached = false;
			if (side) {
				int random = Random.Range(0, 14);
				if (random == 0 && !Macaroni.activeSelf) {
					if (Random.value < 0.5f) {
						Walker.SetDestination(new Vector3(-11.5f, 0, -4f));
					} else {
						Walker.SetDestination(new Vector3(-30.5f, 0, -6f));
					}

					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.3f);
					Macaroni.SetActive(true);
					GetComponentInChildren<ParticleSystem>().Emit(25);
				} else if (random == 1 && !SaltLick.activeSelf) {
					Walker.SetDestination(new Vector3(-26.5f, 0, -10f));
					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.3f);
					SaltLick.SetActive(true);
					GetComponentInChildren<ParticleSystem>().Emit(25);
				} else if (random == 2 && !Stetson.activeSelf) {
					Walker.SetDestination(new Vector3(-22.5f, 0, -6f));
					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.3f);
					Stetson.SetActive(true);
					GetComponentInChildren<ParticleSystem>().Emit(25);
				} else {
					Walker.SetDestination(Walker.graph.Nodes.ElementAt(Random.Range(0, Walker.graph.Nodes.Count)).Key);
					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.6f);
				}
				yield return new WaitForSeconds(0.5f);
			} else {
				int random = Random.Range(0, 14);
				if (random == 0 && !Macaroni.activeSelf) {
					if (Random.value < 0.5f) {
						Walker.SetDestination(new Vector3(27.5f, 0, -3f));
					} else {
						Walker.SetDestination(new Vector3(27.5f, 0, -6f));
					}

					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.3f);
					Macaroni.SetActive(true);
					GetComponentInChildren<ParticleSystem>().Emit(25);
				} else if (random == 1 && !SaltLick.activeSelf) {
					Walker.SetDestination(new Vector3(11.5f, 0, 1f));
					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.3f);
					SaltLick.SetActive(true);
					GetComponentInChildren<ParticleSystem>().Emit(25);
				} else if (random == 2 && !Stetson.activeSelf) {
					Walker.SetDestination(new Vector3(20.5f, 0, 1f));
					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.3f);
					Stetson.SetActive(true);
					GetComponentInChildren<ParticleSystem>().Emit(25);
				} else {
					Walker.SetDestination(Walker.graph.Nodes.ElementAt(Random.Range(0, Walker.graph.Nodes.Count)).Key);
					yield return new WaitUntil(() => Walker.DestinationReached);
					yield return new WaitForSeconds(0.6f);
				}
				yield return new WaitForSeconds(0.5f);
			}
		}
	}
	
	private IEnumerator InfuseSalt() {
		GetComponentInChildren<SkinnedMeshRenderer>().material.color = Red;
		Walker.graph = BodgeManager.Instance.Graphs[0];
		while (true) {
			Walker.DestinationReached = false;
			int random = Random.Range(0, 14);
			if (random == 0 && !Macaroni.activeSelf) {
				if (Random.value < 0.5f) {
					Walker.SetDestination(new Vector3(-11.5f, 0, -4f));
				} else {
					Walker.SetDestination(new Vector3(-30.5f, 0, -6f));
				}

				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.3f);
				Macaroni.SetActive(true);
				GetComponentInChildren<ParticleSystem>().Emit(25);
			} else if (random == 1 && !SaltLick.activeSelf) {
				Walker.SetDestination(new Vector3(-26.5f, 0, -10f));
				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.3f);
				SaltLick.SetActive(true);
				GetComponentInChildren<ParticleSystem>().Emit(25);
			} else if (random == 2 && !Stetson.activeSelf) {
				Walker.SetDestination(new Vector3(-22.5f, 0, -6f));
				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.3f);
				Stetson.SetActive(true);
				GetComponentInChildren<ParticleSystem>().Emit(25);
			} else {
				Walker.SetDestination(Walker.graph.Nodes.ElementAt(Random.Range(0, Walker.graph.Nodes.Count)).Key);
				yield return new WaitUntil(() => Walker.DestinationReached);
				yield return new WaitForSeconds(0.6f);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}