using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionUI : MonoBehaviour
{
    [SerializeField] private Button Pass;
    [SerializeField] private Button Chow;
    [SerializeField] private Button Pong;
    [SerializeField] private Button Kong;
    [SerializeField] private Button Listen;
    [SerializeField] private Button Winning;
    // Start is called before the first frame update
    void Start()
    {
        Pass.gameObject.SetActive(false);
        Chow.gameObject.SetActive(false);
        Pong.gameObject.SetActive(false);
        Kong.gameObject.SetActive(false);
        Listen.gameObject.SetActive(false);
        Winning.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActionUISet(ActionData[] _actions)
    {
        foreach (ActionData action in _actions)
        {
            switch (action.ID)
            {
                case Action.Pass:
                    Pass.gameObject.SetActive(true);
                    break;
                case Action.Chow:
                    Chow.gameObject.SetActive(true);
                    break;
                case Action.Pong:
                    Pong.gameObject.SetActive(true);
                    break;
                case Action.Kong:
                case Action.AdditionKong:
                case Action.ConcealedKong:
                    Kong.gameObject.SetActive(true);
                    break;
                case Action.ReadyHand:
                    Listen.gameObject.SetActive(true);
                    break;
                case Action.Win:
                case Action.DrawnFromDeadWall:
                case Action.SelfDrawnWin:
                    Winning.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void ActionUISetOff()
    {
        Pass.gameObject.SetActive(false);
        Chow.gameObject.SetActive(false);
        Pong.gameObject.SetActive(false);
        Kong.gameObject.SetActive(false);
        Listen.gameObject.SetActive(false);
        Winning.gameObject.SetActive(false);
    }
}
