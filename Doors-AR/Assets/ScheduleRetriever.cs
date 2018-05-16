using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using Vuforia;

//public class HttpRequestTest {

public class ScheduleRetriever
{

    public const string API_URL = "http://vr.tcottin.fr/info/";

    private string mock = "{data:[{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-07T06:30:00.000Z\",\"end\":\"2018-05-07T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637362d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-09T06:30:00.000Z\",\"end\":\"2018-05-09T10:30:00.000Z\",\"summary\":\"Informatique d'entreprise et cloud\",\"location\":\"620 - B009\",\"description\":\"\\n\\nAPP5 INFO\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d343637372d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T06:30:00.000Z\",\"end\":\"2018-05-14T10:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3430392d302d31\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"},{\"type\":\"VEVENT\",\"params\":[],\"dtstamp\":\"20180502T140333Z\",\"start\":\"2018-05-14T11:30:00.000Z\",\"end\":\"2018-05-14T15:30:00.000Z\",\"summary\":\"Systèmes électromécaniques industriels\",\"location\":\"620 - B009\",\"description\":\"\\n\\nMOSTAFA Ali\\nDIALLO Demba\\nAPP3 EES\\n(Exporté le:02/05/2018 16:03)\\n\",\"uid\":\"ADE60323031372d323031382d3830332d302d30\",\"created\":\"19700101T000000Z\",\"last-modified\":\"20180502T140333Z\",\"sequence\":\"1812257813\"}], hasNext=\"false\"";

    private int page = 0; // FIXME: Use that shit // @tcottin : why do that needs a fix? The page should be an attribute that allows us to know where we are
    private int roomId = -1; // FIXME: Use an extenal parameter // @tcottin: fixed - add it as constructor parameter
    private bool hasNext = false;
    private bool hasPrevious = false;
    private List<GameObject> roomEventUIs = new List<GameObject>();

    private GameObject previousPlane = GameObject.Find("PreviousButtonPlane");
    private GameObject nextPlane = GameObject.Find("NextButtonPlane");

    private GameObject canevas;

    public ScheduleRetriever(int roomId)
    {
        MyController.Instance.Retriever = this;
        Debug.Log("Run");
        this.roomId = roomId;
        this.roomId = 1604;
        canevas = GameObject.Instantiate(Resources.Load("Canevas")) as GameObject;
        canevas.transform.rotation = new Quaternion(90, 0, 0, 0);
        ShowPage();

    }

    // go to the next page and update fields
    public bool Next()
    {
        if(hasNext)
        {
            page++;
            hasPrevious = true;
            ShowPage();
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
            if(page == 0)
                hasPrevious = false;
            hasNext = true;
            ShowPage();
            return true;
        }
        return false;
    }

    // manage the plane visibility (is the button available?)
    private void manageVisibility()
    {
        previousPlane.SetActive(hasPrevious);
        nextPlane.SetActive(hasNext);
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

        //Transform target = GameObject.Find("Target").GetComponent<Transform>();

        //var canevaTemp = GameObject.Instantiate(Resources.Load("Canevas"), Vector3.zero, Quaternion.identity) as GameObject;


        RectTransform canevasContainer = canevas.GetComponent<RectTransform>();
        //canevasContainer.SetParent(target);


        //canevasContainer.anchoredPosition = new Vector3(0, 0, 0);
        //canevasContainer.anchoredPosition = target.transform.position;

        int y = 160;

        // Clear previous events
        foreach (GameObject item in roomEventUIs)
        {
            GameObject.Destroy(item);
        }
        roomEventUIs.Clear();

		// Parse data as JSON 
		Debug.Log("Received: " + response);
        JSONNode jsonObject = JSON.Parse(response);
        JSONArray jsonRoomEvents = jsonObject["data"].AsArray;
        hasNext = jsonObject["hasNext"].AsBool;

        manageVisibility();

        // Create events view
        for (int i = 0; i < jsonRoomEvents.Count; i++) {
			RoomEvent room = RoomEvent.fromJson(jsonRoomEvents[i]);
			var roomEventUI = GameObject.Instantiate (Resources.Load("RoomEventUI"), Vector3.zero, Quaternion.identity) as GameObject;
            roomEventUIs.Add(roomEventUI);
            roomEventUI.name = i + "";

            // Place item
            roomEventUI.transform.SetParent(canevasContainer, false);
            roomEventUI.transform.localScale = new Vector3(1, 1, 1);
            roomEventUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, y, 0);
            y = y - 110;

			// Set width
			RectTransform rt = (RectTransform)roomEventUI.transform;
			rt.sizeDelta = new Vector2 (canevasContainer.rect.width - 50, rt.rect.height);
		
			Text[] texts = roomEventUI.GetComponentsInChildren<Text>();
			setupTexts (texts, room, canevasContainer);

			// Set pane title
			// FIXME : I'm ugly - Ask the api man to retrieved the room name, even without events. Tks, biz.
			Text txtRoom = GameObject.Find("txtRoom").GetComponent<Text>();
			txtRoom.text = room.Location; 
		}
	}

	void setupTexts(Text[] texts, RoomEvent eventRoom, RectTransform canevasContainer) {

        foreach (Text aText in texts) {
            RectTransform rtText = (RectTransform) aText.transform;
            rtText.sizeDelta = new Vector2(canevasContainer.rect.width - 100, rtText.rect.height);

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

    public void Close()
    {
        GameObject canevas = GameObject.Find("Canevas");
        GameObject.Destroy(canevas);
    }
}
