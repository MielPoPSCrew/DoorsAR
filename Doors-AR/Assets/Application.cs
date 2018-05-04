using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Application : MonoBehaviour {

    public int roomId = -1;
    //private HttpRequestTest http = new HttpRequestTest();
    private ScheduleRetriever http;
    private Button btnPrevious;
    private Button btnNext;

    // Use this for initialization
    void Start () {
        http = new ScheduleRetriever(1396);
        btnPrevious = GameObject.Find("btnPrev").GetComponent<Button>();
        btnPrevious.onClick.AddListener(onPrevious);

        btnNext = GameObject.Find("btnNext").GetComponent<Button>();
        btnNext.onClick.AddListener(onNext);

        //http.RoomId = roomId;
        btnPrevious.enabled = http.HasPrevious;
        btnNext.enabled = http.HasNext;
    }

    void onNext()
    {
        http.Next();
        btnPrevious.enabled = http.HasPrevious;
    }

    void onPrevious()
    {
        http.Previous();
        btnNext.enabled = http.HasNext;
    }
}
