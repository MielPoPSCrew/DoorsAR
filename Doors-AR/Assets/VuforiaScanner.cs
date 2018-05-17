using UnityEngine;
using System;
using System.Collections;
using Vuforia;
using ZXing;


[AddComponentMenu("System/VuforiaScanner")]
public class VuforiaScanner : MonoBehaviour, ITrackableEventHandler
{    
    private bool cameraInitialized;
    private bool scanning;
    private string previousData;

    private RoomsAR roomAR;

	private Image.PIXEL_FORMAT mFormat = Image.PIXEL_FORMAT.GRAYSCALE;

    private BarcodeReader barCodeReader;

    void Start()
    {
        Debug.Log("STAAAAAAAAAAART");
        barCodeReader = new BarcodeReader();
        StartCoroutine(InitializeCamera());
        scanning = false;
        previousData = "";
        TrackableBehaviour target = GameObject.Find("Target").GetComponent<TrackableBehaviour>();
        target.RegisterTrackableEventHandler(this);
    }

    private IEnumerator InitializeCamera()
    {
        yield return new WaitForSeconds(1.25f);

        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(mFormat, true);
        Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));
        
        var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
        Debug.Log(String.Format("AutoFocus : {0}", isAutoFocus));
        cameraInitialized = true;
    }

    private void Update()
    {

        if (cameraInitialized)
        {
            try
            {
                var cameraFeed = CameraDevice.Instance.GetCameraImage(mFormat);
                if (cameraFeed == null)
                {
                    //throw new Exception("Camera feed doesn't work");
                    return;
                }
				var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.Gray8);
                if (data != null)
                {
                    if(!scanning && !previousData.Equals(data.Text))
                    {
                        Debug.LogError(data.Text);
                        previousData = data.Text;
                        roomAR = new RoomsAR(int.Parse(data.Text));
                        Debug.Log("After");
                    }
                    scanning = true;

                }
                else
                {
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                //Environment.Exit(1);
            }
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus.Equals(TrackableBehaviour.Status.NOT_FOUND) && !previousStatus.Equals(TrackableBehaviour.Status.NOT_FOUND))
        {
            if(scanning)
            {
                scanning = false;
                Debug.Log("Lost track");
                if(roomAR != null) roomAR.onClose();
                previousData = "";
            }
        }
    }
}