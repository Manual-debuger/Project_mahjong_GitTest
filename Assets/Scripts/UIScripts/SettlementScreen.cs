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
                    str = str + "���a" + "/t" + points[i].Point;
                    break;
                case "RemainingBanker":
                    str = str + "�s���Բ�" + "/t" + points[i].Point;
                    break;
                case "NoTriplet":
                    str = str + "���J" + "/t" + points[i].Point;
                    break;
                case "ThreeConcealedTriplet":
                    str = str + "�T�t��" + "/t" + points[i].Point;
                    break;
                case "FourConcealedTriplet":
                    str = str + "�|�t��" + "/t" + points[i].Point;
                    break;
                case "FiveConcealedTriplet":
                    str = str + "���t��" + "/t" + points[i].Point;
                    break;
                case "PongPong":
                    str = str + "���J" + "/t" + points[i].Point;
                    break;
                case "TripletDragon":
                    str = str + "�T���x" + "/t" + points[i].Point;
                    break;
                case "LittleThreeDragon":
                    str = str + "�p�T��" + "/t" + points[i].Point;
                    break;
                case "BigThreeDragon":
                    str = str + "�j�T��" + "/t" + points[i].Point;
                    break;
                case "RoundWind":
                    str = str + "�魷�x" + "/t" + points[i].Point;
                    break;
                case "DoorWind":
                    str = str + "�����x" + "/t" + points[i].Point;
                    break;
                case "LittleFourWind":
                    str = str + "�p�|��" + "/t" + points[i].Point;
                    break;
                case "BigFourWind":
                    str = str + "�j�|��" + "/t" + points[i].Point;
                    break;
                case "CorrectFlower":
                    str = str + "����" + "/t" + points[i].Point;
                    break;
                case "FlowerKong":
                    str = str + "��b" + "/t" + points[i].Point;
                    break;
                case "SevenRobsOne":
                    str = str + "�C�m�@" + "/t" + points[i].Point;
                    break;
                case "FlowerKing":
                    str = str + "�K�P�L��" + "/t" + points[i].Point;
                    break;
                case "SingleTile":
                    str = str + "�Wť" + "/t" + points[i].Point;
                    break;
                case "DoorClear":
                    str = str + "���M" + "/t" + points[i].Point;
                    break;
                case "SelfDrawn":
                    str = str + "�ۺN" + "/t" + points[i].Point;
                    break;
                case "DoorClearAndSelfDrawn":
                    str = str + "���M�ۺN" + "/t" + points[i].Point;
                    break;
                case "RobbingKong":
                    str = str + "�m�b�J" + "/t" + points[i].Point;
                    break;
                case "SelfDrawnOnKong":
                    str = str + "�b�W�}��" + "/t" + points[i].Point;
                    break;
                case "LastTileSelfDrawn":
                    str = str + "��������" + "/t" + points[i].Point;
                    break;
                case "LastTile":
                    str = str + "�����N��" + "/t" + points[i].Point;
                    break;
                case "AllFromOther":
                    str = str + "���D�H" + "/t" + points[i].Point;
                    break;
                case "HeavenlyWin":
                    str = str + "�ѭJ" + "/t" + points[i].Point;
                    break;
                case "EarthlyWin":
                    str = str + "�a�J" + "/t" + points[i].Point;
                    break;
                case "AllSameColor":
                    str = str + "�M�@��" + "/t" + points[i].Point;
                    break;
                case "AllHonors":
                    str = str + "�r�@��" + "/t" + points[i].Point;
                    break;
                case "MixOneColor":
                    str = str + "�V�@��" + "/t" + points[i].Point;
                    break;
                case "ReadyHand":
                    str = str + "ť�P" + "/t" + points[i].Point;
                    break;
                case "HeavenlyReadyHand":
                    str = str + "MIGI" + "/t" + points[i].Point;
                    break;
                case "EarthlyReadyHand":
                    str = str + "MIGI" + "/t" + points[i].Point;
                    break;
                case "Dora":
                    str = str + "�a��P" + "/t" + points[i].Point;
                    break;
                case "HaveFlowerOrWind":
                    str = str + "���ᨣ�r" + "/t" + points[i].Point;
                    break;
                case "HaveKong":
                    str = str + "�b�P" + "/t" + points[i].Point;
                    break;
                case "PointTypeNoHonorsAndFlowers":
                    str = str + "�L�r�L��" + "/t" + points[i].Point;
                    break;
            }
            if (i != points.Length - 1)
            {
            str += "/n";
            }
        }
    }
}
