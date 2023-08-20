using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts.UIScripts
{
    public class CompetitorController : PlayerControllerBase
    {
        [SerializeField] private HandTilesAreaController _handTilesAreaController;
        [SerializeField] private DrawedTileAreaController _drawedTileAreaController;
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void Init()
        {
            _handTilesAreaController.Init();
            _drawedTileAreaController.Init();          
            base.Init();
        }
        public override void SetHandTiles(int tileCount, bool IsDrawing = false)
        {
            _drawedTileAreaController.Init();
            if(IsDrawing)
            {
                _drawedTileAreaController.AddTile(TileSuits.b1);
                tileCount--;
            }
            if (_handTilesAreaController.GetTileSuits().Length == tileCount)
                return;
            else 
            {                
                _handTilesAreaController.SetTiles(tileCount);
            }
        }
        public override void SetHandTiles(List<TileSuits> tileSuits, bool IsDrawing=false)
        {
            _drawedTileAreaController.Init();
            if (IsDrawing)
            {
                _drawedTileAreaController.AddTile(tileSuits.Last());    
                tileSuits.RemoveAt(tileSuits.Count-1);
            }
            if (_handTilesAreaController.GetTileSuits().Length == tileSuits.Count)
                return;
            else
            {
                _handTilesAreaController.SetTiles(tileSuits);
            }                       
        }
        public override void SetSeatInfo(SeatInfo seatInfo)
        {
            try
            {
                base.SetSeatInfo(seatInfo);
                if (seatInfo.TileCount != null && seatInfo.TileCount > 16)
                {
                    _drawedTileAreaController.SetTiles(1);
                    _handTilesAreaController.SetTiles(16);
                }
                else
                {
                    _drawedTileAreaController.Init();
                    _handTilesAreaController.SetTiles(seatInfo.TileCount ?? 3);
                }
            }
            catch (System.Exception)
            {

                throw;
            }   
            
        }
        public override void UpdateSeatInfo(SeatInfo seatInfo)
        {
            base.SetSeatInfo(seatInfo);
            if(seatInfo.TileCount!=null && seatInfo.TileCount>16)
            {
                _drawedTileAreaController.SetTiles(1);
                _handTilesAreaController.SetTiles(16);
            }
            else
            {
                _drawedTileAreaController.Init();
                _handTilesAreaController.SetTiles(seatInfo.TileCount??3);
            }
            
        }
        public override void AddDrawedTile(TileSuits tileSuit)
        {
            _drawedTileAreaController.AddTile(tileSuit);
        }        
        public override void DiscardTile(TileSuits tileSuit)
        {
            _drawedTileAreaController.PopLastTile();
            _seaTilesAreaController.AddTile(tileSuit);
            
        }
        /*public override void RemoveHandTile(TileSuits tileSuit)
        {
            _handTilesAreaController.PopLastTile();
        }*/
    }
}