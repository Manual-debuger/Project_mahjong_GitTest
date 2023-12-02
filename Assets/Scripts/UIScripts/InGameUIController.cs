using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DataTransformNamespace;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;

//Duty: 遊戲中的UI控制器
public class InGameUIController : MonoBehaviour
{
    private static InGameUIController _instance;
    public static InGameUIController Instance { get { return _instance; } }
    [SerializeField] private HandTilesUI _handTilesUIViewer;
    [SerializeField] private ListeningHolesUI _meldTilesUIViewer;
    [SerializeField] private SettingUIButton _settingUIButton;
    [SerializeField] private DiscardTileUI _discardTileUIViewer;
    [SerializeField] private SocialUIButton _socialUIButton;
    [SerializeField] private SupportUIButton _supportUIButton;
    [SerializeField] private WinningSuggestUI _winningSuggestUIViewer;
    [SerializeField] private SettlementScreen _settlementScreen;
    [SerializeField] private CancelHosting _cancelHosting;
    [SerializeField] private Hosting _hosting;
    [SerializeField] private WaitingUI _waitingUI;
    [SerializeField] private HintUI _hintUI;

    [SerializeField] private GameObject InGameUI;
    [SerializeField] private GameObject SettlementUI;
    [SerializeField] private GameObject WaitingUIObject;
    public event EventHandler<DiscardTileEventArgs> DiscardTileEvent;
    public event EventHandler<TileSuitEventArgs> OnTileBeHoldingEvent;
    public event EventHandler<TileSuitEventArgs> LeaveTileBeHoldingEvent;
    public event EventHandler<FloatEventArgs> SetMusicEvent;
    public event EventHandler<FloatEventArgs> SetSoundEvent;
    public event EventHandler<ActionData> UIActiveActionEvent;

    public int count = 0;
    public List<string> PlayerName;
    public List<int> AvatarIndex;
    public bool IsListenState = false;
    public bool IsDiscardState = false;
    public bool isDraw = true;
    public bool[] CanDiscardList = new bool[17];
    private ActionData[] Actions;
    private Dictionary<TileSuits, Dictionary<TileSuits, int>> readyInfo;
    public List<TileSuits> HandTileSuits = new()
    {
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL,
        TileSuits.NULL
    };

    private long CountTime;
    [SerializeField] private GameObject State;
    [SerializeField] private Image StateImage;
    [SerializeField] private TMP_Text StateText;
    [SerializeField] private TMP_Text ErrorText;
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else if (_instance == null)
            _instance = this;

