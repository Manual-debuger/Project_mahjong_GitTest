using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DataTransformNamespace;
using System.Threading.Tasks;

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

    [SerializeField] private GameObject InGameUI;
    [SerializeField] private GameObject SettlementUI;
    public event EventHandler<DiscardTileEventArgs> DiscardTileEvent;
    public event EventHandler<TileSuitEventArgs> OnTileBeHoldingEvent;
    public event EventHandler<TileSuitEventArgs> LeaveTileBeHoldingEvent;
    public event EventHandler<FloatEventArgs> SetMusicEvent;
    public event EventHandler<FloatEventArgs> SetSoundEvent;
    public event EventHandler<ActionData> UIActiveActionEvent;

    public bool IsListenState = false;
    public bool IsDiscardState = false;
    public bool[] CanDiscardList = new bool[17];
    private ActionData[] Actions;
    private ReadyInfoType readyInfo;
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
        _discardTileUIViewer.ActionEvent += UIActiveAction;
        _discardTileUIViewer.ListenOnActionEvent += ListenOn;
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
            TimeOut();
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

            IsListenState = false;
            IsDiscardState = false;
            _discardTileUIViewer.ActionUISetOff();
            _discardTileUIViewer.SetListenOptionOff();
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
            List<TileSuits> tileList = new List<TileSuits> { HandTileSuits[e.TileIndex] };
            string[] tileArray = DataTransform.ReturnIndexToTile(tileList);
            string Tile = tileArray.ToString();
            foreach (KeyValuePair<string, ListeningTilesType> key in readyInfo.key)
            {
                if (Tile == key.Key)
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
    public void SetHandTile(List<TileSuits> tileSuits, bool IsDrawing)
    {
        if (tileSuits.Count == 17)
        {
            IsDrawing = true;
        }
        if (IsDrawing)
        {
            HandTileSuits[16] = tileSuits[tileSuits.Count - 1];
            tileSuits.RemoveAt(tileSuits.Count - 1);
        }
        else
        {
            HandTileSuits[16] = TileSuits.NULL;
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
    public void ActionUISet(ActionData[] actions)
    {
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
        //DiscardTileUIViewer.ActionUISetOn(actions);
    }
    public void ActionUISet(ActionData[] actions, long time)
    {
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
    private void UIActiveAction(object sender, ActionData e)
    {
        IsListenState = false;
        DiscardTileUIViewer.ActionUISetOff();
        UIActiveActionEvent?.Invoke(this, e);
    }
    private void ListenOn(object sender, ActionData e)
    {
        IsListenState = true;
        List<string> tilesStrList = new List<string>();
        foreach (KeyValuePair<string, ListeningTilesType> key in e.ReadyInfo.key)
        {
            tilesStrList.Add(key.Key);
        }
        string[] tilesStrArray = tilesStrList.ToArray();
        List<TileSuits> tilesList = DataTransform.ReturnTileToIndex(tilesStrArray);
        List<int> ListenIndex = new List<int>();
        foreach (TileSuits tile in tilesList)
        {
            for (int i = 0; i < 17; i++)
            {
                if (HandTileSuits[i] == tile)
                {
                    ListenIndex.Add(i);
                }
            }
        }
        _handTilesUIViewer.ListenSetOn(ListenIndex);
    }
    private void ListenOff()
    {
        IsListenState = false;
        _handTilesUIViewer.ListenSetOff();
        _discardTileUIViewer.SetListenOptionOff();
    }
    public async void Settlement(List<SeatInfo> seatInfos,long time)
    {
        InGameUI.SetActive(false);
        SettlementUI.SetActive(true);
        _settlementScreen.SetSettlement(seatInfos);
        await Task.Delay((int)time);
        InGameUI.SetActive(true);
        SettlementUI.SetActive(false);
        for (int i = 0; i < 17; i++)
        {
            HandTileSuits[i] = TileSuits.NULL;
        }
        HandTileUISet();
        TimeOut();
        CountTime = 0;
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
        _discardTileUIViewer.ActionUISetOff();
        _discardTileUIViewer.SetListenOptionOff();
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
        _discardTileUIViewer.ActionUISetOff();
        _discardTileUIViewer.SetListenOptionOff();
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
        IsListenState = false;
        IsDiscardState = false;
        _discardTileUIViewer.ActionUISetOff();
        _discardTileUIViewer.SetListenOptionOff();
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
        _discardTileUIViewer.ActionUISetOff();
        _discardTileUIViewer.SetListenOptionOff();
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
                if (action.OptionTiles.Count == 1)
                {
                    UIActiveActionEvent?.Invoke(this, action);
                    _handTilesUIViewer.SetBright();
                    for (int i = 0; i < 17; i++)
                    {
                        CanDiscardList[i] = false;
                    }
                }
                else
                {
                    IsListenState = true;
                    IsDiscardState = false;
                    _discardTileUIViewer.SetListenOptionOn(action);
                }
                break;
            }
        }
        _discardTileUIViewer.ActionUISetOff();
        //_discardTileUIViewer.SetListenOptionOff();
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
        _discardTileUIViewer.ActionUISetOff();
        _discardTileUIViewer.SetListenOptionOff();
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    public void ChowSelect(int index)
    {
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
        _discardTileUIViewer.SetChowOptionOff();
        IsListenState = false;
        IsDiscardState = false;
        _discardTileUIViewer.ActionUISetOff();
        _discardTileUIViewer.SetListenOptionOff();
        _handTilesUIViewer.SetBright();
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
    }
    private void TimeOut()
    {
        for (int i = 0; i < 17; i++)
        {
            CanDiscardList[i] = false;
        }
        IsListenState = false;
        IsDiscardState = false;
        _handTilesUIViewer.SetBright();
        _discardTileUIViewer.SetListenOptionOff();
        _discardTileUIViewer.SetChowOptionOff();
        _discardTileUIViewer.ActionUISetOff();
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
