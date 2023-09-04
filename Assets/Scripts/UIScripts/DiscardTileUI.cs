using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DiscardTileUI : MonoBehaviour
{
    [SerializeField] private ActionUI actionUI;
    private ActionData[] Actions;
    public event EventHandler<ActionData> ActionEvent;
    public event EventHandler<ActionData> ListenOnActionEvent;

    public GameObject ChowObject;
    public GameObject[] ChowOption;
    public HandTileUI[] ChowOptionTiles;

    public GameObject ListenObject;
    public GameObject[] ListenOption;
    public ListenTile[] ListenOptionTiles;
    public TMP_Text[] ListenOptionRemain;
    public TMP_Text[] ListenOptionScore;
    public void ActionUISetOn(ActionData[] _actions)
    {
        Actions = _actions;
        actionUI.ActionUISet(_actions);
    }
    public void ActionUISetOff()
    {
        actionUI.ActionUISetOff();
    }
    public void Pass()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Pass)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Chow()//未完成
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Chow)
            {
                if (action.OptionTiles.Count == 1)
                    ActionEvent?.Invoke(this, action);
                else
                    SetChowOptionOn();
                break;
            }
        }
    }
    public void Pong()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Pong)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Kong()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Kong || action.ID == Action.AdditionKong || action.ID == Action.ConcealedKong)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Listen()//未完成
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.ReadyHand)
            {
                if (action.OptionTiles.Count == 1)
                    ActionEvent?.Invoke(this, action);
                else
                    SetListenOptionOn();
                break;
            }
        }
    }
    public void Winning()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Win || action.ID == Action.DrawnFromDeadWall || action.ID == Action.SelfDrawnWin)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }


    public void SetChowOptionOn()
    {
        SetListenOptionOff();
        Sprite[] TileSprites = AssetsPoolController.Instance.TileSprites;
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Chow)
            {
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
    public void SetListenOptionOn()
    {
        SetChowOptionOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.ReadyHand)
            {
                ListenObject.SetActive(true);
                ListenOnActionEvent?.Invoke(this, action);
            }
        }

    }

    public void SetListenTileSuggest(ListeningTilesType listeningTilesTypes)
    {
        int count = 0;
        foreach (KeyValuePair<string, int> keyValuePair in listeningTilesTypes.Mahjong)
        {
            ListenOption[count].SetActive(true);
            ListenOptionTiles[count].Appear();
            ListenOptionRemain[count].GetComponent<GameObject>().SetActive(true);
            ListenOptionScore[count].GetComponent<GameObject>().SetActive(true);
            ListenOptionRemain[count].text = "台數" + "\n" + "999";
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
            ListenOptionRemain[i].GetComponent<GameObject>().SetActive(false);
            ListenOptionScore[i].GetComponent<GameObject>().SetActive(false);
        }
    }

    public void SetListenOptionOff()
    {
        ListenObject.SetActive(false);
    }

    public void ChowSelect(int index)
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Chow)
            {
                List<TileSuits> tile = action.OptionTiles[index];
                List<List<TileSuits>> tilelist = new List<List<TileSuits>>();
                tilelist.Add(tile);
                action.OptionTiles = tilelist;
                ActionEvent?.Invoke(this, action);
            }
        }
        SetChowOptionOff();
    }

    public void buttontest()
    {
        Debug.Log("test");
    }
}

