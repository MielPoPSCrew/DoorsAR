﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

    public int roomId = -1;
    private HttpRequestTest http = new HttpRequestTest();
    private Button btnPrevious;
    private Button btnNext;

    // Use this for initialization
    void Start () {
        btnPrevious = GameObject.Find("btnPrev").GetComponent<Button>();
        btnPrevious.onClick.AddListener(onPrevious);

        btnNext = GameObject.Find("btnNext").GetComponent<Button>();
        btnNext.onClick.AddListener(onNext);

        http.RoomId = roomId;
        btnPrevious.enabled = http.HasPrevious;
        btnNext.enabled = http.HasNext;
    }

    void onNext()
    {
        http.next();
        btnPrevious.enabled = http.HasPrevious;
    }

    void onPrevious()
    {
        http.previous();
        btnNext.enabled = http.HasNext;
    }
}