using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DataTransformNamespace;
//Duty: To display the tiles in the hand of the player跟回傳事件
public class HandTilesUI : MonoBehaviour
{
    [SerializeField]
    public List<HandTileUI> _TilesComponents = new();
    [SerializeField]
    private Sprite[] _tileMeshs;
    Dictionary<TileSuits, Dictionary<TileSuits, int>> readyInfo;
    public event EventHandler<TileIndexEventArgs> DiscardTileEvent;
    public event EventHandler<TileIndexEventArgs> OnPointerDownEvent;
    public event EventHandler<TileIndexEventArgs> OnPointerUpEvent;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 17; i++)
        {
            //Debug.Log(i);
            _TilesComponents[i].DiscardTileEvent += DiscardTile;
            _TilesComponents[i].OnPointerDownEvent += OnPointerDown;
            _TilesComponents[i].OnPointerUpEvent += OnPointerUp;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Appear()
    {
        this.gameObject.SetActive(true);
    }
    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }

    public void HandTileSet(int index, TileSuits HandTileSuit)
    {
        if (HandTileSuit != TileSuits.NULL)
        {
            _TilesComponents[index].Appear();
            _TilesComponents[index].SetTile(_tileMeshs[(int)HandTileSuit]);
        }
        else
            _TilesComponents[index].Disappear();
    }
    private void DiscardTile(object sender, TileIndexEventArgs e)
    {
        DiscardTileEvent?.Invoke(this, e);
    }
    private void OnPointerDown(object sender, TileIndexEventArgs e)
    {
        //Debug.Log("UI");
        OnPointerDownEvent?.Invoke(this, e);
    }

    private void OnPointerUp(object sender, TileIndexEventArgs e)
    {
        //Debug.Log("UI");
        OnPointerUpEvent?.Invoke(this, e);
    }

    public void ListenSetOn(List<int> ListenIndex)
    {
        foreach (HandTileUI handTileUI in _TilesComponents)
        {
            handTileUI.SetDark();
        }
        foreach (int index in ListenIndex)
        {
            _TilesComponents[index].SetYellow();
        }
    }

    public void ListenSetOff()
    {
        foreach (HandTileUI handTileUI in _TilesComponents)
        {
            handTileUI.SetBright();
        }
    }

    public void DiscardTileSet(bool[] CanDiscardList)
    {
        for (int i = 0; i < 17; i++)
        {
            if (CanDiscardList[i])
            {
                _TilesComponents[i].SetBright();
            }
            else
            {
                _TilesComponents[i].SetDark();
            }
        }
    }

    public void SetBright()
    {
        foreach (HandTileUI tileComponent in _TilesComponents)
        {
            tileComponent.SetBright();
        }
    }
    public void SetDark()
    {
        foreach (HandTileUI tileComponent in _TilesComponents)
        {
            tileComponent.SetDark();
        }
    }
}
