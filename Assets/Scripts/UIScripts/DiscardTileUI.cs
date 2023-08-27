using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardTileUI : MonoBehaviour
{
    [SerializeField] private ActionUI actionUI;
    private ActionData[] Actions;
    public void ActionUISet(ActionData[] _actions)
    {
        Actions = _actions;
        actionUI.ActionUISet(_actions);
    }
}

