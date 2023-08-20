using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawedTileAreaController : TileAreaControllerBase,IPopTileAble
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AddTile(TileSuits tileSuit)
    {
        if(IsNormalTile(tileSuit))
            base.AddTile(tileSuit);
        else
            throw new System.Exception("Drawed tile must be normal tile");
    }
    public override void Init()
    {
        base.Init();
    }
    public void PopLastTile()
    {
        _TilesComponents[TileCount].Disappear();
        TileCount--;
    }
}
