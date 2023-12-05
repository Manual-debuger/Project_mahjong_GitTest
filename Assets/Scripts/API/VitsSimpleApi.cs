using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Net.Http;
using System.Net;
using System;
using System.Collections;

public class VitsSimpleApi : MonoBehaviour
{
    private AudioSource audioSource;
    private string savePath;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        savePath = string.Format("{0}/{1}.{2}", Application.persistentDataPath, DateTime.UtcNow.ToString("yyMMdd-HHmmss-fff"), "wav");
    }

    public async void OnClick()
    {
        string baseUrl = "http://140.118.216.251:23456/voice/vits";
        string text = "你好,こんにちは";
        int id = 0; // 0 ~ 803
        string format = "mp3";
        float length = 2f;
        float noise = 0.33f;
        float noisew = 0.4f;
        int max = 50;

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

                    // Load the AudioClip from the saved path
                    StartCoroutine(LoadAudioClipAndPlay());
                }
                else
                {
                    Debug.LogError("Failed to download MP3. HTTP error code: " + response.StatusCode);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred while downloading MP3: " + e.Message);
        }
    }

    private IEnumerator LoadAudioClipAndPlay()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + savePath, UnityEngine.AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = audioClip;
                audioSource.Play();

                // Wait until the audio finishes playing
                yield return new WaitForSeconds(audioClip.length);

                // Delete the saved MP3 file after playing
                File.Delete(savePath);

                Debug.Log("MP3 downloaded, played, and file deleted.");
            }
            else
            {
                Debug.LogError("Failed to load MP3: " + www.error);
            }
        }
    }
}
