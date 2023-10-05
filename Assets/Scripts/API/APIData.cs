using System;
using System.Collections.Generic;
using UnityEngine;
using DataTransformNamespace;
using Newtonsoft.Json;


namespace APIDataNamespace
{
    public class APIData : MonoBehaviour
    {
        public static APIData instance;

        public static long Time;
        public static long? PlayingDeadline;
        public static string NowState = "";
        public static string NextState = "";

        public static event EventHandler<RandomSeatEventArgs> RandomSeatEvent;
        public static event EventHandler<DecideBankerEventArgs> DecideBankerEvent;
        public static event EventHandler<OpenDoorEventArgs> OpenDoorEvent;
        public static event EventHandler<GroundingFlowerEventArgs> GroundingFlowerEvent;
        public static event EventHandler<PlayingEventArgs> PlayingEvent;
        public static event EventHandler<WaitingActionEventArgs> WaitingActionEvent;
        public static event EventHandler<HandEndEventArgs> HandEndEvent;
        public static event EventHandler<GameEndEventArgs> GameEndEvent;
        public static event EventHandler<ClosingEventArgs> ClosingEvent;

        public static event EventHandler<PassActionEventArgs> PassEvent;
        public static event EventHandler<DiscardActionEventArgs> DiscardEvent;
        public static event EventHandler<ChowActionEventArgs> ChowEvent;
        public static event EventHandler<PongActionEventArgs> PongEvent;
        public static event EventHandler<KongActionEventArgs> KongEvent;
        public static event EventHandler<ReadyHandActionEventArgs> ReadyHandEvent;
        public static event EventHandler<WinActionEventArgs> WinEvent;
        public static event EventHandler<DrawnActionEventArgs> DrawnEvent;
        public static event EventHandler<GroundingFlowerActionEventArgs> GroundingFlowerActionEvent;
        public static event EventHandler<DrawnFromDeadWallActionEventArgs> DrawnFromDeadWallActionEvent;
        public static event EventHandler<SelfDrawnWinActionEventArgs> SelfDrawnWinActionEvent;

        public static event EventHandler<ResultEventArgs> ResultEvent;

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else if (instance == null)
            {
                instance = this;
            }
        }

        public static void HandleWaitingState(MessageData eventData)
        {
            try
            {
                NowState = eventData.State;
            }
            catch
            {
                throw;
            }
        }

