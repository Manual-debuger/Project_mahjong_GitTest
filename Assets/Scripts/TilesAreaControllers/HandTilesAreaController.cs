using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTilesAreaController : TileAreaControllerBase,IPopTileAble
{
    public override void AddTile(TileSuits tileSuit)
    {
        if(this.TileCount<=16)
        {
            if(this.IsNormalTile(tileSuit))
                base.AddTile(tileSuit);
            else
            {
                Debug.LogError("Error:HandTilesArea.AddTile() tileSuit is not normal");
                throw new System.Exception("Error:HandTilesArea.AddTile() tileSuit is not normal");
            }
        }
        else
        {
            Debug.LogError("Error:HandTilesArea.AddTile() TileCount>16");
            throw new System.Exception("Error:HandTilesArea.AddTile() TileCount>16");
        }
    }
    public override void SetTiles(int tileCount)
    {
        base.SetTiles(tileCount);
    }
    public override void SetTiles(List<TileSuits> tileSuits)
    {
        base.SetTiles(tileSuits);
    }
    public override void UpdateTiles(List<TileSuits> tileSuits)
    {
        base.UpdateTiles(tileSuits);
    }

    public void PopLastTile()
    {
        if(this.TileCount<0)
        {
            Debug.LogError("Error:HandTilesArea.DeleteTile() TileCount<=0"); 
            return;
        }
        else
        {
            _TilesComponents[TileCount].Disappear();
            _TilesComponents[TileCount].ShowTileBackSide();
            TileCount--;
        }       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
