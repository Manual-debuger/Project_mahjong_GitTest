using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListenTile : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    //public event EventHandler<TileIndexEventArgs> DiscardTileEvent;
    //public event EventHandler<TileIndexEventArgs> OnPointerDownEvent;
    //public event EventHandler<TileIndexEventArgs> OnPointerUpEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTile(Sprite texture)
    {
        this._image.sprite = texture;
    }
    public void Appear()
    {
        this.gameObject.SetActive(true);
    }
    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //Debug.Log("OnPointerDown");
    //    OnPointerDownEvent?.Invoke(this, new TileIndexEventArgs(index));
    //}
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    //Debug.Log("OnPointerUp");
    //    OnPointerUpEvent?.Invoke(this, new TileIndexEventArgs(index));
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log("OnPointerEnter");
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Debug.Log("OnPointerExit");
    //}
}
