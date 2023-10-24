using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    public static Login Instance;
    public GameObject PlayerID;
    public GameObject TableID;
    public GameObject Balance;
    public GameObject Tips;
    public GameObject LoginBtn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else if (Instance == null)
            Instance = this;
    }

    public string postJson(string playerID)
    {
        string jsonContent = $"{{\"PlayerID\":\"{playerID}\",\"HomeUrl\":\"https://games.hahaaga.tw/\",\"GameID\":\"chat-m16\",\"ClassID\":300,\"TableID\":null,\"Amount\":0,\"Lang\":\"zh-tw\"}}";
        // Debug.Log("before:" + jsonContent);
        
        string tableID = TableID.GetComponent<TMP_InputField>().text;
        if (tableID != "") jsonContent = jsonContent.Replace("\"TableID\":null", $"\"TableID\":\"{tableID}\"");

        // Debug.Log("after:" + jsonContent);

        return jsonContent;
    }

    public async Task<TXResponse> GetToken(string playerID)
    {
        HttpClient client = new HttpClient();

        var httpContent = new StringContent(postJson(playerID), Encoding.UTF8, "application/json");

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

        TXResponse txResponse = JsonConvert.DeserializeObject<TXResponse>(responseString);

        return txResponse;
    }

    public async void LoginClick()
    {
        string playerID = PlayerID.GetComponent<TMP_InputField>().text;
        if (playerID != null)
        {
            TXResponse txResponse = await GetToken(playerID);

            if (txResponse != null)
            {
                LoginBtn.SetActive(false);
                Balance.GetComponent<TextMeshProUGUI>().text = "Balance:" + txResponse.Balance.ToString();
                Tips.GetComponent<TextMeshProUGUI>().text = "You are logined!!";

                PlayerPrefs.SetString("TXResponseData", JsonConvert.SerializeObject(txResponse));
                PlayerPrefs.Save();
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

[SerializeField]
public class TXResponse
{
    public string Url;
    public int Balance;
    public string Token;
}