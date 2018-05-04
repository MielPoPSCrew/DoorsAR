using UnityEngine;
using UnityEngine.Networking;

/**
 * Performs a HTTP query
 **/
public static class HTTPQuerier
{
    public static string PerformHTTPQuery(string url)
    {

        // unity's web request processor
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        // run request
        webRequest.SendWebRequest();

        // while our request is not over
        while (webRequest.downloadProgress < 1)
        {
            // it can fail
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError(webRequest.error);
                return "";
            }
        }

        // it still can fail
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            // yeah! it works!
            return webRequest.downloadHandler.text;
        }
        return "";

    }
}
