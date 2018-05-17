using UnityEngine;
using UnityEngine.UI;

public class RoomsAR {

    public int roomId = -1;
    //private HttpRequestTest http = new HttpRequestTest();
    private ScheduleRetriever http;

    // Use this for initialization
    public RoomsAR(int identifier)
    {
        http = new ScheduleRetriever(identifier);
    }

    void onNext()
    {
        http.Next();
    }

    void onPrevious()
    {
        http.Previous();
    }

    public void onClose()
    {
        http.Close();
    }
}
