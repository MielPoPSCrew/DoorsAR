using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ButtonListener : MonoBehaviour, IVirtualButtonEventHandler {

    VirtualButtonBehaviour nextButton;
    VirtualButtonBehaviour previousButton;

    public void Start()
    {
        nextButton = GameObject.Find("NextButton").GetComponent<VirtualButtonBehaviour>();
        nextButton.RegisterEventHandler(this);
        previousButton = GameObject.Find("PreviousButton").GetComponent<VirtualButtonBehaviour>();
        previousButton.RegisterEventHandler(this);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if(vb.name.Equals("NextButton"))
        {
            MyController.Instance.Next();
        }
        else if (vb.name.Equals("PreviousButton"))
        {
            MyController.Instance.Previous();
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
    }
    
}
