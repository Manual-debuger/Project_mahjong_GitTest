using Assets.Scripts.UIScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using APIDataNamespace;

public class GameManager : MonoBehaviour,IInitiable
{
    private static GameManager _instance;    

    public static GameManager Instance { get { return _instance; } }
    private int _playerIndex;

    [SerializeField] private AbandonedTilesAreaController _abandonedTilesAreaController;
    [SerializeField] private CentralAreaController _centralAreaController;
    [SerializeField] private List<PlayerControllerBase> _playerControllers;
    [SerializeField] private InGameUIController _inGameUIController;
    [SerializeField] private EffectController _effectController;
    [SerializeField] private AudioController _audioManager;

    public void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
        {
            _instance = this;
            if(_effectController==null)
                _effectController = GameObject.Find("EffectController").GetComponent<EffectController>();
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
            //UIActiveActionEvent=UI需要傳給伺服器的事件 ex.吃碰槓
            _inGameUIController.UIActiveActionEvent+=OnUIActiveActionEvent;

            APIData.RandomSeatEvent += OnRandomSeatEvent;
            APIData.DecideBankerEvent += OnDecideBankerEvent;
            APIData.OpenDoorEvent += OnOpenDoorEvent;
            APIData.GroundingFlowerEvent += OnGroundingFlowerEvent;
            APIData.PlayingEvent += OnPlayingEvent;
            APIData.WaitingActionEvent += OnWaitingActionEvent;
            APIData.HandEndEvent += OnHandEndEvent;
            APIData.GameEndEvent += OnGameEndEvent;
            APIData.ClosingEvent += OnClosingEvent;
            APIData.PassEvent += OnPassActionEvent;
            APIData.DiscardEvent += OnDiscardActionEvent;
            APIData.ChowEvent += OnChowActionEvent;
            APIData.PongEvent += OnPongActionEvent;
            APIData.KongEvent += OnKongActionEvent;
            APIData.DrawnEvent += OnDrawnActionEvent;
            APIData.GroundingFlowerActionEvent += OnGroundingFlowerActionEvent;
        }
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
        if (seatIndex > 3) {            
            Debug.LogError("Error:GameManager.CastAPIIndexToLocalIndex() seatIndex>3");
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
        _playerControllers[e.PlayerIndex].AddMeldTile(e.TileSuitsList);
        //throw new System.NotImplementedException();
    }
    public void OnPongTileEvent(object sender, PongTileEventArgs e)
    {
        _playerControllers[e.PlayerIndex].AddMeldTile(e.TileSuitsList);
        //throw new System.NotImplementedException();
    }
    public void OnKongTileEvent(object sender, KongTileEventArgs e)
    {
        if(e.IsConcealedKong)
            _playerControllers[e.PlayerIndex].AddMeldTile(e.TileSuitsList);
        else
            _playerControllers[e.PlayerIndex].AddMeldTile(e.TileSuitsList);
        //throw new System.NotImplementedException();
    }
    public void OnWinningSuggestEvent(object sender, WinningSuggestArgs e)
    {
        throw new System.NotImplementedException();
    }

    public void OnUIActiveActionEvent(object sender,ActionData actionData)
    {        
        APIData.instance.HandleClickAction(actionData,this._playerIndex);
        //throw new System.NotImplementedException();
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
        try
        {
            _centralAreaController.SetWallCount(e.WallCount ?? -1);
            for (int i = 0; i < e.Seats.Count; i++)
            {
                _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
            }
            _playerControllers[CastAPIIndexToLocalIndex(this._playerIndex)].SetHandTiles(e.Tiles, e.Tiles.Count==17);
        }
        catch (Exception)
        {

            throw;
        }
        //throw new System.NotImplementedException();
    }

    private void OnGroundingFlowerEvent(object sender, GroundingFlowerEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnGroundingFlowerEvent!!!!!!!!!!!!"); ;
            _centralAreaController.SetWallCount(e.WallCount ?? -1);
            for (int i = 0; i < e.Seats.Count; i++)
            {
                e.Seats[i].SeaTile.Add(TileSuits.c1);
                e.Seats[i].SeaTile.Add(TileSuits.c2);
                _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
            }
            _playerControllers[CastAPIIndexToLocalIndex(this._playerIndex)].SetHandTiles(e.Tiles);
        }
        catch (Exception)
        {

            throw;
        }
        
