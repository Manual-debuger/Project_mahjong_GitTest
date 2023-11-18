using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class HintUI : MonoBehaviour
{
    [SerializeField] private GameObject[] actionHintObject;
    [SerializeField]
    private TMP_Text[] actionHintText;
    // Start is called before the first frame update
    void Start()
    {
        closeAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void closeAll()
    {
        for (int i = 0; i < actionHintObject.Length; i++)
        {
            actionHintObject[i].SetActive(false);
        }
    }

    public async void showActionHint(int index, string showText)
    {
        actionHintObject[index].SetActive(true);
        actionHintText[index].text = showText;

        await Task.Delay(500);
        actionHintObject[index].SetActive(false);

    }
}
