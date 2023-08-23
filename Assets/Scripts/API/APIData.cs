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
        public static event EventHandler<DrawnActionEventArgs> DrawnEvent;
        public static event EventHandler<GroundingFlowerActionEventArgs> GroundingFlowerActionEvent;

        private void Awake()
        {
            instance = this;
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
                DecideBankerEventArgs decideBankerEventArgs = new(eventData.BankerIndex, eventData.RemainingBankerCount);
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

                OpenDoorEventArgs openDoorEventArgs = new(eventData.WallCount, tileSuitsList, processedSeats);

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

                GroundingFlowerEventArgs groundingFlowerEventArgs = new(eventData.WallCount, tileSuitsList, processedSeats);

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
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                List<TileSuits> tileSuitsList = DataTransform.ReturnTileToIndex(eventData.Tiles);

                PlayingEventArgs playingEventArgs = new(eventData.PlayingIndex, eventData.PlayingDeadline, eventData.WallCount, tileSuitsList, processedSeats);

                // Playing State not change until action
                if (PlayingDeadline != eventData.PlayingDeadline)
                {
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
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);
                List<TileSuits> tileSuitsList = DataTransform.ReturnTileToIndex(eventData.Tiles);

                WaitingActionEventArgs waitingActionEventArgs = new(eventData.PlayingIndex, eventData.PlayingDeadline, eventData.WallCount, tileSuitsList, eventData.Actions, processedSeats);

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
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);

                HandEndEventArgs handEndEventArgs = new(processedSeats);

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
                List<SeatInfo> processedSeats = DataTransform.MapAllSeats(eventData.Seats);

                HandEndEventArgs handEndEventArgs = new(processedSeats);

                HandEndEvent?.Invoke(instance, handEndEventArgs);
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

                HandEndEventArgs handEndEventArgs = new(processedSeats);

                HandEndEvent?.Invoke(instance, handEndEventArgs);
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

        //Debug.Log("2222222 From Server: " + JsonConvert.SerializeObject(eventData));
        public static void HandleDrawnAction(MessageData playData)
        {
            //Debug.Log("2222222 From Server: " + JsonConvert.SerializeObject(eventData));

            DrawnActionEventArgs drawnActionEventArgs = new(playData.Index, playData.Action, playData.DrawnCount);

            DrawnEvent?.Invoke(instance, drawnActionEventArgs);
        }

        public static void HandleGroundingFlowerAction(MessageData playData)
        {
            //Debug.Log("2222222 From Server: " + JsonConvert.SerializeObject(eventData));

            GroundingFlowerActionEventArgs groundingFlowerActionEventArgs = new(playData.Index, playData.Action, playData.DrawnCount);

            GroundingFlowerActionEvent?.Invoke(instance, groundingFlowerActionEventArgs);
        }

        public async void HandleClickAction(ActionData actionData, int index)
        {
            // 可以選擇吃的牌型
            // Selectable Chow Patterns
            if (actionData.ID == Action.Chow && actionData.OptionTiles.Count >= 2)
            {
                
            }
            // 可以選擇槓的牌型
            // Selectable Kong Patterns
            else if ((actionData.ID == Action.AdditionKong || actionData.ID == Action.ConcealedKong) && actionData.OptionTiles.Count >= 2)
            {
                
            }
            else if (actionData.ID == Action.ReadyHand)
            {
                // 設定聽牌
                // Set ReadyHand

            }
            else
            {
                TablePlayObject requestData = new TablePlayObject
                {
                    Path = Path.TablePlay,
                    Data = new TablePlayData
                    {
                        Index = index,
                        Action = actionData.ID,
                        Option = actionData.OptionTiles != null ? DataTransform.ReturnIndexToTile(actionData.OptionTiles[0]) : null
                    }
                };
                string jsonData = JsonConvert.SerializeObject(requestData);
                Debug.Log("TablePlay request: " + jsonData);
                await API.Instance.SendData(jsonData);
            }
        }
    }
}