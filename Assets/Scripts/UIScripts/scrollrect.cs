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
    public List<Tuple<String, String>> linesToDisplay = new();
    public float displayDuration = 2.0f;
    private bool isDisplaying = false;

    private void Start()
    {
    }

    IEnumerator DisplayTextCoroutine()
    {
        isDisplaying = true;

        while (linesToDisplay.Count!=0)
        {
            AddChatToText(linesToDisplay[0]);
            linesToDisplay.RemoveAt(0);
            yield return new WaitForSeconds(displayDuration);
        }
        isDisplaying = false;
    }
    public void AddChat(List<Tuple<String, String>> text)
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

    public void AddChatToText(Tuple<String, String> text)
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
