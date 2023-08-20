using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Server as a model for the abandoned tiles area
public class SeaTilesAreaController : TileAreaControllerBase,IPopTileAble
{
    
    private List<TileComponent> _highLightedTilesComponents;
    public override void AddTile(TileSuits tileSuit)
    {
        if(TileCount>24) { Debug.LogError("Error:AbandonedTilesArea.AddTile() TileCount>24"); return; }
        
        if(this.IsNormalTile(tileSuit))
        {
            base.AddTile(tileSuit);
            _TilesComponents[TileCount-1].ShowTileFrontSide();
        }
        else
        {
            Debug.LogError("Error:SeaTilesAreaController.AddTile() tileSuit Should be a NormalTile");
            throw new System.Exception("Error:SeaTilesAreaController.AddTile() Should be a NormalTile");            
        }        
    }
    public void PopLastTile()
    {
        if(TileCount<0) 
        {
            Debug.LogError("Error:AbandonedTilesArea.DeleteTile() TileCount<=0"); 
            return;
        }
        else
        {           
            _TilesComponents[TileCount].Disappear();
            _TilesComponents[TileCount].ShowTileBackSide();
            TileCount--;
        }
        
    }

    public void HighLightDiscardTiles(TileSuits tileSuit)
    {
        foreach (var tileComponent in _TilesComponents)
        {
            if (tileComponent.TileSuit == tileSuit)
            {
                tileComponent.HighLight();
                _highLightedTilesComponents.Add(tileComponent);
            }
                
        }
    }
    public void UnHighLightDiscardTiles()
    {
        foreach (var tileComponent in _highLightedTilesComponents)
        {
            tileComponent.UnHighLight();
        }
        _highLightedTilesComponents.Clear();
    }
    private void UnHighLightAllTiles()
    {
        foreach (var tileComponent in _TilesComponents)
        {
            tileComponent.UnHighLight();
        }
    }
    private void Awake()
    {
        _highLightedTilesComponents=new List<TileComponent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        this.UnHighLightAllTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
