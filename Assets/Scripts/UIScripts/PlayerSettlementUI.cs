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
    public void Set(PlayerResultData playerResultData)
    {
        //HeadSet;
        Wind.text = playerResultData.DoorWind;
        PlayerId.text = playerResultData.Nickname;
        string totalScore = playerResultData.Scores.ToString();
        if (playerResultData.Score > 0)
        {
            totalScore += "+" + playerResultData.WinScores.ToString();
        }
        else if (playerResultData.Score < 0)
        {
            totalScore += playerResultData.Score.ToString();
        }
        Score.text = totalScore;
        for (int i = 0; i < playerResultData.DoorTile.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Tiles[3 * i + j].sprite = AssetsPoolController.Instance.TileSprites[(int)playerResultData.DoorTile[i][j]];
            }
        }
        Debug.Log(playerResultData.Tile.Count);
        for (int i = 3* playerResultData.DoorTile.Count; i < playerResultData.Tile.Count; i++)
        {
            Debug.Log(i);
            Tiles[i].sprite = AssetsPoolController.Instance.TileSprites[(int)playerResultData.Tile[i]];
        }
        //Tiles;
    }
}
