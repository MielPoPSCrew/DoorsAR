using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

//public class HttpRequestTest {

public class ScheduleRetriever
{

    public const string API_URL = "http://localhost:1234/info/";

    private string mock = "[{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-07T06:30:00.000Z\",\"end\":\"2018-05-07T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637362d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-09T06:30:00.000Z\",\"end\":\"2018-05-09T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637372d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T06:30:00.000Z\",\"end\":\"2018-05-14T10:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3430392d302d31\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T11:30:00.000Z\",\"end\":\"2018-05-14T15:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3830332d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"}]";

    private int page = 0; // FIXME: Use that shit // @tcottin : why do that needs a fix? The page should be an attribute that allows us to know where we are
    private int roomId = -1; // FIXME: Use an extenal parameter // @tcottin: fixed - add it as constructor parameter
    private bool hasNext = false;
    private bool hasPrevious = false;
    private List<GameObject> roomEventUIs = new List<GameObject>();
    
    public ScheduleRetriever(int roomId)
    {
        this.roomId = roomId;
        ShowPage();
    }

    // go to the next page and update fields
    public bool Next()
    {
        if(hasNext)
        {
            page++;
            ShowPage();
            hasPrevious = true;
            return true;
        }
        return false;
    }

    // go to the previous page and update fields
    public bool Previous()
    {
        if(hasPrevious)
        {
            page--;
            ShowPage();
            hasNext = true;
            return true;
        }
        return false;
    }
    
	// @tcottin was previously `getRequest()`
    // show the current page of events
    public void ShowPage()
	{
        if(roomId == -1)
        {
            return;
        }

        string response = HTTPQuerier.PerformHTTPQuery(API_URL + roomId + "?page=" + page);

        RectTransform canvaContainer = GameObject.Find("pnlContainer").GetComponent<RectTransform>();
        int y = -140;

        // Clear previous events
        foreach (GameObject item in roomEventUIs)
        {
            GameObject.Destroy(item);
        }
        roomEventUIs.Clear();

		// Parse data as JSON 
		Debug.Log("Received: " + response);
		JSONArray jsonRoomEvents = JSON.Parse(response).AsArray;
        //JSONArray jsonRoomEvents = JSON.Parse(mock).AsArray;



        // Create events view
        for (int i = 0; i < jsonRoomEvents.Count; i++) {
			RoomEvent room = RoomEvent.fromJson(jsonRoomEvents[i]);
			var roomEventUI = GameObject.Instantiate (Resources.Load("RoomEventUI"), canvaContainer.transform.position, canvaContainer.transform.rotation) as GameObject;
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

    /* @tcottin don't need anymore
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
    */

    // tells us wether or not there's a next page
    public bool HasNext
    {
        get
        {
            return this.hasNext;
        }
    }

    // tells us wether or not there's a previous page
    public bool HasPrevious
    {
        get
        {
            return this.hasPrevious;
        }
    }
}
