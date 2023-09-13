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
    public event EventHandler<ActionData> ListenOnActionEvent;

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
        actionUI.ActionUISetOff();
    }
    //public void Pass()
    //{
    //    ActionUISetOff();
    //    foreach (ActionData action in Actions)
    //    {
    //        if (action.ID == Action.Pass)
    //        {
    //            ActionEvent?.Invoke(this, action);
    //            break;
    //        }
    //    }
    //}
    //public void Chow()
    //{
    //    ActionUISetOff();
    //    foreach (ActionData action in Actions)
    //    {
    //        if (action.ID == Action.Chow)
    //        {
    //            if (action.OptionTiles.Count == 1)
    //                ActionEvent?.Invoke(this, action);
    //            else
    //                SetChowOptionOn(action);
    //            break;
    //        }
    //    }
    //}
    //public void Pong()
    //{
    //    ActionUISetOff();
    //    foreach (ActionData action in Actions)
    //    {
    //        if (action.ID == Action.Pong)
    //        {
    //            ActionEvent?.Invoke(this, action);
    //            break;
    //        }
    //    }
    //}
    //public void Kong()
    //{
    //    ActionUISetOff();
    //    foreach (ActionData action in Actions)
    //    {
    //        if (action.ID == Action.Kong || action.ID == Action.AdditionKong || action.ID == Action.ConcealedKong)
    //        {
    //            ActionEvent?.Invoke(this, action);
    //            break;
    //        }
    //    }
    //}
    //public void Listen()
    //{
    //    ActionUISetOff();
    //    foreach (ActionData action in Actions)
    //    {
    //        if (action.ID == Action.ReadyHand)
    //        {
    //            if (action.OptionTiles.Count == 1)
    //                ActionEvent?.Invoke(this, action);
    //            else
    //                SetListenOptionOn();
    //            break;
    //        }
    //    }
    //}
    //public void Winning()
    //{
    //    ActionUISetOff();
    //    foreach (ActionData action in Actions)
    //    {
    //        if (action.ID == Action.Win || 
    //            action.ID == Action.DrawnFromDeadWall || 
    //            action.ID == Action.SelfDrawnWin)
    //        {
    //            ActionEvent?.Invoke(this, action);
    //            break;
    //        }
    //    }
    //}


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
        if (action.ID == Action.ReadyHand)
        {
            ListenObject.SetActive(true);
            ListenOnActionEvent?.Invoke(this, action);
        }
    }

    public void SetListenTileSuggest(ListeningTilesType listeningTilesTypes)
    {
        int count = 0;
        foreach (GameObject listenTile in ListenOption)
        {
            listenTile.SetActive(false);
        }
        foreach (KeyValuePair<string, int> keyValuePair in listeningTilesTypes.Mahjong)
        {
            ListenOption[count].SetActive(true);
            ListenOptionTiles[count].Appear();
            //ListenOptionRemain[count].GetComponent<GameObject>().SetActive(true);
            //ListenOptionScore[count].GetComponent<GameObject>().SetActive(true);
            ListenOptionRemain[count].text = "剩餘" + "\n" + keyValuePair.Value;
            //ListenOptionScore[count];

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

