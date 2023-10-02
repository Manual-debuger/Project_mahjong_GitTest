using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DiscardTileUI : MonoBehaviour
{
    [SerializeField] private ActionUI actionUI;
    //private ActionData[] Actions;
    //public event EventHandler<ActionData> ActionEvent;

    public GameObject ChowObject;
    public GameObject[] ChowOption;
    public HandTileUI[] ChowOptionTiles;

    public GameObject ListenObject;
    public GameObject[] ListenOption;
    public ListenTile[] ListenOptionTiles;
    public TMP_Text[] ListenOptionRemain;
    public TMP_Text[] ListenOptionScore;
    public TMP_Text TimeTxt;
    //public void ActionUISetOn(ActionData[] _actions)
    //{
    //    Actions = _actions;
    //    actionUI.ActionUISet(_actions);
    //}
    public void ActionUISetOff()
    {
        SetChowOptionOff();
        SetListenOptionOff();
        actionUI.ActionUISetOff();
    }
    public void SetChowOptionOn(ActionData action)
    {
        //SetListenOptionOff();
        Sprite[] TileSprites = AssetsPoolController.Instance.TileSprites;
        ChowObject.SetActive(true);
        for (int i = 0; i < action.OptionTiles.Count; i++)
        {
            ChowOption[i].SetActive(true);
            for (int j = 0; j < 3; j++)
            {
                ChowOptionTiles[3 * i + j].SetTile(TileSprites[(int)action.OptionTiles[i][j]]);
            }
        }
    }

    public void SetChowOptionOff()
    {
        for (int i = 0; i < 3; i++)
        {
            ChowOption[i].SetActive(false);
        }
        ChowObject.SetActive(false);
    }
    public void SetListenOptionOn(ActionData action)
    {
        SetChowOptionOff();
        ListenObject.SetActive(true);
    }

    public void SetListenTileSuggest(Dictionary<TileSuits, int> listeningTilesTypes)
    {
        int count = 0;
        foreach (GameObject listenTile in ListenOption)
        {
            listenTile.SetActive(false);
        }
        foreach (KeyValuePair<TileSuits, int> keyValuePair in listeningTilesTypes)
        {
            ListenOption[count].SetActive(true);
            ListenOptionTiles[count].Appear();
            ListenOptionTiles[count].SetTile(AssetsPoolController.Instance.TileSprites[(int)keyValuePair.Key]);
            //ListenOptionRemain[count].GetComponent<GameObject>().SetActive(true);
            //ListenOptionScore[count].GetComponent<GameObject>().SetActive(true);
            //ListenOptionRemain[count].text = "剩餘" + "\n" + keyValuePair.Value;
            ListenOptionScore[count].text = "台數" + "\n" + keyValuePair.Value;

            count++;
        }
    }

    public void CloseListenTileSuggest()
    {
        for (int i = 0; i < 10; i++)
        {
            ListenOption[i].SetActive(false);
            ListenOptionTiles[i].Disappear();
            //ListenOptionRemain[i].GetComponent<GameObject>().SetActive(false);
            //ListenOptionScore[i].GetComponent<GameObject>().SetActive(false);
        }
    }

    public void SetListenOptionOff()
    {
        foreach (GameObject listenTile in ListenOption)
        {
            listenTile.SetActive(false);
        }
        ListenObject.SetActive(false);
    }
    public void buttontest()
    {
        //Debug.Log("test");
    }

    public void SetPassOn()
    {
        actionUI.SetPassOn();
    }
    public void SetChowOn()
    {
        actionUI.SetChowOn();
    }
    public void SetPongOn()
    {
        actionUI.SetPongOn();
    }
    public void SetKongOn()
    {
        actionUI.SetKongOn();
    }
    public void SetListenOn()
    {
        actionUI.SetListenOn();
    }
    public void SetWinningOn()
    {
        actionUI.SetWinningOn();
    }
    public void SetPassOff()
    {
        actionUI.SetPassOff();
    }
    public void SetChowOff()
    {
        actionUI.SetChowOff();
    }
    public void SetPongOff()
    {
        actionUI.SetPongOff();
    }
    public void SetKongOff()
    {
        actionUI.SetKongOff();
    }
    public void SetListenOff()
    {
        actionUI.SetListenOff();
    }
    public void SetWinningOff()
    {
        actionUI.SetWinningOff();
    }
    public void SetTime(int CountTime)
    {
        TimeTxt.text = CountTime.ToString();
    }
}

