using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics.Tracing;
using System.IO;
using System.Net;
using System.Net.Http;
using TMPro;
using UnityEngine;
using LaunchDarkly.EventSource;


public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EffectController _effectController;
    [SerializeField] private TextMeshProUGUI _textMeshPro;    
#nullable enable    
#nullable disable
    void Start()
    {
        //var message = (await ChatGPTTool.CallChatGPT("introduce youself")).choices[0].message.content;
        //Debug.Log(message);

        //_eventSource = new EventSource(new Uri("https://localhost:7195/api/Test/ChatGPT?tableID=321"));
        

    }

    public async void StartSSE()
    {
        EventSource _eventSource;
        _eventSource = new EventSource(new Uri("https://localhost:7195/api/Test/SSE"));
        _eventSource.Opened += (sender, e) =>
        {
            Debug.Log("Connection opened.");
        };
        _eventSource.MessageReceived += (sender, e) => {
            Debug.Log($"{e.EventName}:{e.Message.Data}");
           // _textMeshPro.text = e.Message.Data.ToString();
        };
        _eventSource.Error += (sender, e) =>
        {
            Debug.LogWarning("My Connection errored. Message: " + e.Exception.Message);
        };
        _eventSource.Closed += (sender, e) => {
            Debug.LogWarning("My Connection closed. Status: ");
        };
        await _eventSource.StartAsync();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffect(string effectID)
    {
         //_effectController.PlayEffect((EffectID)int.Parse(effectID), 0);
    }
}