using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Duty:處理花牌區
public class FlowerTileAreaController : TileAreaControllerBase
{
   
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void AddTile(TileSuits tileSuit)
    {
       if(TileCount>8)
       {
            Debug.LogError("Error:FlowerTileArea.AddFlowerTile() TileCount>8");
            return; 
       }
       else
       {
           if(this.IsFlowerTile(tileSuit)) 
           {
                base.AddTile(tileSuit);
                _TilesComponents[TileCount-1].ShowTileFrontSide();
           }
           else
           {
                Debug.LogError("Error:FlowerTileArea.AddFlowerTile() tileSuit is not flower");
                throw new System.Exception("Error:FlowerTileArea.AddFlowerTile() tileSuit is not flower");
            
           }
       }               
    }
    public override void SetTiles(List<TileSuits> tileSuits)
    {
        base.SetTiles(tileSuits);
    }

}
