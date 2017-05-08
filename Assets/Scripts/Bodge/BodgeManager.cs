using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class BodgeManager : MonoBehaviour {
	public static BodgeManager Instance;
	public Vector3[] ParkCenters;
	public Vector3[] ParkGates;
	public NodeGraph[] Graphs;

	public GameObject[] PlayerObjectParents = new GameObject[2];
	public GameObject PersonObj;

	public GridObject[] Objects;
	public List<Person> Guests;

	[Header("BuildRollercoaster")]
	public GameObject Coaster;

	[Header("Infuse")]
	public Text Text;
	public GameObject Macaroni;
	public GameObject Dynamite;
	public GameObject Dynamacaroni;

	[Header("LaunchExplosion")]
	public GameObject RidingPerson;
	public GameObject LaunchedPerson;
	public ParticleSystem Exploderino;

	[Header("LaunchFerriswheel")]
	public GameObject BodgeWheelPerson;
	public GameObject Wheel;

	private void Awake() {
		if (FindObjectsOfType<BodgeManager>().Length > 1) {
			Destroy(gameObject);
			return;
		}
		Instance = this;

		if (Application.loadedLevelName == "PostIntro") {
			StartCoroutine(PostIntro());
		}
		if (Application.loadedLevelName == "BuildRollercoaster") {
			StartCoroutine(BuildRollercoaster());
		}
		if (Application.loadedLevelName == "RideRollercoaster") {
			StartCoroutine(PostIntro());
		}
		if (Application.loadedLevelName == "Infuse") {
			StartCoroutine(Infuse());
		}
		if (Application.loadedLevelName == "InfuseSalt") {
			StartCoroutine(Infuse());
		}
		if (Application.loadedLevelName == "TwoParks") {
			StartCoroutine(TwoParks());
		}
		if (Application.loadedLevelName == "LaunchExplosion") {
			StartCoroutine(LaunchExplosion());
		}
		if (Application.loadedLevelName == "LaunchFerriswheel") {
			StartCoroutine(LaunchFerriswheel());
			StartCoroutine(ResetWheel());
		}
		if (Application.loadedLevelName == "CheesyZoom") {
			StartCoroutine(TwoParks());
		}
	}

	private IEnumerator PostIntro() {
		for (int i = 0; i < 90; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity);
			yield return new WaitForSeconds(0.05f);
		}
	}

	private IEnumerator BuildRollercoaster() {
		List<GameObject> coasterParts = new List<GameObject>();
		for (int i = 0; i < Coaster.transform.childCount; i++) {
			coasterParts.Add(Coaster.transform.GetChild(i).gameObject);
		}

		foreach (var part in coasterParts) {
			part.SetActive(false);
		}

		for (int i = 0; i < 90; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity);
		}

		yield return new WaitForSeconds(5f);

		foreach (var part in coasterParts) {
			part.SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}
	}

	private IEnumerator Infuse() {
		Dynamacaroni.SetActive(false);
		for (int i = 0; i < 50; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity);
			yield return new WaitForSeconds(0.05f);
		}

		yield return new WaitForSeconds(12f);
		Dynamacaroni.SetActive(true);
		yield return new WaitForSeconds(0.4f);
		Text.text = "Infused!";
		Dynamite.SetActive(false);
		Macaroni.SetActive(false);
	}

	private IEnumerator InfuseSalt() {
		Dynamacaroni.SetActive(false);
		for (int i = 0; i < 50; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity);
			yield return new WaitForSeconds(0.05f);
		}

		yield return new WaitForSeconds(12f);
		Dynamacaroni.SetActive(true);
		yield return new WaitForSeconds(0.4f);
		Text.text = "Infused!";
		Dynamite.SetActive(false);
		Macaroni.SetActive(false);
	}

	private IEnumerator TwoParks() {
		for (int i = 0; i < 70; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity).GetComponent<BodgePerson>().Walker.graph = Graphs[0];
		}
		for (int i = 0; i < 50; i++) {
			Instantiate(PersonObj, new Vector3(4.85f, 0, 0), Quaternion.identity).GetComponent<BodgePerson>().Walker.graph = Graphs[1];
		}
		yield return null;
	}

	private IEnumerator LaunchExplosion() {
		LaunchedPerson.SetActive(false);
		for (int i = 0; i < 60; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity).GetComponent<BodgePerson>().Walker.graph = Graphs[0];
		}
		for (int i = 0; i < 60; i++) {
			Instantiate(PersonObj, new Vector3(4.85f, 0, 0), Quaternion.identity).GetComponent<BodgePerson>().Walker.graph = Graphs[1];
		}
		yield return new WaitForSeconds(18.5f - (1f / 60f));
		RidingPerson.SetActive(false);
		LaunchedPerson.SetActive(true);
		LaunchedPerson.GetComponent<Rigidbody>().AddForce(new Vector3(-12f, 12f, 0f), ForceMode.VelocityChange);
		LaunchedPerson.GetComponent<Rigidbody>().AddTorque(new Vector3(-2f, 2f, 0f), ForceMode.VelocityChange);
		yield return new WaitForSeconds(2.631f);
		Exploderino.Play();
	}

	private IEnumerator LaunchFerriswheel() {
		for (int i = 0; i < 60; i++) {
			Instantiate(PersonObj, new Vector3(-4.85f, 0, 0), Quaternion.identity).GetComponent<BodgePerson>().Walker.graph = Graphs[0];
		}
		for (int i = 0; i < 60; i++) {
			Instantiate(PersonObj, new Vector3(4.85f, 0, 0), Quaternion.identity).GetComponent<BodgePerson>().Walker.graph = Graphs[1];
		}
		
		Color blue = FindObjectOfType<BodgePerson>().Blue;
		List<GameObject> persons = new List<GameObject>();
		for (int i = 0; i < 6; i++) {
			GameObject newDude = Instantiate(BodgeWheelPerson, Vector3.zero, Quaternion.identity);
			newDude.transform.localRotation = Quaternion.Euler(0, 90, 90);
			newDude.transform.localPosition = new Vector3(0, 0, -2f);
			newDude.transform.SetParent(Wheel.transform.GetChild(i).transform, false);
			newDude.GetComponentInChildren<SkinnedMeshRenderer>().material.color = blue;
			persons.Add(newDude);
		}
		persons.Reverse();
		
		yield return new WaitForSeconds(13.18f);
		foreach (var person in persons) {
			person.transform.parent = null;
			person.GetComponent<Rigidbody>().isKinematic = false;
			person.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(15f, 18f), Random.Range(3f, 7f), Random.Range(-2f, 2f)), ForceMode.VelocityChange);
			person.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Random.Range(-15f, 15f), Random.Range(-4f, 4f), Random.Range(-4f, 4f)), ForceMode.VelocityChange);
			yield return new WaitForSeconds(0.09f);
		}
		
		yield return null;
	}
	private IEnumerator ResetWheel() {
		while (true) {
			for (int i = 0; i < Wheel.transform.childCount; i++) {
				Wheel.transform.GetChild(i).transform.rotation = Quaternion.Euler(-90, 0, 0);
			}
			yield return null;
		}
	}

	private void OnGUI() {
		if (Application.isEditor) {
			GUI.Label(new Rect(12f, 12f, 200f, 200f), Time.timeSinceLevelLoad.ToString());
		}
	}
}
