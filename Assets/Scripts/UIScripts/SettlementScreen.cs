using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SettlementScreen : MonoBehaviour
{
    [SerializeField] private Image[] Tiles;
    [SerializeField] private GameObject[] PointTypeObject;
    [SerializeField] private TMP_Text[] PointTypeText;
    [SerializeField] private TMP_Text[] PointTypeScore;
    [SerializeField] private Image Avatar;
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Wind;
    [SerializeField] private TMP_Text Score;
    [SerializeField] private TMP_Text TimeText;

    private long CountTime;
    //[SerializeField] private PlayerSettlementUI[] players;
    //[SerializeField]
    //private TMP_Text tMP_Text;
    //[SerializeField]
    //private GameObject PointType;
    //[SerializeField]
    //private TMP_Text PointType_Text;
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
            TimeText.text = "0";
        }
        else
        {
            TimeText.text = ((int)CountTime/1000).ToString();
        }
    }

    //public void SetSettlement(List<SeatInfo> seatInfos, long time)
    //{
    //    for (int i = 1; i < seatInfos.Count; i++)
    //    {
    //        bool swapped;
    //        do
    //        {
    //            swapped = false;
    //            if (seatInfos[i - 1].Scores > seatInfos[i].Scores)
    //            {
    //                SeatInfo temp = seatInfos[i - 1];
    //                seatInfos[i - 1] = seatInfos[i];
    //                seatInfos[i] = temp;
    //            }
    //        } while (swapped);
    //    }
    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        players[i].Set(seatInfos[i]);
    //    }
    //    CountTime = time;
    //}
    public void SetSettlement(List<PlayerResultData> playerResultDatas, List<string> PlayerName, List<int> AvatarIndex)
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
        int winner = -1;
        PointTypeReset();
        for (int i = 0; i < playerResultDatas.Count; i++)
        {
            //players[i].Set(playerResultDatas[i]);
            if (playerResultDatas[i].Winner == true)
            {
                winner = i;
            }
        }
        if (winner != -1)
        {
            int avatarIndex = -1;
            for (int i = 0; i < PlayerName.Count; i++)
            {
                if (playerResultDatas[winner].Name == PlayerName[i])
                {
                    avatarIndex = AvatarIndex[i];
                }
            }
            SetAvatar(avatarIndex);
            SetPointType(playerResultDatas[winner].PointList);
            Name.text = playerResultDatas[winner].Name;
            switch (playerResultDatas[winner].DoorWind)
            {
                case "East":
                    Wind.text = "�F";
                    break;
                case "West":
                    Wind.text = "��";
                    break;
                case "South":
                    Wind.text = "�n";
                    break;
                case "North":
                    Wind.text = "�_";
                    break;
                default:
                    break;
            }
            Score.text = playerResultDatas[winner].Scores.ToString() + " + " + playerResultDatas[winner].Score.ToString();

            Tiles[16].gameObject.SetActive(true);
            Sprite[] sprites = AssetsPoolController.Instance.TileSprites;
            for (int i = 0; i < playerResultDatas[winner].DoorTile.Count; i++)
            {
                switch (playerResultDatas[winner].DoorTile[i].Count)
                {
                    case 1:
                        Tiles[3 * i].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][0]];
                        Tiles[3 * i + 1].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][0]];
                        Tiles[3 * i + 2].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][0]];
                        break;
                    case 2:
                        Tiles[3 * i].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][1]];
                        Tiles[3 * i + 1].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][1]];
                        Tiles[3 * i + 2].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][1]];
                        break;
                    case 3:
                        Tiles[3 * i].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][0]];
                        Tiles[3 * i + 1].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][1]];
                        Tiles[3 * i + 2].sprite = sprites[(int)playerResultDatas[winner].DoorTile[i][2]];
                        break;
                }
            }

            for (int i = 0; i < playerResultDatas[winner].Tile.Count; i++)
            {
                Tiles[3 * playerResultDatas[winner].DoorTile.Count + i].sprite = sprites[(int)playerResultDatas[winner].Tile[i]];
            }
        }
        else
        {
            int index = 0;
            for (int i = 0; i < playerResultDatas.Count; i++)
            {
                if (playerResultDatas[i].Scores > playerResultDatas[index].Scores)
                {
                    index = i;
                }
            }
            int avatarIndex = -1;
            for (int i = 0; i < PlayerName.Count; i++)
            {
                if (playerResultDatas[index].Name == PlayerName[i])
                {
                    avatarIndex = AvatarIndex[i];
                }
            }
            SetAvatar(avatarIndex);

            SetPointType(playerResultDatas[index].PointList);
            Name.text = playerResultDatas[index].Name;
            Wind.text = playerResultDatas[index].DoorWind;
            switch (playerResultDatas[index].DoorWind)
            {
                case "East":
                    Wind.text = "�F";
                    break;
                case "West":
                    Wind.text = "��";
                    break;
                case "South":
                    Wind.text = "�n";
                    break;
                case "North":
                    Wind.text = "�_";
                    break;
                default:
                    break;
            }
            Score.text = playerResultDatas[index].Scores.ToString() + " + " + playerResultDatas[index].Score.ToString();

            Tiles[16].gameObject.SetActive(false);
            Sprite[] sprites = AssetsPoolController.Instance.TileSprites;
            for (int i = 0; i < playerResultDatas[index].DoorTile.Count; i++)
            {
                switch (playerResultDatas[index].DoorTile[i].Count)
                {
                    case 1:
                        Tiles[3 * i].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][0]];
                        Tiles[3 * i + 1].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][0]];
                        Tiles[3 * i + 2].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][0]];
                        break;
                    case 2:
                        Tiles[3 * i].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][1]];
                        Tiles[3 * i + 1].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][1]];
                        Tiles[3 * i + 2].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][1]];
                        break;
                    case 3:
                        Tiles[3 * i].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][0]];
                        Tiles[3 * i + 1].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][1]];
                        Tiles[3 * i + 2].sprite = sprites[(int)playerResultDatas[index].DoorTile[i][2]];
                        break;
                }
            }

            for (int i = 0; i < playerResultDatas[index].Tile.Count; i++)
            {
                Tiles[3 * playerResultDatas[index].DoorTile.Count + i].sprite = sprites[(int)playerResultDatas[index].Tile[i]];
            }
        }
    }

    public void SetAvatar(int index)
    {
        Sprite[] PlayerHeadset = AssetsPoolController.Instance.PlayerHeadset;
        if (index != -1)
            Avatar.sprite = PlayerHeadset[index];
        else
            Avatar.sprite = PlayerHeadset[0];
    }

    public void SetTime(long time)
    {
        CountTime = time;
    }

    public void SetPointType(PointType[] points)
    {
        if (points != null && points.Length != 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                PointTypeObject[i].SetActive(true);
                switch (points[i].Describe)
                {
                    case "Banker":
                        PointTypeText[i].text = "���a";
                        break;
                    case "RemainingBanker":
                        PointTypeText[i].text = "�s���Բ�";
                        break;
                    case "NoTriplet":
                        PointTypeText[i].text = "���J";
                        break;
                    case "ThreeConcealedTriplet":
                        PointTypeText[i].text = "�T�t��";
                        break;
                    case "FourConcealedTriplet":
                        PointTypeText[i].text = "�|�t��";
                        break;
                    case "FiveConcealedTriplet":
                        PointTypeText[i].text = "���t��";
                        break;
                    case "PongPong":
                        PointTypeText[i].text = "���J";
                        break;
                    case "TripletDragon":
                        PointTypeText[i].text = "�T���x";
                        break;
                    case "LittleThreeDragon":
                        PointTypeText[i].text = "�p�T��";
                        break;
                    case "BigThreeDragon":
                        PointTypeText[i].text = "�j�T��";
                        break;
                    case "RoundWind":
                        PointTypeText[i].text = "�魷�x";
                        break;
                    case "DoorWind":
                        PointTypeText[i].text = "�����x";
                        break;
                    case "LittleFourWind":
                        PointTypeText[i].text = "�p�|��";
                        break;
                    case "BigFourWind":
                        PointTypeText[i].text = "�j�|��";
                        break;
                    case "CorrectFlower":
                        PointTypeText[i].text = "����";
                        break;
                    case "FlowerKong":
                        PointTypeText[i].text = "��b";
                        break;
                    case "SevenRobsOne":
                        PointTypeText[i].text = "�C�m�@";
                        break;
                    case "FlowerKing":
                        PointTypeText[i].text = "�K�P�L��";
                        break;
                    case "SingleTile":
                        PointTypeText[i].text = "�Wť";
                        break;
                    case "DoorClear":
                        PointTypeText[i].text = "���M";
                        break;
                    case "SelfDrawn":
                        PointTypeText[i].text = "�ۺN";
                        break;
                    case "DoorClearAndSelfDrawn":
                        PointTypeText[i].text = "���M�ۺN";
                        break;
                    case "RobbingKong":
                        PointTypeText[i].text = "�m�b�J";
                        break;
                    case "SelfDrawnOnKong":
                        PointTypeText[i].text = "�b�W�}��";
                        break;
                    case "LastTileSelfDrawn":
                        PointTypeText[i].text = "��������";
                        break;
                    case "LastTile":
                        PointTypeText[i].text = "�����N��";
                        break;
                    case "AllFromOther":
                        PointTypeText[i].text = "���D�H";
                        break;
                    case "HeavenlyWin":
                        PointTypeText[i].text = "�ѭJ";
                        break;
                    case "EarthlyWin":
                        PointTypeText[i].text = "�a�J";
                        break;
                    case "AllSameColor":
                        PointTypeText[i].text = "�M�@��";
                        break;
                    case "AllHonors":
                        PointTypeText[i].text = "�r�@��";
                        break;
                    case "MixOneColor":
                        PointTypeText[i].text = "�V�@��";
                        break;
                    case "ReadyHand":
                        PointTypeText[i].text = "ť�P";
                        break;
                    case "HeavenlyReadyHand":
                        PointTypeText[i].text = "MIGI";
                        break;
                    case "EarthlyReadyHand":
                        PointTypeText[i].text = "MIGI";
                        break;
                    case "Dora":
                        PointTypeText[i].text = "�_�P";
                        break;
                    case "HaveFlowerOrWind":
                        PointTypeText[i].text = "���ᨣ�r";
                        break;
                    case "HaveKong":
                        PointTypeText[i].text = "�b�P";
                        break;
                    case "PointTypeNoHonorsAndFlowers":
                        PointTypeText[i].text = "�L�r�L��";
                        break;
                    default:
                        Debug.LogError("PointTypeError:" + points[i].Describe);
                        break;
                }
                PointTypeScore[i].text = points[i].Point.ToString();
            }
        }
        else
        {
            PointTypeObject[0].SetActive(true);
            PointTypeText[0].text = "�y��";
            PointTypeScore[0].text = "";
        }
    }

    public void PointTypeReset()
    {
        foreach (GameObject gameObject in PointTypeObject)
        {
            gameObject.SetActive(false);
        }
    }
}
