using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerSettlementUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image HeadSet;
    public TMP_Text Wind;
    public TMP_Text PlayerId;
    public TMP_Text Score;
    public Image[] Tiles;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(SeatInfo seatInfo)
    {
        //HeadSet;
        Wind.text = seatInfo.DoorWind;
        PlayerId.text = seatInfo.Nickname;
        string totalScore = seatInfo.Scores.ToString();
        if (seatInfo.WinScores > 0)
        {
            totalScore += "+" + seatInfo.WinScores.ToString();
        }
        else if (seatInfo.WinScores < 0)
        {
            totalScore += seatInfo.WinScores.ToString();
        }
        for (int i = 0; i < 17; i++)
        {
            //AssetsPoolController.Instance.TileSprites[(int)seatInfo.tile];
        }
        Score.text = totalScore;
        //Tiles;
    }
}
