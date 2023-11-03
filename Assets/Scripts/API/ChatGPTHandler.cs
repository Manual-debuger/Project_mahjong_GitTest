using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using LaunchDarkly;
using LaunchDarkly.EventSource;
using System.Threading.Tasks;

public class ChatGPTHandler : MonoBehaviour
{
    #nullable enable    
    #nullable disable
    //public EventHandler<List<Tuple<string,string>>> OnMessageReceived;
    private EventSource _eventSource;
    public Queue<ParsedVitsResponse> _messageQueue = null;
    //public EventHandler<AudioClip> OnAudioClipReceived;
      

    // Start is called before the first frame update
    void Start()
    {
        
    }    
    // Update is called once per frame
    void Update()
    {
      
    } 
    public int GetCharacterIndex(Uri uri)
    {
        var httpClient = new HttpClient();
        using HttpResponseMessage response = httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync();
        return int.Parse(content.Result);
    }

    public List<int> GetCharacterIndexList(Uri uri)
    {
        var httpClient = new HttpClient();
        using HttpResponseMessage response = httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<int>>(content.Result);
    }
    public async Task StartChatGPT(Uri uri,Queue<ParsedVitsResponse> tuples)
    {
        _messageQueue = tuples;
        _eventSource = new EventSource(uri);
        _eventSource.Opened += (sender, e) =>
        {
            Debug.Log("Connection opened.");
        };
        _eventSource.MessageReceived += (sender, e) =>
        {
            Debug.Log($"{e.EventName}:{e.Message.Data}");
            VitsResponse vitsResponse = JsonConvert.DeserializeObject<VitsResponse>(e.Message.Data);
            ParsedVitsResponse parsedVitsResponse = new ParsedVitsResponse(vitsResponse);            
            lock(_messageQueue)
            {
                _messageQueue.Enqueue(parsedVitsResponse);
            }            
        };
        _eventSource.Error += (sender, e) =>
        {
            Debug.LogWarning("Connection errored. Message: " + e.Exception.Message);
        };
        _eventSource.Closed += (sender, e) =>
        {
            Debug.LogWarning("Connection closed.");
        };
        await _eventSource.StartAsync();
    }
    private void OnDestroy()
    {
        _eventSource?.Dispose();
    }
}
