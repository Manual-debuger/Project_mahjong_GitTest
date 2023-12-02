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
    public Queue<string> _messageQueue = null;
    public Queue<string> _voiceQueue = null;
    private string _lastMessage = "";
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
        Debug.Log($"GetCharacterIndex: uri = {uri}");
        var httpClient = new HttpClient();
        using HttpResponseMessage response = httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync();
        return int.Parse(content.Result);
    }

    public async Task<List<int>> GetCharacterIndexList(Uri uri)
    {
        Debug.Log($"GetCharacterIndexList: uri = {uri}");
        var httpClient = new HttpClient();
        using HttpResponseMessage response = httpClient.GetAsync(uri).Result;
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();        
        Debug.Log(content);
        return JsonConvert.DeserializeObject<List<int>>(content);
    }
    public async Task StartChatGPT(Uri uri,Queue<string> messageQueue)
    {
        _messageQueue = messageQueue;        
        _eventSource = new EventSource(uri);
        _eventSource.Opened += (sender, e) =>
        {
            Debug.Log("Connection opened.");
        };
        _eventSource.MessageReceived += (sender, e) =>
        {
            Debug.Log($"{e.EventName}:{e.Message.Data}");
            if(e.EventName=="message" && e.Message.Data!=null && e.Message.Data!="null" && e.Message.Data.Length>50 && !e.Message.Data.Contains(":HEART BEAT, ignore this")&&e.Message.Data!=_lastMessage)
            {
                _lastMessage=e.Message.Data;
                lock (_messageQueue)
                {
                    _messageQueue.Enqueue(e.Message.Data);
                }            
            }
        };
        _eventSource.Error += (sender, e) =>
        {
            Debug.LogWarning($"Connection errored. Message: {e.Exception.Message}, Source: {e.Exception.Source}, Data:{e.Exception.Data}");
            //InGameUIController.Instance.ShowError($"Connection errored. Message: {e.Exception.Message}, Source: {e.Exception.Source}, Data:{e.Exception.Data}");
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