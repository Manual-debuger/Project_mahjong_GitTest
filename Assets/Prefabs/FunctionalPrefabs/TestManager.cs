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
    void Start()
    {
        //var message = (await ChatGPTTool.CallChatGPT("introduce youself")).choices[0].message.content;
        //Debug.Log(message);

        EventSource eventSource = new EventSource(new Uri("http://localhost:5000/"));
        eventSource.MessageReceived += EventSource_MessageReceived;
    }

    private void EventSource_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.LogWarning(e.Message);
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