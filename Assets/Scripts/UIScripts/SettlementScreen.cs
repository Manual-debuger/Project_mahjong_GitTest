using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SettlementScreen : MonoBehaviour
{
    [SerializeField] private PlayerSettlementUI[] players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSettlement(List<SeatInfo> seatInfos)
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
    }
}
