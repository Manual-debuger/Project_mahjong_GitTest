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
using Assets.Scripts;

public class GameManager : MonoBehaviour,IInitiable
{
    private static GameManager _instance;    

    public static GameManager Instance { get { return _instance; } }
    private int _playerIndex;
    private bool _isGameStart = false;
    private float _chatGPTTime = 0;
    [SerializeField] private AbandonedTilesAreaController _abandonedTilesAreaController;
    [SerializeField] private CentralAreaController _centralAreaController;
    [SerializeField] private List<PlayerControllerBase> _playerControllers;
    [SerializeField] private InGameUIController _inGameUIController;
    [SerializeField] private EffectController _effectController;
    [SerializeField] private AudioController _audioController;
    

    public void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
        {
            _instance = this;
            if(_effectController==null)
                _effectController = GameObject.Find("EffectController").GetComponent<EffectController>();
            if(_audioController==null)
                _audioController = GameObject.Find("AudioController").GetComponent<AudioController>();
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

            APIData.WaitingEvent += OnWaitingEvent;
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
            APIData.ReadyHandEvent += OnReadyHandActionEvent;
            APIData.WinEvent += OnWinActionEvent;
            APIData.DrawnEvent += OnDrawnActionEvent;
            APIData.GroundingFlowerActionEvent += OnGroundingFlowerActionEvent;
            APIData.DrawnFromDeadWallActionEvent += OnDrawnFromDeadWallActionEvent;
            APIData.SelfDrawnWinActionEvent += OnSelfDrawnWinActionEvent;
            APIData.ResultEvent += OnResultEvent;
        }
    }


    public void Init()
    {
        foreach (PlayerControllerBase playerController in _playerControllers)
        {
            playerController.Init();
        }
        _abandonedTilesAreaController.Init();
        _centralAreaController.Init();
        //_inGameUIController.Init();
        //_effectController.Init();
        //_audioController.Init();
    }
    // Start is called before the first frame update
    void Start()
    {       

    }

    // Update is called once per frame
    async void Update()
    {
        if(_isGameStart)
        {
            if (_chatGPTTime > 0)
            {
                _chatGPTTime-=Time.deltaTime;
            }
            else
            {
                _chatGPTTime = 30;                
                var characterList = new List<Tuple<string, string>>();
                characterList.Add(new Tuple<string, string>("蘇珊娜", "蘇珊娜是一個擁有無限幽默感的女性，她總是能夠找到場合適當的搞笑元素，並擅長以幽默和諷刺的方式吐槽事情。她能夠在社交場合中輕松拉近人與人之間的距離，並讓每個人都感到開心。她的幽默感和機智使她成為派對的焦點，但她也會在必要時提出有趣的反駁和觀點。"));
                characterList.Add(new Tuple<string, string>("艾利", "艾莉是一個充滿冒險精神的女性，她總是願意嘗試新的事物，探索未知領域，並經歷生活中的刺激瞬間。她經常參加極限運動、旅行冒險、或參與挑戰性的活動，不怕風險，喜歡挑戰自己。她的行為可能會顯得有些冒失，但她的勇氣和樂觀精神讓人難以抗拒。"));
                var moodsList = new List<string> { "開心", "平靜" };
                var directionsList = new List<string> { "東", "南" };
                var currentScoresList = new List<int> { 1000, 2000 };
                var promptFactors = new PromptFactors(characterList,moodsList,directionsList,currentScoresList,13);
                Debug.Log("CallChatGPT");                
                Result result=await ChatGPTTool.CallChatGPT(ChatGPTTool.GeneratePrompt(PromptType.TwoManChat,promptFactors));
                Debug.Log("CallChatGPT result=" + result.choices[0].message.content);                
                _inGameUIController.AddChat(ChatGPTTool.Parsing(result.choices[0].message.content));                
                
                //Debug.Log("CallChatGPT testresult=" + testresult.choices[0].message.content);
                //_inGameUIController.AddChat(new List<Tuple<string, string>>() {new Tuple<string,string>("Test", testresult.choices[0].message.content) });
            }
        }
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
        APIData.instance.HandleClickAction(actionData);
        //throw new System.NotImplementedException();
    }
    #endregion

    #region API handle
    private void OnWaitingEvent(object sender, WaitingEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnWaitingEvent!!!!!!!!!!!!");
        _inGameUIController.setWaiting(e);
        
    }
    
    private void OnRandomSeatEvent(object sender, RandomSeatEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnRandomSeatEvent!!!!!!!!!!!!");
        _isGameStart = true;
        _playerIndex = e.SelfSeatIndex;
        List<string> WindString=new List<string> { "東", "南", "西", "北" };
        try
        {
            for(int i = 0;i < e.Seats.Count; i++)
            {
                //e.Seats[i].DoorWind = WindString[i];
                
                _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
                _centralAreaController.SetScore(CastAPIIndexToLocalIndex(i), e.Seats[i].Scores);
                if(i == _playerIndex)
                {
                    if (e.Seats[i].AutoPlaying != null && (bool)e.Seats[i].AutoPlaying)
                    {
                        _inGameUIController.Hosting();
                    }
                    else
                    {
                        _inGameUIController.CancelHosting();
                    }
                }
            }
            _inGameUIController.ShowState("RandomSeat", 500);
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
        _centralAreaController.SetBanker(CastAPIIndexToLocalIndex(e.BankerIndex),e.RemainingBankerCount??1);
        _inGameUIController.ShowState("DecideBanker", 500);

        for (int i = 0; i < 4; i++)
        {
            if (i == _playerIndex)
            {
                if (e.Seats[i].AutoPlaying != null && (bool)e.Seats[i].AutoPlaying)
                {
                    _inGameUIController.Hosting();
                }
                else
                {
                    _inGameUIController.CancelHosting();
                }
            }
        }
        //throw new System.NotImplementedException();
    }

    private void OnOpenDoorEvent(object sender, OpenDoorEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnOpenDoorEvent!!!!!!!!!!!!");
        try
        {
            _centralAreaController.SetWallCount(e.WallCount);
            for (int i = 0; i < e.Seats.Count; i++)
            {
                _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
                if (i == _playerIndex)
                {
                    if (e.Seats[i].AutoPlaying != null && (bool)e.Seats[i].AutoPlaying)
                    {
                        _inGameUIController.Hosting();
                    }
                    else
                    {
                        _inGameUIController.CancelHosting();
                    }
                }
            }
            _playerControllers[CastAPIIndexToLocalIndex(this._playerIndex)].SetHandTiles(e.Tiles);
            _inGameUIController.ShowState("OpenDoor", 500);
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
            _centralAreaController.SetWallCount(e.WallCount);
            for (int i = 0; i < e.Seats.Count; i++)
            {
                e.Seats[i].SeaTile.Add(TileSuits.c1);
                e.Seats[i].SeaTile.Add(TileSuits.c2);
                _playerControllers[CastAPIIndexToLocalIndex(i)].SetSeatInfo(e.Seats[i]);
                if (i == _playerIndex)
                {
                    if (e.Seats[i].AutoPlaying != null && (bool)e.Seats[i].AutoPlaying)
                    {
                        _inGameUIController.Hosting();
                    }
                    else
                    {
                        _inGameUIController.CancelHosting();
                    }
                }
            }
            _playerControllers[CastAPIIndexToLocalIndex(this._playerIndex)].SetHandTiles(e.Tiles);
            _inGameUIController.ShowState("GroundingFlower", 500);
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

        string debugMessage = "PlayingDeadline = " + e.PlayingTimeLeft;

        try
        {
            //CentralArea
            _centralAreaController.SetWallCount(e.WallCount);
            _centralAreaController.SetHighLightBar(CastAPIIndexToLocalIndex(e.PlayingIndex));

            _playerControllers[CastAPIIndexToLocalIndex(_playerIndex)].SetHandTiles(e.Tiles);
            
            _inGameUIController.ActionUISet(e.Actions ?? new ActionData[0], e.PlayingTimeLeft);
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

                if (i == _playerIndex)
                {
                    if (e.Seats[i].AutoPlaying != null && (bool)e.Seats[i].AutoPlaying)
                    {
                        _inGameUIController.Hosting();
                    }
                    else
                    {
                        _inGameUIController.CancelHosting();
                    }
                }
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

        string debugMessage = "PlayingDeadline = " + e.PlayingTimeLeft;

        try
        {
            //CentralArea
            _centralAreaController.SetWallCount(e.WallCount);
            _centralAreaController.SetHighLightBar(CastAPIIndexToLocalIndex(e.PlayingIndex));

            _playerControllers[CastAPIIndexToLocalIndex(_playerIndex)].SetHandTiles(e.Tiles);
            
            _inGameUIController.ActionUISet(e.Actions ?? new ActionData[0], e.PlayingTimeLeft);

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

                if (i == _playerIndex)
                {
                    if (e.Seats[i].AutoPlaying != null && (bool)e.Seats[i].AutoPlaying)
                    {
                        _inGameUIController.Hosting();
                    }
                    else
                    {
                        _inGameUIController.CancelHosting();
                    }
                }
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

    //打完一局(胡牌/流局)
    private void OnHandEndEvent(object sender, HandEndEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnHandEndEvent!!!!!!!!!!!!");
            //_inGameUIController.Settlement(e.Seats, e.PlayingTimeLeft);
            _inGameUIController.SettlementSetCloseTime(e.PlayingTimeLeft);
            Init();
            _inGameUIController.CancelHosting();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
    }
    //打完一圈
    private void OnGameEndEvent(object sender, GameEndEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnGameEndEvent!!!!!!!!!!!!"); ;
            //_inGameUIController.Settlement(e.Seats, e.PlayingTimeLeft);
            _inGameUIController.SettlementSetCloseTime(e.PlayingTimeLeft);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
    }
    //最後一步
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

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.Chow);
        //throw new System.NotImplementedException();
    }
    
    private void OnPongActionEvent(object sender, PongActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnPongActionEvent!!!!!!!!!!!!");
        //Effect
        Instance._effectController.PlayEffect(EffectID.Pong, CastAPIIndexToLocalIndex(e.Index));

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.Pong);
        //throw new System.NotImplementedException();
    }
    
    private void OnKongActionEvent(object sender, KongActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnKongActionEvent!!!!!!!!!!!!");
        //Effect
        Instance._effectController.PlayEffect(EffectID.Kong, CastAPIIndexToLocalIndex(e.Index));

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.Kong);
        //throw new System.NotImplementedException();
    }
    
    private void OnReadyHandActionEvent(object sender, ReadyHandActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnReadyHandActionEvent!!!!!!!!!!!!");
        //Effect
        Instance._effectController.PlayEffect(EffectID.Listen, CastAPIIndexToLocalIndex(e.Index));

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.ReadyHand);
        //throw new System.NotImplementedException();
    }
    
    private void OnWinActionEvent(object sender, WinActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnWinActionEvent!!!!!!!!!!!!");
        //Effect
        Instance._effectController.PlayEffect(EffectID.Win, CastAPIIndexToLocalIndex(e.Index));

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.Win);
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

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.GroundingFlower);
        //throw new System.NotImplementedException();
    }
    
    private void OnDrawnFromDeadWallActionEvent(object sender, DrawnFromDeadWallActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnDrawnFromDeadWallActionEvent!!!!!!!!!!!!");

        //Effect
        //Instance._effectController.PlayEffect(EffectID.DrawnFromDeadWall, CastAPIIndexToLocalIndex(e.Index));

        //Audio
        //Instance._audioController.PlayAudioEffect(AudioType.DrawnFromDeadWall);
        //throw new System.NotImplementedException();
    }
    
    private void OnSelfDrawnWinActionEvent(object sender, SelfDrawnWinActionEventArgs e)
    {
        Debug.Log("!!!!!!!!!!!!OnSelfDrawnActionEvent!!!!!!!!!!!!");

        //Effect
        //Instance._effectController.PlayEffect(EffectID.SelfDrawn, CastAPIIndexToLocalIndex(e.Index));

        //Audio
        Instance._audioController.PlayAudioEffect(AudioType.SelfDrawn);
        //throw new System.NotImplementedException();
    }

    // 結算資訊
    private void OnResultEvent(object sender, ResultEventArgs e)
    {
        try
        {
            Debug.Log("!!!!!!!!!!!!OnResultEvent!!!!!!!!!!!!");
            string debugMessage = "";
            for (int i = 0; i < e.PlayerResult.Count; i++)
            {
                debugMessage += "Name: " + e.PlayerResult[i].Name;
                debugMessage += ", Door Tiles: ";
                foreach (var doorTiles in e.PlayerResult[i].DoorTile)
                {
                    foreach (var doorTile in doorTiles)
                    {
                        debugMessage += doorTile + ", ";
                    }
                }
                
                debugMessage += "Tiles: ";
                foreach (var tile in e.PlayerResult[i].Tile)
                {
                    debugMessage += tile + ", ";
                }

                debugMessage += "Flower Tiles: ";
                foreach (var FlowerTile in e.PlayerResult[i].FlowerTile)
                {
                    debugMessage += FlowerTile + ", ";
                }
            }
            Debug.Log(debugMessage);

            _inGameUIController.Settlement(e.PlayerResult);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            throw;
        }
    }
    #endregion
}