using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiscardTileUI : MonoBehaviour
{
    [SerializeField] private ActionUI actionUI;
    private ActionData[] Actions;
    public event EventHandler<ActionData> ActionEvent;
    public void ActionUISet(ActionData[] _actions)
    {
        Actions = _actions;
        actionUI.ActionUISet(_actions);
    }
    public void ActionUISetOff()
    {
        actionUI.ActionUISetOff();
    }
    public void Pass()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Pass)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Chow()//未完成
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Chow)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Pong()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID==Action.Pong)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Kong()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Kong || action.ID == Action.AdditionKong|| action.ID == Action.ConcealedKong)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Listen()//未完成
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.ReadyHand)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
    public void Winning()
    {
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Win || action.ID == Action.DrawnFromDeadWall || action.ID == Action.SelfDrawnWin)
            {
                ActionEvent?.Invoke(this, action);
                break;
            }
        }
    }
}

