using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardTileUI : MonoBehaviour
{
    [SerializeField] private ActionUI actionUI;
    public void ActionUISet(ActionData[] actions)
    {
        actionUI.ActionUISet(actions);
    }
}

