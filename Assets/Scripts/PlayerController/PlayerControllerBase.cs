using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
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
        try { _playerInfoPlateController.SetUserName(seatInfo.Nickname); } catch { Debug.LogWarning("SetUserName Wrong"); throw; }
        try { _playerInfoPlateController.SetWindPosision(seatInfo.DoorWind.ToString()); } catch { Debug.LogWarning("SetWindPosision Wrong"); throw; }
        try { _seaTilesAreaController.SetTiles(seatInfo.SeaTile); } catch { Debug.LogWarning("SetSeaTiles Wrong");throw;}
        try { _flowerTileAreaController.SetTiles(seatInfo.FlowerTile); } catch { Debug.LogWarning("SetFlowerTiles Wrong");throw; }     
        try { _meldsAreaController.SetDoors(seatInfo.DoorTile); } catch { Debug.LogWarning("SetDoors Wrong");throw; }
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
            throw;
        }
        //try
        //{
        //    List<TileSuits> TestseaTile = new List<TileSuits>();
        //    TestseaTile.Add(TileSuits.c1);
        //    TestseaTile.Add(TileSuits.c2);
        //    Debug.LogWarning($"UpdateSeatInfo(seatinfo) seatinfo.Seatile:{seatInfo.SeaTile}");
        //    _seaTilesAreaController.UpdateTiles(seatInfo.SeaTile);
        //    _flowerTileAreaController.UpdateTiles(seatInfo.FlowerTile);
        //}
        //catch
        //{
        //    Debug.LogError("UpdateSeatInfo Error");
        //    throw;
        //}
    }   
    public virtual void SetHandTiles(List<TileSuits> tileSuits,bool IsDrawing = false)
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
