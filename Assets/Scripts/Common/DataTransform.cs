using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

namespace DataTransformNamespace
{
    public class DataTransform : MonoBehaviour
    {
        public static List<TileSuits> ReturnTileToIndex(string[] tiles)
        {
            if (tiles != null && tiles.Length > 0)
            {
                List<TileSuits> tileSuitsList = new List<TileSuits>();

                foreach (string tile in tiles)
                {
                    if (tile != null && tile.Length > 0)
                    {
                        if (tile[0] == '_')
                        {
                            string typeString = tile.Substring(1, 2);
                            if (Enum.TryParse(typeString, out TileSuits tileSuit))
                            {
                                // tileSuitsList.Add(tileSuit + 100);
                            }
                            else
                            {
                                Console.WriteLine($"Error: Invalid tile value '{tile}'. Possible values are: {string.Join(", ", Enum.GetNames(typeof(TileSuits)))}");
                            }
                        }
                        else
                        {
                            if (Enum.TryParse(tile, out TileSuits tileSuit))
                            {
                                tileSuitsList.Add(tileSuit);
                            }
                            // Handle error case if enum parsing fails
                        }
                    }
                    else if (tile.Length == 0) // add kong if tile is ""
                    {
                        tileSuitsList.Add(TileSuits.NULL);
                    }
                }

                return tileSuitsList;
            }
            else
            {
                return new List<TileSuits>();
            }
        }

        public static string[]? ReturnIndexToTile(List<TileSuits> tiles)
        {
            Dictionary<string, TileSuits> MajongToIndex = new Dictionary<string, TileSuits>();
            foreach (TileSuits tile in Enum.GetValues(typeof(TileSuits)))
            {
                MajongToIndex[Enum.GetName(typeof(TileSuits), tile)] = tile;
            }

            List<string> indexToMahjong = new List<string>(MajongToIndex.Keys);
            return tiles != null && tiles.Count > 0 ? tiles.Select(tile => indexToMahjong[(int)tile]).ToArray() : null;

            /*if (tiles == null || tiles.Count == 0)
            {
                return null;
            }

            Dictionary<string, TileSuits> MajongToIndex = new Dictionary<string, TileSuits>();
            foreach (TileSuits tile in Enum.GetValues(typeof(TileSuits)))
            {
                MajongToIndex[Enum.GetName(typeof(TileSuits), tile)] = tile;
            }

            List<string> indexToMahjong = new List<string>(MajongToIndex.Keys);

            List<string> result = new List<string>();
            foreach (TileSuits tile in tiles)
            {
                result.Add(indexToMahjong[(int)tile]);
            }

            return result.ToArray();*/
        }

        public static List<SeatInfo> MapAllSeats(IEnumerable<SeatInfo> seats)
        {
            List<SeatInfo> processedSeats = new();
            foreach (SeatInfo seat in seats)
            {
                if(seat == null) continue;
                SeatInfo processedSeat = MapSeat(seat);
                processedSeats.Add(processedSeat);
            }
            return processedSeats;
        }

        public static SeatInfo MapSeat(SeatInfo seat)
        {
            List<List<TileSuits>> doorList = MapStringListsToTileSuitsLists(seat.Door);
            List<TileSuits> flowerList = ReturnTileToIndex(seat.Flowers);
            List<TileSuits> seaList = ReturnTileToIndex(seat.Sea);
            return seat.CloneWithTiles(doorList, flowerList, seaList);
        }

        public static List<List<TileSuits>> MapStringListsToTileSuitsLists(List<string[]> options)
        {
            //List<List<TileSuits>> Options = options != null ? options.ConvertAll(singleData => ReturnTileToIndex(singleData)) : new List<List<TileSuits>>();
            List<List<TileSuits>> option = new();
            if (options != null)
            {
                foreach (string[] singleData in options)
                {
                    List<TileSuits> convertedList = ReturnTileToIndex(singleData);
                    option.Add(convertedList);
                }
            }
            return option;
        }

        public static ActionData[] MapActionData(ActionData[] actionData, List<TileSuits> tiles)
        {
            ActionData[] processedActionData = new ActionData[actionData.Length];
            for(int i = 0;  i < processedActionData.Length; i++)
            {
                ActionData actionData1 = actionData[i];
                if(actionData1 != null)
                {
                    processedActionData[i] = new ActionData
                    {
                        ID = actionData1.ID
                    };
                    switch (actionData1.ID)
                    {
                        case Action.Pass:
                            break;
                        case Action.Discard:
                            processedActionData[i].OptionTiles = (actionData1.Options == null) ? new List<List<TileSuits>>(){ tiles.ToList() } : MapStringListsToTileSuitsLists(actionData1.Options);
                            break;
                        case Action.Chow:
                        case Action.Pong:
                        case Action.Kong:
                        case Action.AdditionKong:
                        case Action.ConcealedKong:
                            processedActionData[i].OptionTiles = MapStringListsToTileSuitsLists(actionData1.Options);
                            break;
                        case Action.ReadyHand:
                            processedActionData[i].OptionTiles = MapStringListsToTileSuitsLists(actionData1.Options);
                            processedActionData[i].ReadyInfoTile = MapKeyToTileKey(actionData1.ReadyInfo);
                            break;
                        case Action.Win:
                        case Action.DrawnFromDeadWall:
                        case Action.SelfDrawnWin:
                            processedActionData[i].OptionTiles = (actionData1.Options == null) ? new List<List<TileSuits>>() { tiles.ToList() } : MapStringListsToTileSuitsLists(actionData1.Options);
                            break;
                    };
                }
            }
            return processedActionData;
        }

        public static Dictionary<TileSuits, Dictionary<TileSuits, int>> MapKeyToTileKey(Dictionary<string, Dictionary<string, int>> key)
        {
            Dictionary<TileSuits, Dictionary<TileSuits, int>> tileKey = new Dictionary<TileSuits, Dictionary<TileSuits, int>>();

            foreach (var kvp in key)
            {
                if (Enum.TryParse<TileSuits>(kvp.Key, out TileSuits tileSuit))
                {
                    Dictionary<TileSuits, int> innerDictionary = new Dictionary<TileSuits, int>();

                    foreach (var innerKvp in kvp.Value)
                    {
                        if (Enum.TryParse<TileSuits>(innerKvp.Key, out TileSuits innerTileSuit))
                        {
                            innerDictionary[innerTileSuit] = innerKvp.Value;
                        }
                        else
                        {
                        }
                    }

                    tileKey[tileSuit] = innerDictionary;
                }
            }
            return tileKey;
        }

        public static List<PlayerResultData> MapAllResult(IEnumerable<PlayerResultData> playerResults)
        {
            List<PlayerResultData> playerResultDatas = new List<PlayerResultData>();
            foreach (PlayerResultData Result in playerResults)
            {
                PlayerResultData processedResult = MapResult(Result);
                playerResultDatas.Add(processedResult);
            }
            return playerResultDatas;
        }

        public static PlayerResultData MapResult(PlayerResultData result)
        {
            List<List<TileSuits>> doorList = MapStringListsToTileSuitsLists(result.Door);
            List<TileSuits> tileList = ReturnTileToIndex(result.Tiles);
            List<TileSuits> flowerList = ReturnTileToIndex(result.Flowers);
            return result.CloneWithTiles(doorList, tileList, flowerList);
        }
    }
}