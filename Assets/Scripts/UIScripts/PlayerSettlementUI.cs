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
        Score.text = seatInfo.Scores.ToString();
        //Tiles;
    }
}