        public static void HandleRandomSeatState(MessageData eventData)
        {
            try
            {
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                RandomSeatEventArgs randomSeatEventArgs = new(eventData.Index, processedSeats);
                if (NowState != eventData.State)
                {
                    NowState = eventData.State;
                    RandomSeatEvent?.Invoke(instance, randomSeatEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void HandleDecideBankerState(MessageData eventData)
        {
            try
            {
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                DecideBankerEventArgs decideBankerEventArgs = new((int)eventData.BankerIndex, eventData.RemainingBankerCount, processedSeats);
                if (NowState != eventData.State)
                {
                    NowState = eventData.State;
                    DecideBankerEvent?.Invoke(instance, decideBankerEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void HandleOpenDoorState(MessageData eventData)
        {
            try
            {
                List<TileSuits> tileSuitsList = DataTransform.ReturnTileToIndex(eventData.Tiles);
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);

                OpenDoorEventArgs openDoorEventArgs = new((int)eventData.WallCount, tileSuitsList, processedSeats);

                if (NowState != eventData.State)
                {
                    NowState = eventData.State;
                    OpenDoorEvent?.Invoke(instance, openDoorEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void HandleGroundingFlowerState(MessageData eventData)
        {
            try
            {
                List<TileSuits> tileSuitsList = DataTransform.ReturnTileToIndex(eventData.Tiles);
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);

                GroundingFlowerEventArgs groundingFlowerEventArgs = new((int)eventData.WallCount, tileSuitsList, processedSeats);

                if (NowState != eventData.State)
                {
                    NowState = eventData.State;
                    GroundingFlowerEvent?.Invoke(instance, groundingFlowerEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void HandlePlayingState(MessageData eventData)
        {
            try
            {
                // Playing State not change until action
                if (PlayingDeadline != eventData.PlayingDeadline)
                {
                    long playingtimeLeft = (long)eventData.PlayingDeadline - eventData.Time;
                    List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                    List<TileSuits> tileSuitsList = DataTransform.ReturnTileToIndex(eventData.Tiles);
                    ActionData[] actionDatas = (eventData.Actions != null) ? DataTransform.MapActionData(eventData.Actions, tileSuitsList) : null;

                    PlayingEventArgs playingEventArgs = new((int)eventData.PlayingIndex, playingtimeLeft, (int)eventData.WallCount, tileSuitsList, actionDatas, processedSeats);
                    PlayingDeadline = eventData.PlayingDeadline;
                    PlayingEvent?.Invoke(instance, playingEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void HandleWaitingActionState(MessageData eventData)
        {
            try
            {
                long playingtimeLeft = (long)eventData.PlayingDeadline - eventData.Time;
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                List<TileSuits> tileSuitsList = DataTransform.ReturnTileToIndex(eventData.Tiles);
                ActionData[] actionDatas = (eventData.Actions != null) ? DataTransform.MapActionData(eventData.Actions, tileSuitsList) : null;

                WaitingActionEventArgs waitingActionEventArgs = new((int)eventData.PlayingIndex, playingtimeLeft, (int)eventData.WallCount, tileSuitsList, actionDatas, processedSeats);

                // Playing State not change until action
                if (PlayingDeadline != eventData.PlayingDeadline)
                {
                    PlayingDeadline = eventData.PlayingDeadline;
                    WaitingActionEvent?.Invoke(instance, waitingActionEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public static void HandleHandEndState(MessageData eventData)
        {
            try
            {
                long playingtimeLeft = (long)eventData.NextStateTime - eventData.Time;
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                HandEndEventArgs handEndEventArgs = new(playingtimeLeft, processedSeats);

                // Playing State not change until action
                if (NowState != eventData.State)
                {
                    HandEndEvent?.Invoke(instance, handEndEventArgs);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public static void HandleGameEndState(MessageData eventData)
        {
            try
            {
                long playingtimeLeft = (long)eventData.NextStateTime - eventData.Time;
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                GameEndEventArgs gameEndEventArgs = new(playingtimeLeft, processedSeats);

                GameEndEvent?.Invoke(instance, gameEndEventArgs);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public static void HandleClosingState(MessageData eventData)
        {
            try
            {
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                ClosingEventArgs closingEventArgs = new(processedSeats);

                ClosingEvent?.Invoke(instance, closingEventArgs);
                API.Instance.CloseConnection();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void HandlePassAction(MessageData playData)
        {
            PassActionEventArgs passActionEventArgs = new(playData.Index, playData.Action);

            PassEvent?.Invoke(instance, passActionEventArgs);
        }

        public static void HandleDiscardAction(MessageData playData)
        {
            List<TileSuits> optionTile = DataTransform.ReturnTileToIndex(playData.Option);

            DiscardActionEventArgs discardActionEventArgs = new(playData.Index, playData.Action, optionTile);

            DiscardEvent?.Invoke(instance, discardActionEventArgs);
        }

        public static void HandleChowAction(MessageData playData)
        {
            List<List<TileSuits>> optionsTile = DataTransform.MapStringListsToTileSuitsLists(playData.Options);

            ChowActionEventArgs chowActionEventArgs = new(playData.Index, playData.Action, optionsTile);

            ChowEvent?.Invoke(instance, chowActionEventArgs);
        }

        public static void HandlePongAction(MessageData playData)
        {
            List<List<TileSuits>> optionsTile = DataTransform.MapStringListsToTileSuitsLists(playData.Options);

            PongActionEventArgs pongActionEventArgs = new(playData.Index, playData.Action, optionsTile);

            PongEvent?.Invoke(instance, pongActionEventArgs);
        }

        public static void HandleKongAction(MessageData playData)
        {
            List<List<TileSuits>> optionsTileSuitsList = DataTransform.MapStringListsToTileSuitsLists(playData.Options);

            KongActionEventArgs kongActionEventArgs = new(playData.Index, playData.Action, optionsTileSuitsList);

            KongEvent?.Invoke(instance, kongActionEventArgs);
        }

        public static void HandleReadyHandAction(MessageData playData)
        {
            List<TileSuits> optionTile = DataTransform.ReturnTileToIndex(playData.Option);

            ReadyHandActionEventArgs groundingFlowerActionEventArgs = new(playData.Index, playData.Action, optionTile);

            ReadyHandEvent?.Invoke(instance, groundingFlowerActionEventArgs);
        }
        public static void HandleWinAction(MessageData playData)
        {
            WinActionEventArgs groundingFlowerActionEventArgs = new(playData.Index, playData.Action);

            WinEvent?.Invoke(instance, groundingFlowerActionEventArgs);
        }

        public static void HandleDrawnAction(MessageData playData)
        {
            DrawnActionEventArgs drawnActionEventArgs = new(playData.Index, playData.Action, (int)playData.DrawnCount);

            DrawnEvent?.Invoke(instance, drawnActionEventArgs);
        }

        public static void HandleGroundingFlowerAction(MessageData playData)
        {
            GroundingFlowerActionEventArgs groundingFlowerActionEventArgs = new(playData.Index, playData.Action, (int)playData.DrawnCount);

            GroundingFlowerActionEvent?.Invoke(instance, groundingFlowerActionEventArgs);
        }
        
        public static void HandleDrawnFromDeadWallAction(MessageData playData)
        {
            DrawnFromDeadWallActionEventArgs drawnFromDeadWallActionEventArgs = new(playData.Index, playData.Action);

            DrawnFromDeadWallActionEvent?.Invoke(instance, drawnFromDeadWallActionEventArgs);
        }

        public static void HandleSelfDrawnWinAction(MessageData playData)
        {
            SelfDrawnWinActionEventArgs selfDrawnWinActionEventArgs = new(playData.Index, playData.Action);

            SelfDrawnWinActionEvent?.Invoke(instance, selfDrawnWinActionEventArgs);
        }

        public static void HandleTableResult(MessageData resultData)
        {
            List<PlayerResultData> playerResultData = DataTransform.MapAllResult(resultData.Results);

            ResultEventArgs resultEventArgs = new(playerResultData);

            ResultEvent?.Invoke(instance, resultEventArgs);
        }
        
        public static void HandleAutoPlay()
        {
            // Global.gameData.AutoPlaying = !Global.gameData.AutoPlaying;
        }

        public async void HandleClickAction(ActionData actionData)
        {
            var requestData = new
            {
                Path = Path.TablePlay,
                Data = new
                {
                    Action = actionData.ID,
                    Option = actionData.OptionTiles != null ? DataTransform.ReturnIndexToTile(actionData.OptionTiles[0]) : null
                }
            };
            string jsonData = JsonConvert.SerializeObject(requestData);
            Debug.Log("TablePlay request: " + jsonData);
            await API.Instance.SendData(jsonData);
        }

        public async void SentAutoPlay()
        {
            var requestData = new
            {
                Path = Path.TableAutoPlay
            };
            string jsonData = JsonConvert.SerializeObject(requestData);
            await API.Instance.SendData(jsonData);
        }
    }
}