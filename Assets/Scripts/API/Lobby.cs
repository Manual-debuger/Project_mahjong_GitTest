using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

public class Lobby : MonoBehaviour
{
    public static Lobby Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else if (Instance == null)
            Instance = this;
    }

    public async Task<TXResponse> GetToken()
    {
        HttpClient client = new HttpClient();

        string jsonContent = "{\"PlayerID\":\"test-user\",\"HomeUrl\":\"https://games.hahaaga.tw/\",\"GameID\":\"chat-m16\",\"ClassID\":300,\"TableID\":null,\"Amount\":0,\"Lang\":\"zh-tw\"}";

        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://games.hahaaga.tw/api/v1/api/login", httpContent);

        string responseString = "";
        
        if (response.IsSuccessStatusCode)
        {
            responseString = await response.Content.ReadAsStringAsync();
        }
        else
        {
            responseString = "Http request failed, ErrorCode:" + response.StatusCode;
        }
        Debug.Log("From hahaages:" + responseString);

        TXResponse tXResponse = JsonConvert.DeserializeObject<TXResponse>(responseString);

        return tXResponse;
    }
}

[SerializeField]
public class TXResponse
{
    public string Url;
    public int Banlance;
    public string Token;
}
