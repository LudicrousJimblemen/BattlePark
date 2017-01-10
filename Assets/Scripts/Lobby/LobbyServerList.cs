using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

public class LobbyServerList : MonoBehaviour {
	public LobbyManager lobbyManager;

	public RectTransform serverListRect;
	public GameObject serverEntryPrefab;
	public GameObject noServerFound;

	protected int currentPage;
	protected int previousPage;

	void OnEnable() {
		currentPage = 0;
		previousPage = 0;

		foreach (Transform t in serverListRect)
			Destroy(t.gameObject);

		noServerFound.SetActive(false);

		RequestPage(0);
	}

	public void OnGUIMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
		if (matches.Count == 0) {
			if (currentPage == 0) {
				//SUMMON A RAD BUTTON WITH NO SERVER FOUND AND IT'S ALSO NONINTERACTABLE
			}
			currentPage = previousPage;
		} else {
			foreach (Transform yourself in serverListRect) {
				Fire(yourself);
			}
	
			for (int i = 0; i < matches.Count; ++i) {
				GameObject newEntry = (GameObject)Instantiate(serverEntryPrefab);
	
				newEntry.GetComponent<LobbyServerEntry>().Populate(matches[i], lobbyManager);
	
				newEntry.transform.SetParent(serverListRect, false);
			}
		}
	}

	public void ChangePage(int dir) {
		int newPage = Mathf.Max(0, currentPage + dir);

		if (noServerFound.activeSelf) {
			newPage = 0;
		}
		
		RequestPage(newPage);
	}

	public void RequestPage(int page) {
		previousPage = currentPage;
		currentPage = page;
		lobbyManager.matchMaker.ListMatches(page, 6, "", true, 0, 0, OnGUIMatchList);
	}
	
	private void Fire(Transform yourself) {
		Destroy(yourself.gameObject);
	}
}