using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LobbyServerList : MonoBehaviour {
	public GameObject serverEntryPrefab;

	protected int currentPage;
	protected int previousPage;

	void OnEnable() {
		currentPage = 0;
		previousPage = 0;
		
		for (int i = 1; i < transform.childCount; i++) {
			Destroy(transform.GetChild(i).gameObject);
		}

		RequestPage(0);
	}

	public void OnGUIMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
		if (matches.Count == 0) {
			if (currentPage == 0) {
				GameObject newEntry = (GameObject)Instantiate(serverEntryPrefab);
				newEntry.GetComponent<LobbyServerEntry>().NoneFound();
				newEntry.transform.SetParent(transform, false);
			}
			currentPage = previousPage;
		} else {
			for (int i = 1; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
	
			for (int i = 0; i < matches.Count; ++i) {
				GameObject newEntry = (GameObject)Instantiate(serverEntryPrefab);
				newEntry.GetComponent<LobbyServerEntry>().Populate(matches[i]);
				newEntry.transform.SetParent(transform, false);
			}
		}
		
		for (int i = 0; i < transform.childCount; i++) {
			((RectTransform)transform.GetChild(i).transform).localPosition += new Vector3(
				0,
				(-50 * i) + (25 * (transform.childCount - 1)),
				0);
		}
	}

	public void RequestPage(int page) {
		previousPage = currentPage;
		currentPage = page;
		LobbyManager.Instance.matchMaker.ListMatches(page, 5, "", true, 0, 0, OnGUIMatchList);
	}
}