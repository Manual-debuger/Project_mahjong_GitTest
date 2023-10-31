using UnityEngine;
using System;
using System.Net.Http;
using System.Net;
using System.IO;

public class VitsSimpleApi : MonoBehaviour
{
    public AudioSource audioSource;
    private void Start()
    {
        
    }

    public async void OnClick()
    {
        string baseUrl = "http://140.118.216.251:23456/voice/vits";
        string text = "你好,こんにちは";
        int id = 0; //0 ~ 803
        string format = "mp3";
        float length = 2f;
        float noise = 0.33f;
        float noisew = 0.4f;
        int max = 50;

        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "output.mp3");

        var builder = new UriBuilder(baseUrl);
        builder.Query = string.Format("text={0}&id={1}&format={2}&length={3}&noise={4}&noisew={5}&max={6}",
            WebUtility.UrlEncode(text), id, format, length, noise, noisew, max);

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
                    File.WriteAllBytes(savePath, mp3Data);

                    string base64String = Convert.ToBase64String(mp3Data);

                    // Convert the downloaded base64 string to byte array
                    byte[] decodedBytes = Convert.FromBase64String(base64String);

                    // Create an AudioClip from the mp3 data
                    AudioClip audioClip = MP3Transform.MP3ToAudioClip(mp3Data);

                    // Assign the AudioClip to the AudioSource
                    audioSource.clip = audioClip;

                    // Play the audio
                    audioSource.Play();

                    Debug.Log("MP3 download successful and played through AudioSource.");
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