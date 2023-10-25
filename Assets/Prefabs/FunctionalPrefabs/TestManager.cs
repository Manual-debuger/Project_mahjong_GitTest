using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net.Http;
using TMPro;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EffectController _effectController;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    void Start()
    {
        //var message = (await ChatGPTTool.CallChatGPT("introduce youself")).choices[0].message.content;
        //Debug.Log(message);
        var stream = new HttpClient().GetStreamAsync("https://localhost:7195/api/Test/SSE").Result;
        using (var reader=new StreamReader(stream))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                Debug.Log(line + "\n");
            }
        }
    }

    private void EventSource_EventCommandExecuted(object sender, EventCommandEventArgs e)
    {
        _textMeshPro.text = e.Arguments.Keys.ToString();
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