using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Net;

public class VitsSimpleApi : MonoBehaviour
{
    private async void Start()
    {
        string baseUrl = "http://140.118.216.251:23456/voice/vits";
        string text = "§A¦n";
        int id = 0; //0 ~ 803
        string format = "mp3";
        float noise = 0.33f;
        float noisew = 0.4f;
        int max = 50;

        //string savePath = System.IO.Path.Combine(Application.persistentDataPath, "output.mp3");

        var builder = new UriBuilder(baseUrl);
        builder.Query = string.Format("text={0}&id={1}&format={2}&noise={3}&noisew={4}&max={5}",
            WebUtility.UrlEncode(text), id, format, noise, noisew, max);

        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Send a GET request and receive a response
                HttpResponseMessage response = await client.GetAsync(builder.Uri);

                // Check if the MP3 response was successfully received

                if (response.IsSuccessStatusCode)
                {
                    byte[] mp3Data = await response.Content.ReadAsByteArrayAsync();
                    //File.WriteAllBytes(savePath, mp3Data);

                    string base64String = Convert.ToBase64String(mp3Data);

                    Debug.Log("MP3 download successful and converted to Base64 string.");
                    //Debug.Log(base64String);
                }
                else
                {
                    Debug.LogError("Failed to download MP3. HTTP error code:" + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while downloading MP3:" + e.Message);
        }
    }
}