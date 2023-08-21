using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using APIDataNamespace;

public class API : MonoBehaviour
{
    public static API Instance;

    private ClientWebSocket socket;
    private CancellationTokenSource cancellationTokenSource;
    private Uri uri;

    private bool isLoggedInAndEnterTable = false; // 僅在初次按下進入遊戲登入且成功進桌時會被設定

    private long Time;
    private long? PlayingDeadline;
    private string NowState = "";
    private string NextState = "";

    public event EventHandler<RandomSeatEventArgs> RandomSeatEvent;
    public event EventHandler<DecideBankerEventArgs> DecideBankerEvent;
    public event EventHandler<OpenDoorEventArgs> OpenDoorEvent;
    public event EventHandler<GroundingFlowerEventArgs> GroundingFlowerEvent;
    public event EventHandler<PlayingEventArgs> PlayingEvent;
    public event EventHandler<WaitingActionEventArgs> WaitingActionEvent;

    public event EventHandler<PassActionEventArgs> PassEvent;
    public event EventHandler<DiscardActionEventArgs> DiscardEvent;
    public event EventHandler<ChowActionEventArgs> ChowEvent;
    public event EventHandler<PongActionEventArgs> PongEvent;
    public event EventHandler<KongActionEventArgs> KongEvent;
    public event EventHandler<DrawnActionEventArgs> DrawnEvent;
    public event EventHandler<GroundingFlowerActionEventArgs> GroundingFlowerActionEvent;


    private void Awake()
    {
        Instance = this;
        uri = new Uri("ws://localhost:80/api/v1/games/mahjong16");
    }

    private async void Start()
    {
        await Connect();
    }