        _handTilesUIViewer.DiscardTileEvent += DiscardTile;
        _handTilesUIViewer.OnPointerDownEvent += OnDiscardTileSuggestEvent;
        _handTilesUIViewer.OnPointerUpEvent += LeaveDiscardTileSuggestEvent;
        _settingUIButton.SetMusicEvent += SetMusic;
        _settingUIButton.SetSoundEvent += SetSound;
        _waitingUI.CloseWaitingEvent += CloseWaiting;
        isDraw = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        HandTileSort();
        HandTileUISet();
    }

    // Update is called once per frame
    void Update()
    {
        CountTime -= (long)(Time.deltaTime * 1000);
        if (CountTime < 0)
        {
            UIReset();
            _discardTileUIViewer.SetTime(0);
        }
        else
        {
            _discardTileUIViewer.SetTime((int)(CountTime/1000));
        }
    }

    private void DiscardTile(object sender, TileIndexEventArgs e)
    {
        //Debug.Log("UI");
        if (CanDiscardList[e.TileIndex])
        {
            ActionData DiscardTileInfo = new ActionData();
            if (IsDiscardState)
            {
                DiscardTileInfo.ID = Action.Discard;
                ListenOff();
            }
            if (IsListenState)
            {
                DiscardTileInfo.ID = Action.ReadyHand;
                DiscardTileUIViewer.SetListenOptionOff();
                _handTilesUIViewer.ListenSetOff();
            }
            DiscardTileInfo.OptionTiles = new List<List<TileSuits>>();
            DiscardTileInfo.OptionTiles.Add(new List<TileSuits> { HandTileSuits[e.TileIndex] });
            HandTileSuits[e.TileIndex] = HandTileSuits[16];
            HandTileSuits[16] = TileSuits.NULL;

            HandTileSort();
            HandTileUISet();

            _discardTileUIViewer.ActionUISetOff();
            IsListenState = false;
            IsDiscardState = false;
            _handTilesUIViewer.SetBright();
            for (int i = 0; i < 17; i++)
            {
                CanDiscardList[i] = false;
            }
            UIActiveActionEvent?.Invoke(this, DiscardTileInfo);
        }
    }
    private void OnDiscardTileSuggestEvent(object sender, TileIndexEventArgs e)
    {
        if (IsListenState)
        {
            TileSuits tile = HandTileSuits[e.TileIndex];
            foreach (KeyValuePair<TileSuits, Dictionary<TileSuits, int>> key in readyInfo)
            {
                if (tile == key.Key)
                {
                    _discardTileUIViewer.SetListenTileSuggest(key.Value);
                    break;
                }
            }
        }
        OnTileBeHoldingEvent?.Invoke(this, new TileSuitEventArgs(HandTileSuits[e.TileIndex]));
    }
    private void LeaveDiscardTileSuggestEvent(object sender, TileIndexEventArgs e)
    {
        //Debug.Log("LeaveDiscardTileSuggestEvent");
        if (IsListenState)
        {
            _discardTileUIViewer.CloseListenTileSuggest();
        }
        LeaveTileBeHoldingEvent?.Invoke(this, new TileSuitEventArgs(HandTileSuits[e.TileIndex]));

    }
    public void HandTileSort()
    {
        List<TileSuits> sublist = HandTileSuits.GetRange(0, 16);

        sublist.Sort(new Comparison<TileSuits>((x, y) => x.CompareTo(y)));
        sublist.RemoveAll(x => x == TileSuits.NULL);
        while (sublist.Count < 16)
        {
            sublist.Insert(0, TileSuits.NULL);
        }

        HandTileSuits.RemoveRange(0, 16);
        HandTileSuits.InsertRange(0, sublist);
        //HandTileSuits.Sort(new Comparison<TileSuits>((x, y) => x.CompareTo(y)));
    }
    public void HandTileUISet()
    {
        for (int i = 0; i < 17; i++)
        {
            _handTilesUIViewer.HandTileSet(i, HandTileSuits[i]);
        }
    }
    public void SetHandTile(List<TileSuits> tileSuits)
    {
        if (tileSuits.Count % 3 == 2 && isDraw)
        {
            HandTileSuits[16] = tileSuits[tileSuits.Count - 1];
            tileSuits.RemoveAt(tileSuits.Count - 1);
        }
        else
        {
            HandTileSuits[16] = TileSuits.NULL;
            isDraw = true;
        }

        for (int i = 0; i < tileSuits.Count; i++)
        {
            HandTileSuits[16 - tileSuits.Count + i] = tileSuits[i];
        }
        for (int i = 0; i < 16 - tileSuits.Count; i++)
        {
            HandTileSuits[i] = TileSuits.NULL;
        }
        HandTileUISet();
    }
    private void SetMusic(object sender, FloatEventArgs e)
    {
        //Debug.Log(e.f);
        SetMusicEvent?.Invoke(this, e);
    }
    private void SetSound(object sender, FloatEventArgs e)
    {
        //Debug.Log(e.f);
        SetSoundEvent?.Invoke(this, e);
    }
    public void ActionUISet(ActionData[] actions, long time)
    {
        UIReset();
        Actions = actions;
        foreach (ActionData action in actions)
        {
            switch (action.ID)
            {
                case Action.Pass:
                    _discardTileUIViewer.SetPassOn();
                    break;
                case Action.Discard:
                    for (int i = 0; i < 17; i++)
                    {
                        CanDiscardList[i] = false;
                    }
                    foreach (List<TileSuits> tileSuits in action.OptionTiles)
                    {
                        foreach (TileSuits tileSuit in tileSuits)
                        {
                            for (int i = 0; i < 17; i++)
                            {
                                if (HandTileSuits[i] == tileSuit)
                                {
                                    CanDiscardList[i] = true;
                                }
                            }
                        }
                    }
                    _handTilesUIViewer.DiscardTileSet(CanDiscardList);
                    IsDiscardState = true;
                    break;
                case Action.Chow:
                    _discardTileUIViewer.SetChowOn();
                    break;
                case Action.Pong:
                    _discardTileUIViewer.SetPongOn();
                    break;
                case Action.Kong:
                case Action.AdditionKong:
                case Action.ConcealedKong:
                    _discardTileUIViewer.SetKongOn();
                    break;
                case Action.ReadyHand:
                    _discardTileUIViewer.SetListenOn();
                    break;
                case Action.Win:
                case Action.DrawnFromDeadWall:
                case Action.SelfDrawnWin:
                    _discardTileUIViewer.SetWinningOn();
                    break;
                default:
                    break;
            }
        }
        CountTime = time;
    }
    //public async void Settlement(List<SeatInfo> seatInfos,long time)
    //{
    //    InGameUI.SetActive(false);
    //    SettlementUI.SetActive(true);
    //    _settlementScreen.SetSettlement(seatInfos, time);

    //    await Task.Delay((int)time);
    //    InGameUI.SetActive(true);
    //    SettlementUI.SetActive(false);
    //    for (int i = 0; i < 17; i++)
    //    {
    //        HandTileSuits[i] = TileSuits.NULL;
    //    }
    //    HandTileUISet();
    //    UIReset();
    //    CountTime = 0;
    //}
    public void Settlement(List<PlayerResultData> playerResultDatas)
    {
        InGameUI.SetActive(false);
        SettlementUI.SetActive(true);
        if (playerResultDatas != null)
            _settlementScreen.SetSettlement(playerResultDatas,PlayerName,AvatarIndex);
    }

    public async void SettlementSetCloseTime(long time)
    {
        _settlementScreen.SetTime(time);

        await Task.Delay((int)time);
        InGameUI.SetActive(true);
        SettlementUI.SetActive(false);
        for (int i = 0; i < 17; i++)
        {
            HandTileSuits[i] = TileSuits.NULL;
        }
        HandTileUISet();
        UIReset();
        CountTime = 0;
    }
    public async void ShowState(string State, long time)
    {
        this.StateText.text = State;
        StartCoroutine(StateAppear());
        await Task.Delay(1500);
        StartCoroutine(StateDisappear());

        //this.StateName.SetActive(true);
        //await Task.Delay((int)time);
        //this.StateName.SetActive(false);
    }

    IEnumerator StateAppear()
    {
        State.SetActive(true);
        StateText.color = new Color(StateText.color.r, StateText.color.g, StateText.color.b, 0); 
        StateImage.color = new Color(StateImage.color.r, StateImage.color.g, StateImage.color.b, 0);
        while (StateText.color.a < 1.0f)
        {
            StateText.color = new Color(StateText.color.r, StateText.color.g, StateText.color.b, StateText.color.a + (Time.deltaTime *3));
            StateImage.color = new Color(StateImage.color.r, StateImage.color.g, StateImage.color.b, StateImage.color.a + (Time.deltaTime *3));
            yield return null;
        }
    }
    IEnumerator StateDisappear()
    {
        StateText.color = new Color(StateText.color.r, StateText.color.g, StateText.color.b, 1);
        StateImage.color = new Color(StateImage.color.r, StateImage.color.g, StateImage.color.b, 1);
        while (StateText.color.a > 0.0f)
        {
            StateText.color = new Color(StateText.color.r, StateText.color.g, StateText.color.b, StateText.color.a - (Time.deltaTime * 3));
            StateImage.color = new Color(StateImage.color.r, StateImage.color.g, StateImage.color.b, StateImage.color.a - (Time.deltaTime * 3));
            yield return null;
        }
        State.SetActive(false);
    }
    public void Pass()
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Pass)
            {
                UIActiveActionEvent?.Invoke(this, action);
                break;
            }
        }
        IsListenState = false;
        IsDiscardState = false;
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    public void Chow()
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Chow)
            {
                if (action.OptionTiles.Count == 1)
                {
                    UIActiveActionEvent?.Invoke(this, action);
                    _handTilesUIViewer.SetBright();
                    isDraw = false;
                    break;
                }
                else
                {
                    _discardTileUIViewer.SetChowOptionOn(action);
                    _handTilesUIViewer.SetDark();
                    break;
                }
            }
        }
        IsListenState = false;
        IsDiscardState = false;
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    public void Pong()
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Pong)
            {
                UIActiveActionEvent?.Invoke(this, action);
                break;
            }
        }
        isDraw = false;
        IsListenState = false;
        IsDiscardState = false;
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    public void Kong()
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Kong || action.ID == Action.AdditionKong || action.ID == Action.ConcealedKong)
            {
                UIActiveActionEvent?.Invoke(this, action);
                break;
            }
        }
        IsListenState = false;
        IsDiscardState = false;
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    public void Listen()
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.ReadyHand)
            {
                IsListenState = true;
                IsDiscardState = false;
                List<TileSuits> listeningTiles = new List<TileSuits>();
                readyInfo = action.ReadyInfoTile;
                foreach (KeyValuePair<TileSuits, Dictionary<TileSuits, int>> key in action.ReadyInfoTile)
                {
                    listeningTiles.Add(key.Key);
                }
                for (int i = 0; i < 17; i++)
                {
                    foreach (TileSuits listeningTile in listeningTiles)
                    {
                        if (listeningTile == HandTileSuits[i])
                        {
                            CanDiscardList[i] = true;
                        }
                        else
                        {
                            CanDiscardList[i] = false;
                        }
                    }
                }
                _handTilesUIViewer.ListenSetOn(CanDiscardList);
                _discardTileUIViewer.SetListenOptionOn(action);
                break;
            }
        }
    }
    private void ListenOff()
    {
        IsListenState = false;
        _handTilesUIViewer.ListenSetOff();
        _discardTileUIViewer.SetListenOptionOff();
    }
    public void Winning()
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Win ||
                action.ID == Action.DrawnFromDeadWall ||
                action.ID == Action.SelfDrawnWin)
            {
                UIActiveActionEvent?.Invoke(this, action);
                break;
            }
        }
        IsListenState = false;
        IsDiscardState = false;
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    public void ChowSelect(int index)
    {
        _discardTileUIViewer.ActionUISetOff();
        foreach (ActionData action in Actions)
        {
            if (action.ID == Action.Chow)
            {
                List<TileSuits> tile = action.OptionTiles[index];
                List<List<TileSuits>> tilelist = new List<List<TileSuits>>();
                tilelist.Add(tile);
                action.OptionTiles = tilelist;
                UIActiveActionEvent?.Invoke(this, action);
            }
        }
        isDraw = false;
        IsListenState = false;
        IsDiscardState = false;
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    private void UIReset()
    {
        _discardTileUIViewer.ActionUISetOff();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
        IsListenState = false;
        IsDiscardState = false;
        isDraw = true;
        _handTilesUIViewer.SetBright();
    }

    public void CancelHosting()
    {
        _cancelHosting.Disappear();
        _hosting.Appear();
    }

    public void Hosting()
    {
        _cancelHosting.Appear();
        _hosting.Disappear();
    }

    public void test()
    {
        //var text = Tuple.Create("原", "你說的對，但《原神》是由米哈遊自主研發的全新開放世界冒險遊戲。 遊戲發生在一個被稱作「提瓦特」的幻想世界，在這裡，被神選中的人將被授予「神之眼」，導引元素之力。 你將扮演一位名為「旅行者」的神秘角色，在自由的旅行中邂逅性格各異、能力獨特的同伴們，和他們一起擊敗強敵，找回失散的親人——同時，逐步發掘「原神」的真相。");
        //List<int> index = new();
        //index.Add(1);
        //index.Add(2);
        //index.Add(3);
        //index.Add(4);
        //_waitingUI.SetPlayerHead(index);
        //ShowState("測試", 1000);
        //_socialUIButton.AddChat(text,40);
        //_hintUI.showActionHint(0, "原");
        //_hintUI.showActionHint(1, "吃");
        //_hintUI.showActionHint(2, "碰");
        //_hintUI.showActionHint(3, "槓");
        List<PlayerResultData> testData = new();

        {
            testData.Add(new());
            testData.Add(new());
            testData.Add(new());
            testData.Add(new());

            AvatarIndex.Clear();
            PlayerName.Clear();

            for (int i = 0; i < testData.Count; i++)
            {
                testData[i].Name = "test" + i.ToString();
                AvatarIndex.Add(i);
                PlayerName.Add(testData[i].Name);
            }
            testData[0].DoorWind = "東";
            testData[1].DoorWind = "南";
            testData[2].DoorWind = "西";
            testData[3].DoorWind = "北";
            testData[0].Scores = 1000;
            testData[1].Scores = 2000;
            testData[2].Scores = 3000;
            testData[3].Scores = 4000;
            testData[0].Score = 100;
            testData[1].Score = 200;
            testData[2].Score = 300;
            testData[3].Score = 400;

            testData[0].PointList = new PointType[0];
            testData[1].PointList = new PointType[0];
            testData[2].PointList = new PointType[0];
            testData[3].PointList = new PointType[0];
            //testData[0].PointList = new PointType[12];
            //testData[0].PointList[0].Describe = "Banker";
            //testData[0].PointList[0].Point = 0;
            //testData[0].PointList[1].Describe = "RemainingBanker";
            //testData[0].PointList[1].Point = 1;
            //testData[0].PointList[2].Describe = "NoTriplet";
            //testData[0].PointList[2].Point = 2;
            //testData[0].PointList[3].Describe = "ThreeConcealedTriplet";
            //testData[0].PointList[3].Point = 3;
            //testData[0].PointList[4].Describe = "FourConcealedTriplet";
            //testData[0].PointList[4].Point = 4;
            //testData[0].PointList[5].Describe = "FiveConcealedTriplet";
            //testData[0].PointList[5].Point = 5;
            //testData[0].PointList[6].Describe = "PongPong";
            //testData[0].PointList[6].Point = 6;
            //testData[0].PointList[7].Describe = "TripletDragon";
            //testData[0].PointList[7].Point = 7;
            //testData[0].PointList[8].Describe = "LittleThreeDragon";
            //testData[0].PointList[8].Point = 8;
            //testData[0].PointList[9].Describe = "BigThreeDragon";
            //testData[0].PointList[9].Point = 9;
            //testData[0].PointList[10].Describe = "RoundWind";
            //testData[0].PointList[10].Point = 10;
            //testData[0].PointList[11].Describe = "DoorWind";
            //testData[0].PointList[11].Point = 11;

            //testData[1].PointList = new PointType[8];
            //testData[1].PointList[0].Describe = "LittleFourWind";
            //testData[1].PointList[0].Point = 0;
            //testData[1].PointList[1].Describe = "BigFourWind";
            //testData[1].PointList[1].Point = 1;
            //testData[1].PointList[2].Describe = "CorrectFlower";
            //testData[1].PointList[2].Point = 2;
            //testData[1].PointList[3].Describe = "FlowerKong";
            //testData[1].PointList[3].Point = 3;
            //testData[1].PointList[4].Describe = "SevenRobsOne";
            //testData[1].PointList[4].Point = 4;
            //testData[1].PointList[5].Describe = "FlowerKing";
            //testData[1].PointList[5].Point = 5;
            //testData[1].PointList[6].Describe = "SingleTile";
            //testData[1].PointList[6].Point = 6;
            //testData[1].PointList[7].Describe = "DoorClear";
            //testData[1].PointList[7].Point = 7;

            //testData[2].PointList = new PointType[4];
            //testData[2].PointList[0].Describe = "SelfDrawn";
            //testData[2].PointList[0].Point = 0;
            //testData[2].PointList[1].Describe = "DoorClearAndSelfDrawn";
            //testData[2].PointList[1].Point = 1;
            //testData[2].PointList[2].Describe = "RobbingKong";
            //testData[2].PointList[2].Point = 2;
            //testData[2].PointList[3].Describe = "SelfDrawnOnKong";
            //testData[2].PointList[3].Point = 3;

            //testData[3].PointList = new PointType[1];
            //testData[3].PointList[0].Describe = "LastTileSelfDrawn";
            //testData[3].PointList[0].Point = 0;
            if (count < 4)
            {
                testData[count].Winner = true;
                count++;
            }
            else
            {
                count = 0;
            }
        }
        Settlement(testData);
    }
    //public void AddChat(List<Tuple<string, string>> text)
    //{
    //    _socialUIButton.AddChat(text);
    //}

    public void AddChat(Tuple<string, string> text, float time)
    {
        _socialUIButton.AddChat(text, time);
    }

    public void setWaiting(WaitingEventArgs waitingEventArgs)
    {
        _waitingUI.SetWaiting(waitingEventArgs);

        PlayerName.Clear();
        for (int i = 0; i < waitingEventArgs.Seats.Count; i++)
        {
            PlayerName.Add(waitingEventArgs.Seats[i].Name);
        }
    }
    public void setWaiting(WaitingEventArgs waitingEventArgs,List<int> index)
    {
        try
        {
            _waitingUI.SetWaiting(waitingEventArgs, index);
            AvatarIndex.Clear();
            for (int i = 0; i < index.Count; i++)
            {
                AvatarIndex.Add(index[i]);
            }

            PlayerName.Clear();
            for (int i = 0; i < waitingEventArgs.Seats.Count; i++)
            {
                PlayerName.Add(waitingEventArgs.Seats[i].Name);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            throw;
        }
        
    }

    private void CloseWaiting(object sender, EventArgs e)
    {
        WaitingUIObject.SetActive(false);
    }

    public void CloseWait()
    {
        WaitingUIObject.SetActive(false);
    }

    public void ShowError(string error)
    {
        ErrorText.text += error+"\n";
    }

    public void ShowActionHint(int index,string actionName)
    {
        _hintUI.showActionHint(index, actionName);
    }

    public SettingUIButton SettingUIButton
    {
        get => default;
        set
        {
        }
    }
    public UICountdownTimer UICountdownTimer
    {
        get => default;
        set
        {
        }
    }
    public HandTilesUI HandTilesUIViewer
    {
        get => default;
        set
        {
        }
    }
    public WinningSuggestUI WinningSuggestUIViewer
    {
        get => default;
        set
        {
        }
    }
    public SocialUIButton SocialUIButton
    {
        get => default;
        set
        {
        }
    }
    public SupportUIButton SupportUIButton
    {
        get => default;
        set
        {
        }
    }
    public ListeningHolesUI ListeningHolesUI
    {
        get => default;
        set
        {
        }
    }
    public DiscardTileUI DiscardTileUIViewer
    {
        get { return _discardTileUIViewer; }
        private set
        {
            _discardTileUIViewer = value;
        }
    }
    public SettlementScreen SettlementScreen
    {
        get => default;
        set
        {
        }
    }
}
