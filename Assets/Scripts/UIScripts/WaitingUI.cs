using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class WaitingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] playerName;
    [SerializeField] private Image[] playerHeadSet;
    [SerializeField] private TMP_Text[] roomSet;
    [SerializeField] private TMP_Text roomId;
    [SerializeField] private TMP_Text TimeText;
    public event EventHandler<EventArgs> CloseWaitingEvent;
    private long CountTime = 0;
    private bool isCount = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CountTime > 0)
        {
            CountTime -= (long)Time.deltaTime;
            UpdateCountdownText();
        }
        else
        {
            if (isCount)
            {
                CloseWaitingEvent?.Invoke(this, new EventArgs());
            }
        }
    }
    void UpdateCountdownText()
    {
        TimeText.text = ((int)(CountTime / 1000)).ToString();
    }

    public void SetWaiting(WaitingEventArgs waitingEventArgs)
    {
        for (int i = 0; i < waitingEventArgs.Seats.Count; i++)
        {
            playerName[i].text = waitingEventArgs.Seats[i].Nickname;
        }
        roomSet[0].text = "底牌" + '\t' + waitingEventArgs.Ante.ToString() + '/' + waitingEventArgs.ScorePerPoint;
        roomSet[1].text = "圈數" + '\t' + waitingEventArgs.Round.ToString()+"圈";
        roomSet[2].text = "出牌時間" + '\t' + "6"+ "秒";//waitingEventArgs.Round.ToString()
        roomId.text = waitingEventArgs.TableID.ToString();

        if (waitingEventArgs.NextStateTime != null)
        {
            isCount = true;
            CountTime = (long)waitingEventArgs.NextStateTime - waitingEventArgs.Time;
            Debug.Log(CountTime);
            UpdateCountdownText();
            if (CountTime < 0)
            {
                CloseWaitingEvent?.Invoke(this, new EventArgs());
            }
        }
        else
        {
            TimeText.text = "0";
            Debug.Log("NextStateTime is null");
        }
    }

    public void SetWaiting(WaitingEventArgs waitingEventArgs,List<int> playerHeadIndex)
    {
        Sprite[] PlayerHeadset = AssetsPoolController.Instance.PlayerHeadset;
        for (int i = 0; i < waitingEventArgs.Seats.Count; i++)
        {
            playerName[i].text = waitingEventArgs.Seats[i].Nickname;
            playerHeadSet[i].sprite= PlayerHeadset[playerHeadIndex[i]];
        }
        for (int i = waitingEventArgs.Seats.Count; i < PlayerHeadset.Length; i++)
        {
            playerHeadSet[i].sprite = PlayerHeadset[0];
        }
        roomSet[0].text = "底牌" + '\t' + waitingEventArgs.Ante.ToString() + '/' + waitingEventArgs.ScorePerPoint;
        roomSet[1].text = "圈數" + '\t' + waitingEventArgs.Round.ToString() + "圈";
        roomSet[2].text = "出牌時間" + '\t' + "6" + "秒";//waitingEventArgs.Round.ToString()
        roomId.text = waitingEventArgs.TableID.ToString();

        if (waitingEventArgs.NextStateTime != null)
        {
            isCount = true;
            CountTime = (long)waitingEventArgs.NextStateTime - waitingEventArgs.Time;
            Debug.Log(CountTime);
            UpdateCountdownText();
            if (CountTime < 0)
            {
                CloseWaitingEvent?.Invoke(this, new EventArgs());
            }
        }
        else
        {
            TimeText.text = "0";
            Debug.Log("NextStateTime is null");
        }
    }

    public void SetPlayerHead(List<int> playerHeadIndex)
    {
        Sprite[] PlayerHeadset = AssetsPoolController.Instance.PlayerHeadset;
        for (int i = 0; i < playerHeadIndex.Count; i++)
        {
            playerHeadSet[i].sprite = PlayerHeadset[playerHeadIndex[i]];
        }
    }
}
