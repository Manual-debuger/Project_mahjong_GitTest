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

public class ChatGPTHandler : MonoBehaviour
{
    #nullable enable    
    #nullable disable
    public EventHandler<List<Tuple<string,string>>> OnMessageReceived;
    //public EventHandler<AudioClip> OnAudioClipReceived;
    
    private HttpClient _httpClient;  
    private StreamReader _streamReader;

    // Start is called before the first frame update
    void Start()
    {
        
    }    
    // Update is called once per frame
    void Update()
    {
      
    }      
    public void StartChatGPT(Uri uri)
    {
        EventSource eventSource = new EventSource(uri);
        eventSource.MessageReceived += EventSource_MessageReceived;
    }

    private void EventSource_MessageReceived(object sender, MessageReceivedEventArgs e)
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
}
