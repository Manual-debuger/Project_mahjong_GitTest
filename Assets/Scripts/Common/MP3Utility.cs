using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MP3Sharp;

public static class MP3Transform
{
    public static AudioClip MP3ToAudioClip(byte[] mp3Data)
    {
        using (MP3Stream mp3Stream = new MP3Stream(new System.IO.MemoryStream(mp3Data)))
        {
            List<float> samplesList = new List<float>();

            byte[] buffer = new byte[4096];
            int bytesRead = 0;

            while ((bytesRead = mp3Stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead / 2; i += 2)
                {
                    short sample = (short)((buffer[i + 1] << 8) | buffer[i]);
                    float floatSample = sample / 32768.0f;
                    samplesList.Add(floatSample);
                }
            }

            float[] samples = samplesList.ToArray();

            int channels = 1;
            int sampleRate = 44100; // mp3Stream.Frequency * 2

            AudioClip audioClip = AudioClip.Create("AudioClip", samples.Length, channels, sampleRate, false);
            audioClip.SetData(samples, 0);

            return audioClip;
        }
    }

    /*public static AudioClip GetAudioClipFromMP3ByteArray(byte[] mp3Data)
    {
        var mp3MemoryStream = new
MemoryStream(mp3Data);
        MP3Sharp.MP3Stream mp3Stream = new MP3Sharp.MP3Stream(mp3MemoryStream);

        //Get the converted stream data
        MemoryStream convertedAudioStream = new MemoryStream();
        byte[] buffer = new byte[2048];
        int bytesReturned = -1;
        int totalBytesReturned = 0;

        while (bytesReturned != 0)
        {
            bytesReturned = mp3Stream.Read(buffer, 0, buffer.Length);
            convertedAudioStream.Write(buffer, 0, bytesReturned);
            totalBytesReturned += bytesReturned;
        }

        Debug.Log("MP3 file has " + mp3Stream.ChannelCount + " channels with a frequency of " +
                  mp3Stream.Frequency);

        byte[] convertedAudioData = convertedAudioStream.ToArray();

        //bug of mp3sharp that audio with 1 channel has right channel data, to skip them
        byte[] data = new byte[convertedAudioData.Length / 2];
        for (int i = 0; i < data.Length; i += 2)
        {
            data[i] = convertedAudioData[2 * i];
            data[i + 1] = convertedAudioData[2 * i + 1];
        }

        Wav wav = new Wav(data, mp3Stream.ChannelCount, mp3Stream.Frequency);

        AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
        audioClip.SetData(wav.LeftChannel, 0);

        return audioClip;
    }*/
}