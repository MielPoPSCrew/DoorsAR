using UnityEngine;
using UnityEngine.UI;

public class RoomsAR {

    public int roomId = -1;
    //private HttpRequestTest http = new HttpRequestTest();
    private ScheduleRetriever http;
    private Button btnPrevious;
    private Button btnNext;

    // Use this for initialization
    public RoomsAR(int identifier)
    {
        http = new ScheduleRetriever(identifier);
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

    public void onClose()
    {
        http.Close();
    }
}
