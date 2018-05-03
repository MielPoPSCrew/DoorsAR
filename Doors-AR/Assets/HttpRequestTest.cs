using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class HttpRequestTest : MonoBehaviour {
    public const string API_URL = "http://localhost:1234/info/";

    private string mock = "[{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-07T06:30:00.000Z\",\"end\":\"2018-05-07T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637362d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-09T06:30:00.000Z\",\"end\":\"2018-05-09T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637372d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T06:30:00.000Z\",\"end\":\"2018-05-14T10:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3430392d302d31\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T11:30:00.000Z\",\"end\":\"2018-05-14T15:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3830332d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"}]";

    private int page = 0; // FIXME: Use that shit
    private int roomId = -1; // FIXME: Use an extenal parameter
    private bool hasNext = false;
    private bool hasPrevious = false;
    private List<GameObject> roomEventUIs = new List<GameObject>();
    
    private void onRequestApi()
	{
		StartCoroutine (getRequest ());
	}

    public void next()
    {
        page++;
        Debug.Log("Requestion next page " + (page));
        StartCoroutine(getRequest());
    }

    public void previous()
    {
        if(page - 1 >= 0)
        {
            --page;
            Debug.Log("Requestion previous page " + (page));
            StartCoroutine(getRequest());
        }


        Debug.Log("No request if page < 0");
    }

	IEnumerator getRequest()
	{
        if(roomId == -1)
        {
            yield return "";
        }

		UnityWebRequest uwr = UnityWebRequest.Get(API_URL + roomId + "?page=" + page);
        yield return uwr.SendWebRequest();

		if (uwr.isNetworkError)
		{
			Debug.Log("Error While Sending: " + uwr.error);
		}
		else
		{
            RectTransform canvaContainer = GameObject.Find("pnlContainer").GetComponent<RectTransform>();
            int y = -140;

            // Clear previous events
            foreach (GameObject item in roomEventUIs)
            {
                Destroy(item);
            }
            roomEventUIs.Clear();

			// Parse data as JSON 
			Debug.Log("Received: " + uwr.downloadHandler.text);
			JSONArray jsonRoomEvents = JSON.Parse(uwr.downloadHandler.text).AsArray;
            //JSONArray jsonRoomEvents = JSON.Parse(mock).AsArray;



            // Create events view
            for (int i = 0; i < jsonRoomEvents.Count; i++) {
				RoomEvent room = RoomEvent.fromJson(jsonRoomEvents[i]);
				var roomEventUI = Instantiate (Resources.Load("RoomEventUI"), canvaContainer.transform.position, canvaContainer.transform.rotation) as GameObject;
                roomEventUIs.Add(roomEventUI);

				// Place item
				roomEventUI.transform.SetParent (canvaContainer);
				roomEventUI.transform.position = new Vector3 (roomEventUI.transform.position.x, y, roomEventUI.transform.position.z);
				y = y + 110;

				// Set width
				RectTransform rt = (RectTransform)roomEventUI.transform;
				rt.sizeDelta = new Vector2 (canvaContainer.rect.width - 50, rt.rect.height);
		
				Text[] texts = roomEventUI.GetComponentsInChildren<Text>();
				setupTexts (texts, room, canvaContainer);

				// Set pane title
				// FIXME : I'm ugly - Ask the api man to retrieved the room name, even without events. Tks, biz.
				Text txtRoom = GameObject.Find("txtRoom").GetComponent<Text>();
				txtRoom.text = room.Location; 
			}
		}
	}

	void setupTexts(Text[] texts, RoomEvent eventRoom, RectTransform canvaContainer) {

        foreach (Text aText in texts) {
            RectTransform rtText = (RectTransform) aText.transform;
            rtText.sizeDelta = new Vector2(canvaContainer.rect.width - 100, rtText.rect.height);

            if (aText.name.Equals("txtDate"))
            {
                aText.text = eventRoom.FromDate.ToString() + " - " + eventRoom.ToDate;
                
            }
            else if(aText.name.Equals("txtSummary"))
            {
                aText.text = eventRoom.Summary;
            }
            else if (aText.name.Equals("txtDescription"))
            {

                aText.text = eventRoom.Description.Trim();
                rtText.sizeDelta = new Vector2(rtText.rect.width, 50);
            }
        }
	}

    public int RoomId
    {
        get
        {
            return this.roomId;
        }
        set
        {
            this.roomId = value;
            this.page = 0;
            onRequestApi();
        }
    }

    public bool HasNext
    {
        get
        {
            return this.hasNext;
        }
    }

    public bool HasPrevious
    {
        get
        {
            return this.hasPrevious;
        }
    }
}
