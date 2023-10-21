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

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else if(Instance == null)
            Instance = this;

        uri = new Uri("wss://willime.live/api/v1/games/mahjong16");
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
            TXResponse TXResponse = JsonConvert.DeserializeObject<TXResponse>(PlayerPrefs.GetString("TXResponseData"));
            await Login(TXResponse.Token);
            await StartListening();
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
            Debug.LogError($"WebSocket listening error: {ex.Message}");
            throw;
        }
    }

    public async Task Login(string token = null)
    {
        LoginObject requestData = new LoginObject
        {
            Path = Path.Login,
            Data = new LoginData
            {
                IsGuest = true,
                Token = token
            }
        };

        string jsonData = JsonConvert.SerializeObject(requestData);
        Debug.Log("Login request: " + jsonData);
        await SendData(jsonData);
    }

    public async Task TableEnter(object config = null)
    {
        TableEnterObject requestData = new TableEnterObject
        {
            Path = Path.TableEnter,
            Data = config
        };

        string jsonData = JsonConvert.SerializeObject(requestData);
        Debug.Log("TableEnter request: " + jsonData);
        await SendData(jsonData);
    }

    private void HandleMessage(string data)
    {
        // Parse the incoming JSON data into a Unity C# object
        MessageObject message = JsonConvert.DeserializeObject<MessageObject>(data);

        
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
            case Path.TableAutoPlay:
                APIData.HandleAutoPlay();
                break;
            default:
                Debug.LogError("Unknown message Path: " + message.Path);
                break;
        }
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
            // Debug.Log("TableEvent: " + eventData.State);

            APIData.Time = eventData.Time;
            switch (eventData.State)
            {
                case "Waiting":
                    APIData.NowState = eventData.State;
                    APIData.HandleWaitingState(eventData);
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
                    APIData.HandleHandEndState(eventData);
                    break;
                case "GameEnd":
                    APIData.HandleGameEndState(eventData);
                    break;
                case "Closing":
                    APIData.HandleClosingState(eventData);
                    break;
                default:
                    Debug.LogError("Unknown state: " + eventData.State);
                    break;
            }
        }
        catch
        {
            throw;
        }
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
                    APIData.HandleReadyHandAction(playData);
                    break;
                case Action.Win:
                    APIData.HandleWinAction(playData);
                    break;
                case Action.Drawn: // Action=9 
                    APIData.HandleDrawnAction(playData);
                    break;
                case Action.GroundingFlower:
                    APIData.HandleGroundingFlowerAction(playData);
                    break;
                case Action.DrawnFromDeadWall:
                    APIData.HandleDrawnFromDeadWallAction(playData);
                    break;
                case Action.SelfDrawnWin:
                    APIData.HandleSelfDrawnWinAction(playData);
                    break;
            }
        }
    }

    private void HandleTableResult(MessageData data)
    {
        try
        {
            APIData.HandleTableResult(data);
        }
        catch
        {
            throw;
        }
    }

    // Call this method to send data to the WebSocket server
    public async Task SendData(string data)
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            await SocketSendData(data);
        }
    }

    private async Task SocketSendData(string data)
    {
        var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationTokenSource.Token);
    }

    public void CloseConnection()
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            cancellationTokenSource.Cancel();
            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by user", cancellationTokenSource.Token);
            Debug.Log("WebSocket connection closed.");
        }
    }

    private void OnDestroy()
    {
        CloseConnection();
    }
}