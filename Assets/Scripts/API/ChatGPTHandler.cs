using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ChatGPTHandler : MonoBehaviour
{
    #nullable enable
    public Uri? uri=null;
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
        if(uri!=null)
        {
            _httpClient = new HttpClient();
            
            string context = "";
            using (_streamReader = new StreamReader(_httpClient.GetStreamAsync(uri).Result))
            {
                while (!_streamReader.EndOfStream)
                {
                    var line = _streamReader.ReadLine();
                    context+= line + "\n";
                }
                if (context.Contains("message"))
                {
                    var vitsResponse = JsonConvert.DeserializeObject<VitsResponse>(context);
                    try
                    {
                        var result = ChatGPTTool.Parsing(vitsResponse.message);
                        OnMessageReceived?.Invoke(this, result);
                        //_inGameUIController.AddChat(result);
                    }
                    catch
                    {
                        Debug.LogWarning("ChatGPTTool.Parsing failed");
                    }
                }
            }            
        }
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