        //throw new System.NotImplementedException();
    }

    private void OnPlayingEvent(object sender, PlayingEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnPlayingEvent!!!!!!!!!!!!");

        string debugMessage = "PlayingDeadline = " + e.PlayingDeadline;

        try
        {
            _centralAreaController.SetWallCount(e.WallCount ?? -1);
            _playerControllers[CastAPIIndexToLocalIndex(this._playerIndex)].SetHandTiles(e.Tiles, e.PlayingIndex == this._playerIndex);
            for (int i=0;i< e.Seats.Count();i++)
            {
                debugMessage += ", Name: " + e.Seats[i].Nickname;

                debugMessage += "Door Tiles: ";
                foreach (var DoorTile in e.Seats[i].DoorTile) { debugMessage += DoorTile + ", "; }

                debugMessage += ", Sea Tiles: ";
                foreach (var seaTile in e.Seats[i].SeaTile) { debugMessage += seaTile + ", "; }

                debugMessage += "Flower Tiles: ";
                foreach (var FlowerTile in e.Seats[i].FlowerTile) { debugMessage += FlowerTile + ", "; }

                _playerControllers[CastAPIIndexToLocalIndex(i)].UpdateSeatInfo(e.Seats[i]);
                _centralAreaController.SetScore(CastAPIIndexToLocalIndex(i), e.Seats[i].Scores);

                
            }

            //Effect
            _effectController.StopAllEffects();
            //if (e.PlayingIndex != null && e.Actions != null)
            //{
            //    switch (e.Actions[0].ID)
            //    {
            //        case Action.Chow:
            //            _effectController.PlayEffect(EffectID.Chow, CastAPIIndexToLocalIndex(e.PlayingIndex ?? 0));
            //            break;
            //        case Action.Pong:
            //            _effectController.PlayEffect(EffectID.Pong, CastAPIIndexToLocalIndex(e.PlayingIndex ?? 0));
            //            break;
            //        case Action.Kong:
            //        case Action.ConcealedKong:
            //        case Action.AdditionKong:
            //            _effectController.PlayEffect(EffectID.Kong, CastAPIIndexToLocalIndex(e.PlayingIndex ?? 0));
            //            break;
            //        case Action.Discard:
            //        case Action.Pass:
            //        case Action.Win:
            //        case Action.SelfDrawnWin:
            //        case Action.Drawn:
            //        case Action.ReadyHand:
            //            //_effectController.PlayEffect(EffectID.Win, CastAPIIndexToLocalIndex(e.PlayingIndex ?? 0));
            //            Debug.LogWarning("Not Implemented this Effect yet");
            //            break;
            //        default:
            //            Debug.LogWarning("Not Implemented this Effect yet");
            //            break;
            //    }
            //}
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

        try
        {
            _centralAreaController.SetWallCount(e.WallCount ?? -1);
            _playerControllers[CastAPIIndexToLocalIndex(_playerIndex)].SetHandTiles(e.Tiles);
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
    
    private void OnHandEndEvent(object sender, HandEndEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnHandEndEvent!!!!!!!!!!!!");
            
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
    }
    
    private void OnGameEndEvent(object sender, GameEndEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnGameEndEvent!!!!!!!!!!!!"); ;
            
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
    }
    
    private void OnClosingEvent(object sender, ClosingEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnClosingEvent!!!!!!!!!!!!"); ;
            
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
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
        //Effect
        Instance._effectController.PlayEffect(EffectID.Chow, CastAPIIndexToLocalIndex(e.Index));
        //throw new System.NotImplementedException();
    }
    
    private void OnPongActionEvent(object sender, PongActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnPongActionEvent!!!!!!!!!!!!");
        //Effect
        Instance._effectController.PlayEffect(EffectID.Pong, CastAPIIndexToLocalIndex(e.Index));
        //throw new System.NotImplementedException();
    }
    
    private void OnKongActionEvent(object sender, KongActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnKongActionEvent!!!!!!!!!!!!");
        //Effect
        Instance._effectController.PlayEffect(EffectID.Kong, CastAPIIndexToLocalIndex(e.Index));
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