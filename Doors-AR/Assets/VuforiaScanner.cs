using UnityEngine;
using System;
using System.Collections;
using Vuforia;
using ZXing;


[AddComponentMenu("System/VuforiaScanner")]
public class VuforiaScanner : MonoBehaviour
{    
    private bool cameraInitialized;
    private bool scanning;
    private string previousData;

    private RoomsAR roomAR;

	private Image.PIXEL_FORMAT mFormat = Image.PIXEL_FORMAT.GRAYSCALE;

    private BarcodeReader barCodeReader;

    void Start()
    {
        Debug.Log("STAAAAAAAAAAARt");
        barCodeReader = new BarcodeReader();
        StartCoroutine(InitializeCamera());
        scanning = false;
        previousData = "";
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
                    }
                    scanning = true;
                    // TODO init buttons

                }
                else
                {
                    if(scanning)
                    {
                        roomAR.onClose();
                        scanning = false;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }    
}