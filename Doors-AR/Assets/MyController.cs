using System;

public class MyController
{
    private static readonly MyController instance = new MyController();

    private static ScheduleRetriever retriever;

    static MyController() { }

    private MyController() { }

    public static MyController Instance
    {
        get
        {
            return instance;
        }
    }

    public void Next()
    {
        if(retriever != null)
            retriever.Next();
    }

    public void Previous()
    {
        if (retriever != null)
            retriever.Previous();
    }

    public ScheduleRetriever Retriever {
        set 
        {
            retriever = value;
        }
    }

}
