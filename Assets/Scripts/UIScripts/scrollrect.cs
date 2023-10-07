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

    private void Start()
    {
    }

    public void AddChat(List<Tuple<String, String>> text)
    {
        string newcontent = "";
        for (int i = text.Count - 1; i >= 0; i--)
        {
            newcontent += "\n";
            newcontent += text[i].Item1;
            newcontent += "\n";
            newcontent += text[i].Item2;
        }
        textView.text = textView.text + newcontent;
        StartCoroutine("ScrollToBottom");
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollControl.verticalNormalizedPosition = 0f;
    }
}
