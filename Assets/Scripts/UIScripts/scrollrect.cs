using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class scrollrect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textView;
    [SerializeField]
    private ScrollRect scrollControl;
    public List<Tuple<string, string>> linesToDisplay = new();
    public string PlayerName;
    public List<char> TextToDisplay = new();
    public float timeFactor = 0.9f;
    private bool isDisplaying = false;

    private void Start()
    {
    }

    IEnumerator DisplayTextCoroutine()
    {
        isDisplaying = true;

        while (linesToDisplay.Count != 0)
        {
            AddChatToText(linesToDisplay[0]);
            linesToDisplay.RemoveAt(0);
            yield return new WaitForSeconds(2);
        }
        isDisplaying = false;
    }

    IEnumerator DisplayTextbyCharCoroutine(float time)
    {
        isDisplaying = true;
        string newcontent = "";
        newcontent += "<color=#2491aa>";
        newcontent += PlayerName;
        newcontent += "</color>";
        newcontent += ":";
        newcontent += "\n";
        textView.text += newcontent;

        while (TextToDisplay.Count != 0)
        {
            textView.text = textView.text + TextToDisplay[0];
            TextToDisplay.RemoveAt(0);
            yield return new WaitForSeconds(time);
        }
        textView.text = textView.text + "\n";
        //StartCoroutine("ScrollToBottom");
        isDisplaying = false;
    }
    public void AddChat(List<Tuple<string, string>> text)
    {
        for (int i = 0; i < text.Count; i++)
        {
            linesToDisplay.Add(text[i]);
        }
        if (!isDisplaying)
        {
            StartCoroutine(DisplayTextCoroutine());
        }
    }

    public void AddChat(Tuple<string, string> text, float time)
    {
        PlayerName = text.Item1;
        TextToDisplay.AddRange(text.Item2.ToCharArray());

        if (!isDisplaying)
        {
            StartCoroutine(DisplayTextbyCharCoroutine(time*timeFactor/TextToDisplay.Count));
        }
    }

    public void AddChatToText(Tuple<string, string> text)
    {
        string newcontent = "";
        newcontent += "<color=#2491aa>";
        newcontent += text.Item1;
        newcontent += "</color>";
        newcontent += ":";
        newcontent += "\n";
        newcontent += text.Item2;
        newcontent += "\n";
        textView.text = textView.text + newcontent;
        StartCoroutine("ScrollToBottom");
    }

    IEnumerator ScrollToBottom()
    {
        yield return null;
        scrollControl.verticalNormalizedPosition = 0f;
    }
}