    private async Task Connect()
    {
        socket = new ClientWebSocket();
        cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await socket.ConnectAsync(uri, cancellationTokenSource.Token);
            Debug.Log("WebSocket connected.");
            await Login();
            _ = StartListening();
        }
        catch (Exception ex)
        {
            Debug.LogError($"WebSocket connection failed: {ex.Message}");
            throw;
        }
    }

    private async Task StartListening()
    {
        try
        {
            var receiveBuffer = new List<byte>();
            var bufferSize = 1024; // Use a larger buffer size
            while (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[bufferSize];
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    receiveBuffer.AddRange(buffer.Take(result.Count)); // Add received bytes to the buffer

                    if (result.EndOfMessage) // Check if it's the end of the message
                    {
                        var message = Encoding.UTF8.GetString(receiveBuffer.ToArray());
                        // Handle incoming message
                        HandleMessage(message);

                        receiveBuffer.Clear(); // Clear the buffer for the next message
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //Debug.LogError($"WebSocket listening error: {ex.Message}");
            throw;
        }
    }

    public async Task Login(string token = null)
    {
        var requestData = new LoginObject
        {
            Path = Path.Login,
            Data = new LoginData
            {
                IsGuest = true,
                Token = token
            }
        };

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("Login request: " + jsonData);
        await SendDataToServer(jsonData);
    }

    public async Task TableEnter(object config = null)
    {
        // Simulate sending data
        var requestData = new TableEnterObject
        {
            Path = "game.table.enter",
            Data = config
        };

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("TableEnter request: " + jsonData);
        await SendDataToServer(jsonData);
    }

    private void HandleMessage(string data)
    {
        // Parse the incoming JSON data into a Unity C# object
        MessageObject message = JsonConvert.DeserializeObject<MessageObject>(data);

        //if (message.Path != Path.TableEvent || (message.Path == Path.TableEvent && message.Data.Index == 0))
        //{
        Debug.Log("From Server: " + data);
        // Switch based on the Path to handle different message types
        switch (message.Path)
        {
            case Path.Ack:
                HandleAck();
                break;
            case Path.Login:
                HandleLogin();
                break;
            case Path.TableEnter:
                HandleTableEnter();
                break;
            case Path.TableEvent:
                HandleTableEvent(message.Data);
                break;
            case Path.TablePlay:
                HandleTablePlay(message.Data);
                break;
            case Path.TableResult:
                HandleTableResult(message.Data);
                break;
            default:
                Debug.LogError("Unknown message Path: " + message.Path);
                break;
        }
        //}
    }

    // Implement individual message handlers for each message type based on the enums in Game.tso.ts
    private void HandleAck()
    {
        Debug.Log("Ack");
    }

    private async void HandleLogin()
    {
        await TableEnter();
    }

    private void HandleTableEnter()
    {
        if (!isLoggedInAndEnterTable)
        {
            isLoggedInAndEnterTable = true;
        }
    }

    private void HandleTableEvent(MessageData eventData)
    {
        try
        {
            Debug.Log("TableEvent: " + eventData.State);

            Time = eventData.Time;
            switch (eventData.State)
            {
                case "Waiting":
                    NowState = eventData.State;
                    break;
                case "RandomSeat":
                    APIData.HandleRandomSeatState(eventData);
                    break;
                case "DecideBanker":
                    APIData.HandleDecideBankerState(eventData);
                    break;
                case "OpenDoor":
                    APIData.HandleOpenDoorState(eventData);
                    break;
                case "GroundingFlower":
                    APIData.HandleGroundingFlowerState(eventData);
                    break;
                case "SortingTiles":
                    //GameClass.instance.HandleSortingTiles(eventData);
                    break;
                case "Playing":
                    APIData.HandlePlayingState(eventData);
                    break;
                case "DelayPlaying":
                    //GameClass.instance.HandleDelayPlayingState(eventData);
                    break;
                case "WaitingAction":
                    APIData.HandleWaitingActionState(eventData);
                    break;
                case "HandEnd":
                    //GameClass.instance.HandleHandEndState(eventData);
                    break;
                case "GameEnd":
                    //GameClass.instance.HandleGameEndState(eventData);
                    break;
                case "Closing":
                    //GameClass.instance.HandleClosingState(eventData);
                    break;
                default:
                    Debug.LogError("Unknown state: " + eventData.State);
                    break;
            }

            // GameClass.instance.UpdateEventTimer(); eventTimer = System.DateTime.Now.Ticks / 10000;
        }
        catch (Exception e)
        {
            throw;
        }

        /*if (GameClass.instance == null)
        {
            Global.UpdateTableBasicInfo(eventData);
            return;
        }*/
    }

    private void HandleTablePlay(MessageData playData)
    {
        if (playData != null)
        {
            switch (playData.Action)
            {
                case Action.Pass:
                    APIData.HandlePassAction(playData);
                    break;
                case Action.Discard:
                    APIData.HandleDiscardAction(playData);
                    break;
                case Action.Chow:
                    APIData.HandleChowAction(playData);
                    break;
                case Action.Pong:
                    APIData.HandlePongAction(playData);
                    break;
                case Action.Kong:
                case Action.AdditionKong:
                case Action.ConcealedKong:
                    APIData.HandleKongAction(playData);
                    break;
                case Action.ReadyHand:
                    break;
                case Action.Win:
                    break;
                case Action.Drawn: // Action=9 
                    APIData.HandleDrawnAction(playData);
                    break;
                case Action.GroundingFlower:
                    APIData.HandleGroundingFlowerAction(playData);
                    break;
                case Action.DrawnFromDeadWall:
                    break;
                case Action.SelfDrawnWin:
                    break;
            }
        }
        else
        {
            //CloseAllBtn();
        }
    }

    private void HandleTableResult(MessageData data)
    {
        // Implement the logic for handling the table result message
    }

    // Call this method to send data to the WebSocket server
    public async Task SendDataToServer(string data)
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            await SendData(data);
        }
    }

    private async Task SendData(string data)
    {
        var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationTokenSource.Token);
    }

    private async Task CloseConnection()
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            cancellationTokenSource.Cancel();
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by user", cancellationTokenSource.Token);
            Debug.Log("WebSocket connection closed.");
        }
    }

    private void OnDestroy()
    {
        CloseConnection().Wait();
    }
}