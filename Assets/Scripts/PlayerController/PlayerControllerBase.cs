using System;
using System.Collections.Generic;
using UnityEngine;

//提供基本(本地跟其他三位玩家)都有的基本操作
public class PlayerControllerBase : MonoBehaviour,IInitiable
{
    //private HandTilesAreaController _handTiles;    
    [SerializeField] protected FlowerTileAreaController _flowerTileAreaController;
    [SerializeField] protected MeldsAreaController _meldsAreaController;
    [SerializeField] protected PlayerInfoPlateController _playerInfoPlateController;
    [SerializeField] protected SeaTilesAreaController _seaTilesAreaController;    



    public TileSuits[] FlowerTileSuits { get { return _flowerTileAreaController.GetTileSuits(); } }
    //public TileSuit[] MeldTilesuits { get { return _meldsAreaController.TilesSuits; } }

    void Awake()
    {
        Init();
        if(_flowerTileAreaController == null) 
            _flowerTileAreaController=this.GetComponentInChildren<FlowerTileAreaController>();
        if(_seaTilesAreaController == null) 
            _seaTilesAreaController=this.GetComponentInChildren<SeaTilesAreaController>();
        if(_meldsAreaController == null)
            _meldsAreaController=this.GetComponentInChildren<MeldsAreaController>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public virtual void AddDrawedTile(TileSuits tileSuit) { Debug.LogWarning("Must override this function AddDrawedTile, Do NOT Use this base function"); }
               
    public void AddFlowerTile(TileSuits tileSuit)
    {
        _flowerTileAreaController.AddTile(tileSuit);
    }
    public void AddMeldTile(List<TileSuits> tileSuitsList)
    {
        _meldsAreaController.AddMeld(tileSuitsList);
    }    
    public virtual void DiscardTile(TileSuits tileSuit) {Debug.LogWarning("Must override this function DiscardTile, Do NOT Use this base function"); }

    public virtual void SetSeatInfo(SeatInfo seatInfo)
    {       
        try
        {
            if (seatInfo == null)
            {
                Debug.LogWarning("SetSeatInfo(seatinfo) seatinfo is null");
                seatInfo = new SeatInfo();            
            }
            try 
            { _playerInfoPlateController.SetUserName(seatInfo.Nickname); } 
            catch (Exception ex)
            {
                Debug.LogWarning("SetUserName Wrong");
                InGameUIController.Instance.ShowError($"SetUserName Wrong:{ex.Message}");
                throw; 
            }
            try 
                { _playerInfoPlateController.SetWindPosision(seatInfo.DoorWind.ToString()); }
            catch(Exception ex)
            {
                Debug.LogWarning("SetWindPosision Wrong");
                InGameUIController.Instance.ShowError($"SetWindPosision Wrong:{ex.Message}");
                throw; 
            }
            try 
                { _flowerTileAreaController.SetTiles(seatInfo.FlowerTile); }
            catch (Exception ex)
            {
                Debug.LogWarning("SetTiles Wrong");
                InGameUIController.Instance.ShowError($"SetTiles Wrong:{ex.Message}");
                throw;
            }
            try 
                { _seaTilesAreaController.SetTiles(seatInfo.SeaTile); } 
            catch (Exception ex)
            {
                Debug.LogWarning("SetSeaTiles Wrong");
                InGameUIController.Instance.ShowError($"SetSeaTiles Wrong:{ex.Message}");
                throw;
            }
            try 
                { _meldsAreaController.SetDoors(seatInfo.DoorTile); }
            catch (Exception ex)
            {
                Debug.LogWarning("SetDoors Wrong");
                InGameUIController.Instance.ShowError($"SetDoors Wrong:{ex.Message}");
                throw;
            }
        }
        catch (Exception ex)
        {
            InGameUIController.Instance.ShowError($"SetSeatInfo Error {ex.Message}");
            Time.timeScale = 0;
            throw;
        }
        
    }
    public virtual void UpdateSeatInfo(SeatInfo seatInfo)
    {
        try
        {
            _seaTilesAreaController.SetTiles(seatInfo.SeaTile);
            _flowerTileAreaController.SetTiles(seatInfo.FlowerTile);
            _meldsAreaController.SetDoors(seatInfo.DoorTile);
        }
        catch (System.Exception)
        {
            InGameUIController.Instance.ShowError($"UpdateSeatInfo Error");
            Time.timeScale = 0;
            throw;
        }       
    }   
    public void SetUserAvaterImage(int imageIndex)
    {
        _playerInfoPlateController.SetUserAvaterPhoto(AssetsPoolController.Instance.AvaterMaterials[imageIndex-1]);//imageIndex-1 because the index of image is start from 1
    }
    public virtual void SetHandTiles(List<TileSuits> tileSuits)
    {
        Debug.LogWarning("Must override this function SetHandTiles, Do NOT Use this base function");
    }
    public virtual void SetHandTiles(int tileCount, bool IsDrawing=false)
    {
        Debug.LogWarning("Must override this function SetHandTiles, Do NOT Use this base function");
    }
    public virtual void UpdateHandTiles(List<TileSuits> tileSuits,bool IsDrawing = false)
    {
        Debug.LogWarning("Must override SetHandTiles.UpdateHandTiles, Do NOT Use this base function");
    }
    public virtual void UpdateHandTiles(int tileCount, bool IsDrawing = false)
    {
        Debug.LogWarning("Must override SetHandTiles.UpdateHandTiles(int), Do NOT Use this base function");
    }
    public virtual void Init()
    {
        _flowerTileAreaController.Init();
        _meldsAreaController.Init();       
    }
}
