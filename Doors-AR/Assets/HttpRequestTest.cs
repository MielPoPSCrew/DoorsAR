using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class HttpRequestTest : MonoBehaviour {

	public GameObject roomEventUI;
	public string mock = "[{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-07T06:30:00.000Z\",\"end\":\"2018-05-07T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637362d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-09T06:30:00.000Z\",\"end\":\"2018-05-09T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637372d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T06:30:00.000Z\",\"end\":\"2018-05-14T10:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3430392d302d31\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T11:30:00.000Z\",\"end\":\"2018-05-14T15:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3830332d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"}]";
	public Button btnRequest;

	// Use this for initialization
	void Start () {
		Button btnRequest = GameObject.Find("btnRequest").GetComponent<Button>();
		btnRequest.onClick.AddListener (TaskOnClick);
		//StartCoroutine();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TaskOnClick()
	{
		//Output this to console when the Button is clicked
		Debug.Log("You have clicked the button!");
				StartCoroutine (getRequest ("http://localhost:1234/info/1396?page=0"));
	}

	IEnumerator getRequest(string uri)
	{
		UnityWebRequest uwr = UnityWebRequest.Get(uri);
		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
			Debug.Log("Received: " + uwr.downloadHandler.text);
			//var jsonRoomEvents = JSON.Parse (uwr.downloadHandler.text);
			JSONArray jsonRoomEvents = JSON.Parse(mock).AsArray;
			for (int i = 0; i < jsonRoomEvents.Count; i++) {
				RoomEvent room = RoomEvent.fromJson(jsonRoomEvents[i]);
				Debug.Log("Create: " + i + " --" + room);

				var truc = Instantiate (Resources.Load("RoomEventUI")) as GameObject;

				// FIXME : I'm ugly - Ask the api man to retrieved the room name, even without events. Tks, biz.
				Text txtRoom = GameObject.Find("txtRoom").GetComponent<Text>();
				txtRoom.text = room.Location;
			}



		}
	}
}
