using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SettlementScreen : MonoBehaviour
{
    [SerializeField] private PlayerSettlementUI[] players;
    private long CountTime;
    [SerializeField]
    private TMP_Text tMP_Text;
    // Start is called before the first frame update
    void Start()
    {
        CountTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CountTime -= (long)(Time.deltaTime * 1000);
        if (CountTime < 0)
        {
            tMP_Text.text = "0";
        }
        else
        {
            tMP_Text.text = ((int)CountTime/1000).ToString();
        }
    }

    public void SetSettlement(List<SeatInfo> seatInfos, long time)
    {
        for (int i = 1; i < seatInfos.Count; i++)
        {
            bool swapped;
            do
            {
                swapped = false;
                if (seatInfos[i - 1].Scores > seatInfos[i].Scores)
                {
                    SeatInfo temp = seatInfos[i - 1];
                    seatInfos[i - 1] = seatInfos[i];
                    seatInfos[i] = temp;
                }
            } while (swapped);
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Set(seatInfos[i]);
        }
        CountTime = time;
    }
    public void SetSettlement(List<PlayerResultData> playerResultDatas, long time)
    {
        for (int i = 1; i < playerResultDatas.Count; i++)
        {
            bool swapped;
            do
            {
                swapped = false;
                if (playerResultDatas[i - 1].Scores > playerResultDatas[i].Scores)
                {
                    PlayerResultData temp = playerResultDatas[i - 1];
                    playerResultDatas[i - 1] = playerResultDatas[i];
                    playerResultDatas[i] = temp;
                }
            } while (swapped);
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Set(playerResultDatas[i]);
        }
        CountTime = time;
    }
}
