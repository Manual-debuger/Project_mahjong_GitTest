using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public partial class ChatGPTTool
{
    public async static Task<Result> CallChatGPT(string msg)
    {
        HttpClient client = new HttpClient();
        string uri = "https://api.openai.com/v1/chat/completions";

        // Request headers.
        client.DefaultRequestHeaders.Add(
            "Authorization", "Bearer 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");

        var JsonString = $@"
        {{
            ""model"": ""gpt-3.5-turbo"",
            ""messages"": [{{""role"": ""user"", ""content"":""{msg}"" }}]
        }}";
        
        var content = new StringContent(JsonString, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(uri, content);
        var JSON = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<Result>(JSON);
    }
}

//[System.Serializable]
public class Choice
{
    public int index { get; set; }
    public Message message { get; set; }
    public string finish_reason { get; set; }
}

//[System.Serializable]
public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

//[System.Serializable]
public class Result
{
    public string id { get; set; }
    public string @object { get; set; }
    public int created { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}

//[System.Serializable]
public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}