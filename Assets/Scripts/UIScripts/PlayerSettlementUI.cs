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
        PlayerId.text = playerResultData.Name;
        string totalScore = playerResultData.Scores.ToString();
        if (playerResultData.Score > 0)
        {
            totalScore += "+" + playerResultData.Score.ToString();
        }
        else if (playerResultData.Score < 0)
        {
            totalScore += playerResultData.Score.ToString();
        }
        Score.text = totalScore;
        Sprite[] sprites = AssetsPoolController.Instance.TileSprites;
        for (int i = 0; i < playerResultData.DoorTile.Count; i++)
        {
            switch (playerResultData.DoorTile[i].Count)
            {
                case 1:
                    Tiles[3 * i].sprite = sprites[(int)playerResultData.DoorTile[i][0]];
                    Tiles[3 * i + 1].sprite = sprites[(int)playerResultData.DoorTile[i][0]];
                    Tiles[3 * i + 2].sprite = sprites[(int)playerResultData.DoorTile[i][0]];
                    break;
                case 2:
                    Tiles[3 * i].sprite = sprites[(int)playerResultData.DoorTile[i][1]];
                    Tiles[3 * i + 1].sprite = sprites[(int)playerResultData.DoorTile[i][1]];
                    Tiles[3 * i + 2].sprite = sprites[(int)playerResultData.DoorTile[i][1]];
                    break;
                case 3:
                    Tiles[3 * i].sprite = sprites[(int)playerResultData.DoorTile[i][0]];
                    Tiles[3 * i + 1].sprite = sprites[(int)playerResultData.DoorTile[i][1]];
                    Tiles[3 * i + 2].sprite = sprites[(int)playerResultData.DoorTile[i][2]];
                    break;
            }
        }
        Debug.Log(playerResultData.Tile.Count);
        for (int i = 3* playerResultData.DoorTile.Count; i < playerResultData.Tile.Count; i++)
        {
            Debug.LogError(playerResultData.Tiles);
            Tiles[i].sprite = sprites[(int)playerResultData.Tile[i]];
        }
        //Tiles;
    }
}
