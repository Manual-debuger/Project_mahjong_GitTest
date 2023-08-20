using Assets.Scripts.UIScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour,IInitiable
{
    private static GameManager _instance;    

    public static GameManager Instance { get { return _instance; } }
    private int _playerIndex;

    [SerializeField] private AbandonedTilesAreaController _abandonedTilesAreaController;
    [SerializeField] private CentralAreaController _centralAreaController;
    [SerializeField] private List<PlayerControllerBase> _playerControllers;
    [SerializeField] private InGameUIController _inGameUIController;
    [SerializeField] private AnimController _animController;
    [SerializeField] private AudioController _audioManager;
    [SerializeField] private API _api;

    public void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
            _instance = this;

        _playerControllers = new List<PlayerControllerBase>
        {
            GameObject.Find("Main_Tiles").GetComponent<PlayerController>(),
            GameObject.Find("Player_Tiles W").GetComponent<CompetitorController>(),
            GameObject.Find("Player_Tiles N").GetComponent<CompetitorController>(),
            GameObject.Find("Player_Tiles E").GetComponent<CompetitorController>()
        };

        _inGameUIController.DiscardTileEvent += OnDiscardTileEvent;
        _inGameUIController.OnTileBeHoldingEvent += OnTileBeHoldingEvent;
        _inGameUIController.LeaveTileBeHoldingEvent += OnLeaveTileBeHoldingEvent;

        _api.RandomSeatEvent += OnRandomSeatEvent;
        _api.DecideBankerEvent += OnDecideBankerEvent;
        _api.OpenDoorEvent += OnOpenDoorEvent;
        _api.GroundingFlowerEvent += OnGroundingFlowerEvent;
        _api.PlayingEvent += OnPlayingEvent;
        _api.WaitingActionEvent += OnWaitingActionEvent;
        _api.PassEvent += OnPassActionEvent;
        _api.DiscardEvent += OnDiscardActionEvent;
        _api.ChowEvent += OnChowActionEvent;
        _api.PongEvent += OnPongActionEvent;
        _api.KongEvent += OnKongActionEvent;
        _api.DrawnEvent += OnDrawnActionEvent;
        _api.GroundingFlowerActionEvent += OnGroundingFlowerActionEvent;
    }

  
    public void Init()
    {
        throw new System.NotImplementedException();
    }
    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int CastAPIIndexToLocalIndex(int seatIndex)
    {
        if (seatIndex > 3) {             Debug.LogError("Error:GameManager.CastAPIIndexToLocalIndex() seatIndex>3");
                   throw new System.Exception("Error:GameManager.CastAPIIndexToLocalIndex() seatIndex>3");
               }
        if ((seatIndex - _playerIndex + 4) % 4>3 || (seatIndex - _playerIndex + 4) % 4 < 0)
            Debug.LogError("Error:GameManager.CastAPIIndexToLocalIndex() result error");
        return (seatIndex - _playerIndex + 4) % 4;
    }

    #region UI Event handle
    private void OnTileBeHoldingEvent(object sender, TileSuitEventArgs e)
    {
        _abandonedTilesAreaController.HighLightDiscardTiles(e.TileSuit);
    }
    private void OnLeaveTileBeHoldingEvent(object sender, TileSuitEventArgs e)
    {
        _abandonedTilesAreaController.UnHighLightDiscardTiles();        
    }


    public void OnDiscardTileEvent(object sender,DiscardTileEventArgs e)
    {
        //Debug.Log("GM");
        _abandonedTilesAreaController.AddTile(e.PlayerIndex, e.TileSuit);
        //throw new System.NotImplementedException();
    }
    public void OnChowTileEvent(object sender, ChowTileEventArgs e)
    {
        _playerControllers[e.PlayerIndex].AddMeldTile(MeldTypes.Sequence, e.TileSuitsList);
        //throw new System.NotImplementedException();
    }
    public void OnPongTileEvent(object sender, PongTileEventArgs e)
    {
        _playerControllers[e.PlayerIndex].AddMeldTile(MeldTypes.Triplet, e.TileSuitsList);
        //throw new System.NotImplementedException();
    }
    public void OnKongTileEvent(object sender, KongTileEventArgs e)
    {
        if(e.IsConcealedKong)
            _playerControllers[e.PlayerIndex].AddMeldTile(MeldTypes.ConcealedQuadplet, e.TileSuitsList);
        else
            _playerControllers[e.PlayerIndex].AddMeldTile(MeldTypes.ExposedQuadplet, e.TileSuitsList);
        //throw new System.NotImplementedException();
    }
    public void OnWinningSuggestEvent(object sender, WinningSuggestArgs e)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    
    #region API handle
    private void OnRandomSeatEvent(object sender, RandomSeatEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnRandomSeatEvent!!!!!!!!!!!!");
        _playerIndex = e.SelfSeatIndex;
        List<string> WindString=new List<string> { "東", "南", "西", "北" };
        try
        {
            for(int i=0;i<4;i++)
            {
                //e.Seats[i].DoorWind = WindString[i];
                
                _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
                _centralAreaController.SetScore(CastAPIIndexToLocalIndex(i), e.Seats[i].Scores);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            throw;
        }
        
        //throw new System.NotImplementedException();
    }

    private void OnDecideBankerEvent(object sender, DecideBankerEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnDecideBankerEvent!!!!!!!!!!!!");
        _centralAreaController.SetBanker(CastAPIIndexToLocalIndex(e.BankerIndex??0),e.RemainingBankerCount??1);
        //throw new System.NotImplementedException();
    }

    private void OnOpenDoorEvent(object sender, OpenDoorEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnOpenDoorEvent!!!!!!!!!!!!");
        //_playerControllers[this._playerIndex].SetHandTiles(e.Tiles);
        _centralAreaController.SetWallCount(e.WallCount ?? -1);
        for (int i = 0; i < e.Seats.Count; i++)
        {
            _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
        }
        //throw new System.NotImplementedException();
    }

    private void OnGroundingFlowerEvent(object sender, GroundingFlowerEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnGroundingFlowerEvent!!!!!!!!!!!!");;
        _centralAreaController.SetWallCount(e.WallCount ?? -1);
        //_playerControllers[this._playerIndex].SetHandTiles(e.Tiles);
        for (int i = 0; i < e.Seats.Count; i++)
        {
            e.Seats[i].SeaTile.Add(TileSuits.c1);
            e.Seats[i].SeaTile.Add(TileSuits.c2);
            _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
        }
        
        //throw new System.NotImplementedException();
    }

    private void OnPlayingEvent(object sender, PlayingEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnPlayingEvent!!!!!!!!!!!!");

        string debugMessage = "PlayingDeadline = " + e.PlayingDeadline;

        _centralAreaController.SetWallCount(e.WallCount ?? -1);
        _playerControllers[this._playerIndex].UpdateHandTiles(e.Tiles, e.PlayingIndex == this._playerIndex);
        try
        {
            for (int i=0;i< e.Seats.Count();i++)
            {
                debugMessage += ", Name: " + e.Seats[i].Nickname + ", Sea Tiles: ";
                foreach (var seaTile in e.Seats[i].SeaTile)
                {
                    debugMessage += seaTile + ", ";
                }

                debugMessage += "Flower Tiles: ";
                foreach (var FlowerTile in e.Seats[i].FlowerTile)
                {
                    debugMessage += FlowerTile + ", ";
                }
                _playerControllers[CastAPIIndexToLocalIndex(i)].UpdateSeatInfo(e.Seats[i]);
                _centralAreaController.SetScore(CastAPIIndexToLocalIndex(i), e.Seats[i].Scores);
            }
            Debug.Log(debugMessage);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
        //throw new System.NotImplementedException();
    }

    private void OnWaitingActionEvent(object sender, WaitingActionEventArgs e)
    {
        Debug.LogWarning("!!!!!!!!!!!!OnWaitingActionEvent!!!!!!!!!!!!");
        Debug.LogWarning($"OnWaitingActionEvent e.Seats.Count={e.Seats.Count}");

        string debugMessage = "PlayingDeadline = " + e.PlayingDeadline;

        _centralAreaController.SetWallCount(e.WallCount ?? -1);
        _playerControllers[this._playerIndex].UpdateHandTiles(e.Tiles, e.PlayingIndex == this._playerIndex);
        try
        {
            for (int i = 0; i < e.Seats.Count(); i++)
            {
                debugMessage += ", Name: " + e.Seats[i].Nickname + ", Sea Tiles: ";
                foreach (var seaTile in e.Seats[i].SeaTile)
                {
                    debugMessage += seaTile + ", ";
                }

                debugMessage += "Flower Tiles: ";
                foreach (var FlowerTile in e.Seats[i].FlowerTile)
                {
                    debugMessage += FlowerTile + ", ";
                }
                _playerControllers[CastAPIIndexToLocalIndex(i)].UpdateSeatInfo(e.Seats[i]);
                _centralAreaController.SetScore(CastAPIIndexToLocalIndex(i), e.Seats[i].Scores);
            }
            Debug.Log(debugMessage);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }

        //throw new System.NotImplementedException();
    }
    
    private void OnPassActionEvent(object sender, PassActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnPassActionEvent!!!!!!!!!!!!");
        //throw new System.NotImplementedException();
    }
    
    private void OnDiscardActionEvent(object sender, DiscardActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnDiscardActionEvent!!!!!!!!!!!!");
        if (e.Options.Count > 1)
            Debug.LogWarning("Options of DiscardActionEventArgs more than 1");       
        //_playerControllers[CastAPIIndexToLocalIndex(e.Index)].DiscardTile(e.Options[0]);
        //throw new System.NotImplementedException();
    }
    
    private void OnChowActionEvent(object sender, ChowActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnChowActionEvent!!!!!!!!!!!!");
        //throw new System.NotImplementedException();
    }
    
    private void OnPongActionEvent(object sender, PongActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnPongActionEvent!!!!!!!!!!!!");
        //throw new System.NotImplementedException();
    }
    
    private void OnKongActionEvent(object sender, KongActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnKongActionEvent!!!!!!!!!!!!");
        //throw new System.NotImplementedException();
    }
    
    private void OnDrawnActionEvent(object sender, DrawnActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnDrawnActionEvent!!!!!!!!!!!!");        
        //throw new System.NotImplementedException();
    }
    
    private void OnGroundingFlowerActionEvent(object sender, GroundingFlowerActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnGroundingFlowerActionEvent!!!!!!!!!!!!");
        //throw new System.NotImplementedException();
    }
    #endregion
}