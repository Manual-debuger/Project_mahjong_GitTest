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
    [SerializeField]
    private GameObject PointType;
    [SerializeField]
    private TMP_Text PointType_Text;
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
    public void SetSettlement(List<PlayerResultData> playerResultDatas)
    {
        //for (int i = 1; i < playerResultDatas.Count; i++)
        //{
        //    bool swapped;
        //    do
        //    {
        //        swapped = false;
        //        if (playerResultDatas[i - 1].Scores > playerResultDatas[i].Scores)
        //        {
        //            PlayerResultData temp = playerResultDatas[i - 1];
        //            playerResultDatas[i - 1] = playerResultDatas[i];
        //            playerResultDatas[i] = temp;
        //            swapped = true;
        //        }
        //    } while (swapped);
        //}
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Set(playerResultDatas[i]);
            if (playerResultDatas[i].PointList != null && playerResultDatas[i].PointList.Length != 0)
            {
                setPointType(playerResultDatas[i].PointList);
            }
        }
    }

    public void SetTime(long time)
    {
        CountTime = time;
    }

    public void setPointType(PointType[] points)
    {
        string str ="";
        for (int i = 0; i < points.Length - 1; i++)
        {
            switch (points[i].Describe)
            {
                case "Banker":
                    str = str + "莊家" + "/t" + points[i].Point;
                    break;
                case "RemainingBanker":
                    str = str + "連莊拉莊" + "/t" + points[i].Point;
                    break;
                case "NoTriplet":
                    str = str + "平胡" + "/t" + points[i].Point;
                    break;
                case "ThreeConcealedTriplet":
                    str = str + "三暗刻" + "/t" + points[i].Point;
                    break;
                case "FourConcealedTriplet":
                    str = str + "四暗刻" + "/t" + points[i].Point;
                    break;
                case "FiveConcealedTriplet":
                    str = str + "五暗刻" + "/t" + points[i].Point;
                    break;
                case "PongPong":
                    str = str + "對對胡" + "/t" + points[i].Point;
                    break;
                case "TripletDragon":
                    str = str + "三元台" + "/t" + points[i].Point;
                    break;
                case "LittleThreeDragon":
                    str = str + "小三元" + "/t" + points[i].Point;
                    break;
                case "BigThreeDragon":
                    str = str + "大三元" + "/t" + points[i].Point;
                    break;
                case "RoundWind":
                    str = str + "圈風台" + "/t" + points[i].Point;
                    break;
                case "DoorWind":
                    str = str + "門風台" + "/t" + points[i].Point;
                    break;
                case "LittleFourWind":
                    str = str + "小四喜" + "/t" + points[i].Point;
                    break;
                case "BigFourWind":
                    str = str + "大四喜" + "/t" + points[i].Point;
                    break;
                case "CorrectFlower":
                    str = str + "正花" + "/t" + points[i].Point;
                    break;
                case "FlowerKong":
                    str = str + "花槓" + "/t" + points[i].Point;
                    break;
                case "SevenRobsOne":
                    str = str + "七搶一" + "/t" + points[i].Point;
                    break;
                case "FlowerKing":
                    str = str + "八仙過海" + "/t" + points[i].Point;
                    break;
                case "SingleTile":
                    str = str + "獨聽" + "/t" + points[i].Point;
                    break;
                case "DoorClear":
                    str = str + "門清" + "/t" + points[i].Point;
                    break;
                case "SelfDrawn":
                    str = str + "自摸" + "/t" + points[i].Point;
                    break;
                case "DoorClearAndSelfDrawn":
                    str = str + "門清自摸" + "/t" + points[i].Point;
                    break;
                case "RobbingKong":
                    str = str + "搶槓胡" + "/t" + points[i].Point;
                    break;
                case "SelfDrawnOnKong":
                    str = str + "槓上開花" + "/t" + points[i].Point;
                    break;
                case "LastTileSelfDrawn":
                    str = str + "海底撈月" + "/t" + points[i].Point;
                    break;
                case "LastTile":
                    str = str + "海底摸魚" + "/t" + points[i].Point;
                    break;
                case "AllFromOther":
                    str = str + "全求人" + "/t" + points[i].Point;
                    break;
                case "HeavenlyWin":
                    str = str + "天胡" + "/t" + points[i].Point;
                    break;
                case "EarthlyWin":
                    str = str + "地胡" + "/t" + points[i].Point;
                    break;
                case "AllSameColor":
                    str = str + "清一色" + "/t" + points[i].Point;
                    break;
                case "AllHonors":
                    str = str + "字一色" + "/t" + points[i].Point;
                    break;
                case "MixOneColor":
                    str = str + "混一色" + "/t" + points[i].Point;
                    break;
                case "ReadyHand":
                    str = str + "聽牌" + "/t" + points[i].Point;
                    break;
                case "HeavenlyReadyHand":
                    str = str + "MIGI" + "/t" + points[i].Point;
                    break;
                case "EarthlyReadyHand":
                    str = str + "MIGI" + "/t" + points[i].Point;
                    break;
                case "Dora":
                    str = str + "懸賞牌" + "/t" + points[i].Point;
                    break;
                case "HaveFlowerOrWind":
                    str = str + "見花見字" + "/t" + points[i].Point;
                    break;
                case "HaveKong":
                    str = str + "槓牌" + "/t" + points[i].Point;
                    break;
                case "PointTypeNoHonorsAndFlowers":
                    str = str + "無字無花" + "/t" + points[i].Point;
                    break;
            }
            if (i != points.Length - 1)
            {
            str += "/n";
            }
        }
    }
}
